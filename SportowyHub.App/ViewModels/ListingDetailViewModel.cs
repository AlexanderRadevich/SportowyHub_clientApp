using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Models.Api;
using SportowyHub.Services.Auth;
using SportowyHub.Services.Favorites;
using SportowyHub.Services.Listings;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.Toast;

namespace SportowyHub.ViewModels;

public partial class ListingDetailViewModel(
    IListingsService listingsService,
    INavigationService nav,
    IToastService toastService,
    IFavoritesService favoritesService,
    IAuthService authService) : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial bool HasError { get; set; }

    [ObservableProperty]
    public partial ListingDetail? Listing { get; set; }

    [ObservableProperty]
    public partial bool IsFavorited { get; set; }

    [ObservableProperty]
    public partial bool IsTogglingFavorite { get; set; }

    [ObservableProperty]
    public partial string PreviewTitle { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string PreviewPrice { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string PreviewCurrency { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string PreviewCity { get; set; } = string.Empty;

    public string DisplayTitle => Listing?.Title ?? PreviewTitle;

    public string FormattedPrice
    {
        get
        {
            if (Listing?.Price is { } price && Listing?.Currency is { } currency)
            {
                return $"{price} {currency}";
            }

            if (!string.IsNullOrEmpty(PreviewPrice) && !string.IsNullOrEmpty(PreviewCurrency))
            {
                return $"{PreviewPrice} {PreviewCurrency}";
            }

            return string.Empty;
        }
    }

    public string FormattedLocation
    {
        get
        {
            if (Listing is not null)
            {
                var parts = new[] { Listing.City, Listing.Region }
                    .Where(p => !string.IsNullOrWhiteSpace(p));
                return string.Join(", ", parts);
            }

            return !string.IsNullOrWhiteSpace(PreviewCity) ? PreviewCity : string.Empty;
        }
    }

    private string _listingId = string.Empty;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var id) && id is string idStr)
        {
            _listingId = idStr;
        }

        if (query.TryGetValue("title", out var title) && title is string titleStr)
        {
            PreviewTitle = titleStr;
        }

        if (query.TryGetValue("price", out var price) && price is string priceStr)
        {
            PreviewPrice = priceStr;
        }

        if (query.TryGetValue("currency", out var currency) && currency is string currencyStr)
        {
            PreviewCurrency = currencyStr;
        }

        if (query.TryGetValue("city", out var city) && city is string cityStr)
        {
            PreviewCity = cityStr;
        }

        OnPropertyChanged(nameof(DisplayTitle));
        OnPropertyChanged(nameof(FormattedPrice));
        OnPropertyChanged(nameof(FormattedLocation));

        if (!string.IsNullOrEmpty(_listingId))
        {
            LoadListingCommand.Execute(null);
        }
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await nav.GoBackAsync();
    }

    [RelayCommand]
    private async Task LoadListing(CancellationToken ct)
    {
        IsLoading = true;
        HasError = false;

        try
        {
            Listing = await listingsService.GetListingAsync(_listingId, ct);
        }
        catch (Exception ex)
        {
            HasError = true;
            await toastService.ShowError(ex.Message);
        }
        finally
        {
            OnPropertyChanged(nameof(DisplayTitle));
            OnPropertyChanged(nameof(FormattedPrice));
            OnPropertyChanged(nameof(FormattedLocation));
            IsLoading = false;
        }

        if (Listing is not null)
        {
            await favoritesService.LoadFavoriteIdsAsync(ct);
            IsFavorited = favoritesService.IsFavorite(_listingId);
        }
    }

    [RelayCommand]
    private async Task ToggleFavorite(CancellationToken ct)
    {
        if (!await authService.IsLoggedInAsync())
        {
            await nav.GoToAsync("login");
            return;
        }

        IsTogglingFavorite = true;
        var wasFavorited = IsFavorited;
        IsFavorited = !wasFavorited;

        try
        {
            if (wasFavorited)
            {
                await favoritesService.RemoveAsync(_listingId, ct);
            }
            else
            {
                await favoritesService.AddAsync(_listingId, ct);
            }
        }
        catch (Exception ex)
        {
            IsFavorited = wasFavorited;
            await toastService.ShowError(ex.Message);
        }
        finally
        {
            IsTogglingFavorite = false;
        }
    }
}
