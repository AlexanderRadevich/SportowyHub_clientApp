using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using SportowyHub.Messages;
using SportowyHub.Models;
using SportowyHub.Models.Api;
using SportowyHub.Services.Auth;
using SportowyHub.Services.Favorites;
using SportowyHub.Services.Listings;
using SportowyHub.Services.Locale;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.Sections;
using SportowyHub.Services.Toast;

namespace SportowyHub.ViewModels;

public partial class HomeViewModel(
    IListingsService listingsService,
    INavigationService nav,
    IToastService toastService,
    ITokenProvider authService,
    ISectionsService sectionsService,
    IFavoritesService favoritesService,
    ILocaleService localeService,
    ILogger<HomeViewModel> logger) : ObservableObject
{
    private const int PageSize = 20;
    private const int HotPicksCount = 6;
    private int _offset;
    private bool _hasMoreItems = true;

    public ObservableCollection<ListingCardItem> Listings { get; } = [];
    public ObservableCollection<ListingCardItem> HotPicks { get; } = [];
    public ObservableCollection<Section> Sections { get; } = [];

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial bool IsRefreshing { get; set; }

    [ObservableProperty]
    public partial bool IsEmpty { get; set; }

    [ObservableProperty]
    public partial bool HasSections { get; set; }

    [ObservableProperty]
    public partial bool HasHotPicks { get; set; }

    [ObservableProperty]
    public partial string? SelectedCondition { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalResultsText))]
    public partial int TotalResults { get; set; }

    public string TotalResultsText =>
        TotalResults > 0
            ? string.Format(Resources.Strings.AppResources.HomeShowingResults, TotalResults)
            : string.Empty;

    [ObservableProperty]
    public partial bool IsLoggedIn { get; set; }

    [RelayCommand]
    private async Task LoadFavorites(CancellationToken ct)
    {
        try
        {
            var user = await authService.GetCurrentUserAsync();
            IsLoggedIn = user is not null;
            if (user is not null)
            {
                await favoritesService.LoadFavoriteIdsAsync(ct);
                SyncFavoriteStates();
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to load favorite IDs");
        }
    }

    [RelayCommand]
    private async Task SelectCondition(string? condition, CancellationToken ct)
    {
        SelectedCondition = condition;
        await LoadListings(ct);
    }

    [RelayCommand]
    private async Task GoToCreateListing()
    {
        var user = await authService.GetCurrentUserAsync();
        if (user is null)
        {
            await nav.NavigateToLoginWithReturnUrlAsync("//home");
            return;
        }

        if (user.TrustLevel == Models.TrustLevels.Unverified)
        {
            await toastService.ShowError(Resources.Strings.AppResources.CreateListingPhoneRequired);
            return;
        }

        await nav.GoToAsync("create-edit-listing");
    }

    [RelayCommand]
    private async Task LoadListings(CancellationToken ct)
    {
        if (IsLoading)
        {
            return;
        }

        IsLoading = true;

        try
        {
            await FetchAndPopulateListingsAsync(ct);
        }
        catch (Exception ex)
        {
            await toastService.ShowError(ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task LoadSections(CancellationToken ct)
    {
        var locale = localeService.TwoLetterLanguageCode;
        var result = await sectionsService.GetSectionsAsync(locale, ct);
        if (result.IsSuccess)
        {
            Sections.Clear();
            foreach (var section in result.Data!.Sports)
            {
                Sections.Add(section);
            }
            HasSections = Sections.Count > 0;
        }
        else
        {
            logger.LogWarning("Failed to load sections: {Error}", result.ErrorMessage);
            HasSections = false;
        }
    }

    [RelayCommand]
    private async Task Refresh(CancellationToken ct)
    {
        try
        {
            await FetchAndPopulateListingsAsync(ct);
        }
        catch (Exception ex)
        {
            await toastService.ShowError(ex.Message);
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task LoadMoreListings(CancellationToken ct)
    {
        if (IsLoading || !_hasMoreItems)
        {
            return;
        }

        IsLoading = true;

        try
        {
            var (items, _) = await FetchPageAsync(_offset, ct);

            foreach (var listing in items)
            {
                Listings.Add(ToCardItem(listing));
            }

            _offset += PageSize;
            _hasMoreItems = items.Count >= PageSize;
        }
        catch (Exception ex)
        {
            await toastService.ShowError(ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task GoToListingDetail(ListingSummary listing)
    {
        await nav.GoToListingDetailAsync(listing.Id, listing.Title, listing.Price, listing.Currency, listing.City);
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task ToggleFavorite(ListingSummary listing, CancellationToken ct)
    {
        var user = await authService.GetCurrentUserAsync();
        if (user is null)
        {
            await nav.NavigateToLoginWithReturnUrlAsync("//home");
            return;
        }

        var wasFavorited = favoritesService.IsFavorite(listing.Id);

        if (wasFavorited)
        {
            var result = await favoritesService.RemoveAsync(listing.Id, ct);
            if (!result.IsSuccess)
            {
                await toastService.ShowError(result.ErrorMessage!);
                return;
            }
        }
        else
        {
            var result = await favoritesService.AddAsync(listing.Id, ct);
            if (!result.IsSuccess)
            {
                await toastService.ShowError(result.ErrorMessage!);
                return;
            }
        }

        var newState = !wasFavorited;
        foreach (var item in Listings.Concat(HotPicks))
        {
            if (item.Listing.Id == listing.Id)
            {
                item.IsFavorited = newState;
            }
        }
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        if (Application.Current is null)
        {
            return;
        }

        var currentTheme = Application.Current.UserAppTheme;
        var newTheme = currentTheme == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark;

        if (currentTheme == AppTheme.Unspecified)
        {
            newTheme = Application.Current.RequestedTheme == AppTheme.Dark
                ? AppTheme.Light
                : AppTheme.Dark;
        }

        var code = newTheme == AppTheme.Dark ? "dark" : "light";
        Preferences.Set("app_theme", code);
        Application.Current.UserAppTheme = newTheme;
        Helpers.StatusBarHelper.Apply(newTheme);
    }

    [RelayCommand]
    private void GoToSearch()
    {
        if (Shell.Current is Shell shell)
        {
            shell.CurrentItem = shell.Items[0].Items[1];
        }
    }

    [RelayCommand]
    private void GoToFavorites()
    {
        if (Shell.Current is Shell shell)
        {
            shell.CurrentItem = shell.Items[0].Items[2];
        }
    }

    [RelayCommand]
    private async Task GoToCart()
    {
        await toastService.ShowSuccess(Resources.Strings.AppResources.HomeCartComingSoon);
    }

    [RelayCommand]
    private void GoToFilteredSearch(Section section)
    {
        if (Shell.Current is Shell shell)
        {
            SearchViewModel.PendingNavigationSection = section;
            shell.CurrentItem = shell.Items[0].Items[1];
        }
    }

    private void SyncFavoriteStates()
    {
        foreach (var item in Listings.Concat(HotPicks))
        {
            var isFav = favoritesService.IsFavorite(item.Listing.Id);
            if (item.IsFavorited != isFav)
            {
                item.IsFavorited = isFav;
            }
        }
    }

    private ListingCardItem ToCardItem(ListingSummary listing) =>
        new(listing, favoritesService.IsFavorite(listing.Id));

    private async Task FetchAndPopulateListingsAsync(CancellationToken ct)
    {
        _offset = 0;
        _hasMoreItems = true;

        var (items, total) = await FetchPageAsync(0, ct);

        Listings.Clear();
        HotPicks.Clear();

        foreach (var listing in items)
        {
            Listings.Add(ToCardItem(listing));
        }

        var hotPickCount = Math.Min(HotPicksCount, items.Count);
        for (var i = 0; i < hotPickCount; i++)
        {
            HotPicks.Add(ToCardItem(items[i]));
        }

        _offset = PageSize;
        _hasMoreItems = items.Count >= PageSize;
        TotalResults = total;
        IsEmpty = Listings.Count == 0;
        HasHotPicks = HotPicks.Count > 0;
    }

    private async Task<(List<ListingSummary> Items, int Total)> FetchPageAsync(int offset, CancellationToken ct)
    {
        if (SelectedCondition is not null)
        {
            var result = await listingsService.SearchAsync(
                condition: SelectedCondition,
                limit: PageSize,
                offset: offset,
                ct: ct);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(result.ErrorMessage);
            }

            return (result.Data!.Items.Select(ToListingSummary).ToList(), result.Data.Total);
        }

        var listingsResult = await listingsService.GetListingsAsync(PageSize, offset, ct);
        if (!listingsResult.IsSuccess)
        {
            throw new InvalidOperationException(listingsResult.ErrorMessage);
        }

        return (listingsResult.Data!.Listings, listingsResult.Data.Total);
    }

    private static ListingSummary ToListingSummary(SearchResultItem item) =>
        new(
            item.Id,
            item.Slug,
            item.Title,
            (decimal?)item.Price,
            item.Currency,
            item.City,
            int.TryParse(item.CategoryId, out var catId) ? catId : 0,
            null,
            item.PublishedAt);
}
