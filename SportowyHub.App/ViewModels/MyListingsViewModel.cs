using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Models.Api;
using SportowyHub.Resources.Strings;
using SportowyHub.Services.Auth;
using SportowyHub.Services.ListingManagement;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.Toast;

namespace SportowyHub.ViewModels;

public partial class MyListingsViewModel(
    IListingManagementService listingManagementService,
    INavigationService nav,
    IToastService toastService,
    ITokenProvider authService) : ObservableObject
{
    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial bool IsRefreshing { get; set; }

    [ObservableProperty]
    public partial bool IsEmpty { get; set; }

    [ObservableProperty]
    public partial string? SelectedFilter { get; set; }

    public ObservableCollection<MyListingSummary> Listings { get; } = [];

    [RelayCommand]
    private async Task Appearing(CancellationToken ct)
    {
        if (Listings.Count == 0)
        {
            await LoadListings(ct);
        }
    }

    [RelayCommand]
    private async Task LoadListings(CancellationToken ct)
    {
        IsLoading = true;
        IsEmpty = false;

        var result = await listingManagementService.GetMyListingsAsync(SelectedFilter, ct);
        if (result.IsSuccess)
        {
            Listings.Clear();
            foreach (var item in result.Data!.Listings)
            {
                Listings.Add(item);
            }

            IsEmpty = Listings.Count == 0;
        }
        else
        {
            await toastService.ShowError(result.ErrorMessage!);
        }

        IsLoading = false;
    }

    [RelayCommand]
    private async Task RefreshListings(CancellationToken ct)
    {
        var result = await listingManagementService.GetMyListingsAsync(SelectedFilter, ct);
        if (result.IsSuccess)
        {
            Listings.Clear();
            foreach (var item in result.Data!.Listings)
            {
                Listings.Add(item);
            }

            IsEmpty = Listings.Count == 0;
        }
        else
        {
            await toastService.ShowError(result.ErrorMessage!);
        }

        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task FilterByStatus(string? status, CancellationToken ct)
    {
        SelectedFilter = status;
        await LoadListings(ct);
    }

    [RelayCommand]
    private async Task PublishListing(MyListingSummary item, CancellationToken ct)
    {
        var result = await listingManagementService.UpdateStatusAsync(item.Id, "published", ct);
        if (result.IsSuccess)
        {
            await LoadListings(ct);
        }
        else
        {
            await toastService.ShowError(result.ErrorMessage!);
        }
    }

    [RelayCommand]
    private async Task HideListing(MyListingSummary item, CancellationToken ct)
    {
        var result = await listingManagementService.UpdateStatusAsync(item.Id, "hidden", ct);
        if (result.IsSuccess)
        {
            await LoadListings(ct);
        }
        else
        {
            await toastService.ShowError(result.ErrorMessage!);
        }
    }

    [RelayCommand]
    private async Task ResubmitListing(MyListingSummary item, CancellationToken ct)
    {
        var result = await listingManagementService.ResubmitForReviewAsync(item.Id, ct);
        if (result.IsSuccess)
        {
            await LoadListings(ct);
        }
        else
        {
            await toastService.ShowError(result.ErrorMessage!);
        }
    }

    [RelayCommand]
    private async Task DeleteListing(MyListingSummary item, CancellationToken ct)
    {
        var confirmed = await nav.DisplayAlertAsync(
            AppResources.MyListingsDeleteConfirmTitle,
            AppResources.MyListingsDeleteConfirmMessage,
            AppResources.MyListingsDelete,
            AppResources.Cancel);

        if (!confirmed)
        {
            return;
        }

        var result = await listingManagementService.DeleteListingAsync(item.Id, ct);
        if (result.IsSuccess)
        {
            Listings.Remove(item);
            IsEmpty = Listings.Count == 0;
        }
        else
        {
            await toastService.ShowError(result.ErrorMessage!);
        }
    }

    [RelayCommand]
    private async Task GoToEditListing(MyListingSummary item)
    {
        await nav.GoToAsync($"create-edit-listing?id={Uri.EscapeDataString(item.Id)}");
    }

    [RelayCommand]
    private async Task GoToCreateListing()
    {
        var user = await authService.GetCurrentUserAsync();
        if (user is null || user.TrustLevel == Models.TrustLevels.Unverified)
        {
            await toastService.ShowError(Resources.Strings.AppResources.CreateListingPhoneRequired);
            return;
        }

        await nav.GoToAsync("create-edit-listing");
    }
}
