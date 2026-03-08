using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Models.Api;
using SportowyHub.Services.Auth;
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
    ISectionsService sectionsService) : ObservableObject
{
    private const int PageSize = 20;
    private int _offset;
    private bool _hasMoreItems = true;

    public ObservableCollection<ListingSummary> Listings { get; } = [];
    public ObservableCollection<Section> Sections { get; } = [];

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial bool IsRefreshing { get; set; }

    [ObservableProperty]
    public partial bool IsEmpty { get; set; }

    [ObservableProperty]
    public partial bool HasSections { get; set; }

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
            _offset = 0;
            _hasMoreItems = true;
            Listings.Clear();

            var response = await listingsService.GetListingsAsync(PageSize, 0, ct);

            foreach (var listing in response.Listings)
            {
                Listings.Add(listing);
            }

            _offset = PageSize;
            _hasMoreItems = response.Listings.Count >= PageSize;
            IsEmpty = Listings.Count == 0;
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
            _offset = 0;
            _hasMoreItems = true;
            Listings.Clear();

            var response = await listingsService.GetListingsAsync(PageSize, 0, ct);

            foreach (var listing in response.Listings)
            {
                Listings.Add(listing);
            }

            _offset = PageSize;
            _hasMoreItems = response.Listings.Count >= PageSize;
            IsEmpty = Listings.Count == 0;
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
            var response = await listingsService.GetListingsAsync(PageSize, _offset, ct);

            foreach (var listing in response.Listings)
            {
                Listings.Add(listing);
            }

            _offset += PageSize;
            _hasMoreItems = response.Listings.Count >= PageSize;
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
    private void GoToSearch()
    {
        if (Shell.Current is Shell shell)
        {
            shell.CurrentItem = shell.Items[0].Items[1];
        }
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
}
