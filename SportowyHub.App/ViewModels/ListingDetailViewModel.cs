using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Models.Api;
using SportowyHub.Services.Listings;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.Toast;

namespace SportowyHub.ViewModels;

public partial class ListingDetailViewModel(
    IListingsService listingsService,
    INavigationService nav,
    IToastService toastService) : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial bool HasError { get; set; }

    [ObservableProperty]
    public partial ListingDetail? Listing { get; set; }

    public string FormattedPrice =>
        Listing?.Price is { } price && Listing?.Currency is { } currency
            ? $"{price} {currency}"
            : string.Empty;

    public string FormattedLocation
    {
        get
        {
            var parts = new[] { Listing?.City, Listing?.Region }
                .Where(p => !string.IsNullOrWhiteSpace(p));
            return string.Join(", ", parts);
        }
    }

    private string _listingId = string.Empty;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var id) && id is string idStr)
        {
            _listingId = idStr;
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
            OnPropertyChanged(nameof(FormattedPrice));
            OnPropertyChanged(nameof(FormattedLocation));
            IsLoading = false;
        }
    }
}
