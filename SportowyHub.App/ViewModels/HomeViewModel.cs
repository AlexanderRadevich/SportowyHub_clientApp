using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Models.Api;
using SportowyHub.Services.Auth;
using SportowyHub.Services.Favorites;
using SportowyHub.Services.Listings;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.Sections;
using SportowyHub.Services.Toast;

namespace SportowyHub.ViewModels;

public partial class HomeViewModel(
    IListingsService listingsService,
    INavigationService nav,
    IToastService toastService,
    IAuthService authService,
    ISectionsService sectionsService,
    IFavoritesService favoritesService) : ObservableObject
{
    private const int PageSize = 20;
    private const int HotPicksCount = 6;
    private int _offset;
    private bool _hasMoreItems = true;

    public ObservableCollection<ListingSummary> Listings { get; } = [];
    public ObservableCollection<ListingSummary> HotPicks { get; } = [];
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
    public partial string? SelectedCondition { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalResultsText))]
    public partial int TotalResults { get; set; }

    public string TotalResultsText =>
        string.Format(Resources.Strings.AppResources.HomeShowingResults, TotalResults);

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
            }
        }
        catch
        {
            // Silently fail — favorites cache is non-critical
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
    private async Task LoadSections()
    {
        try
        {
            var locale = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            var response = await sectionsService.GetSectionsAsync(locale);
            Sections.Clear();
            foreach (var section in response.Sports)
            {
                Sections.Add(section);
            }
            HasSections = Sections.Count > 0;
        }
        catch
        {
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
                Listings.Add(listing);
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
        var query = $"listing-detail?id={Uri.EscapeDataString(listing.Id)}" +
                    $"&title={Uri.EscapeDataString(listing.Title)}" +
                    $"&price={Uri.EscapeDataString(listing.Price?.ToString(CultureInfo.InvariantCulture) ?? string.Empty)}" +
                    $"&currency={Uri.EscapeDataString(listing.Currency ?? string.Empty)}" +
                    $"&city={Uri.EscapeDataString(listing.City ?? string.Empty)}";
        await nav.GoToAsync(query);
    }

    [RelayCommand]
    private async Task ToggleFavorite(ListingSummary listing)
    {
        var user = await authService.GetCurrentUserAsync();
        if (user is null)
        {
            await nav.NavigateToLoginWithReturnUrlAsync("//home");
            return;
        }

        var wasFavorited = favoritesService.IsFavorite(listing.Id);

        try
        {
            if (wasFavorited)
            {
                await favoritesService.RemoveAsync(listing.Id);
            }
            else
            {
                await favoritesService.AddAsync(listing.Id);
            }
            OnPropertyChanged(nameof(Listings));
            OnPropertyChanged(nameof(HotPicks));
        }
        catch (Exception ex)
        {
            await toastService.ShowError(ex.Message);
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
            SearchViewModel.PendingSportSection = section;
            shell.CurrentItem = shell.Items[0].Items[1];
        }
    }

    public bool IsListingFavorited(string listingId) => favoritesService.IsFavorite(listingId);

    private async Task FetchAndPopulateListingsAsync(CancellationToken ct)
    {
        _offset = 0;
        _hasMoreItems = true;

        var (items, total) = await FetchPageAsync(0, ct);

        Listings.Clear();
        HotPicks.Clear();

        foreach (var listing in items)
        {
            Listings.Add(listing);
        }

        var hotPickCount = Math.Min(HotPicksCount, items.Count);
        for (var i = 0; i < hotPickCount; i++)
        {
            HotPicks.Add(items[i]);
        }

        _offset = PageSize;
        _hasMoreItems = items.Count >= PageSize;
        TotalResults = total;
        IsEmpty = Listings.Count == 0;
    }

    private async Task<(List<ListingSummary> Items, int Total)> FetchPageAsync(int offset, CancellationToken ct)
    {
        if (SelectedCondition is not null)
        {
            var response = await listingsService.SearchAsync(
                condition: SelectedCondition,
                limit: PageSize,
                offset: offset,
                ct: ct);
            return (response.Items.Select(ToListingSummary).ToList(), response.Total);
        }

        var listings = await listingsService.GetListingsAsync(PageSize, offset, ct);
        return (listings.Listings, listings.Total);
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
