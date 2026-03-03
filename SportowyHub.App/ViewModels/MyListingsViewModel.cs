using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Models.Api;
using SportowyHub.Resources.Strings;
using SportowyHub.Services.ListingManagement;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.Toast;

namespace SportowyHub.ViewModels;

public partial class MyListingsViewModel(
    IListingManagementService listingManagementService,
    INavigationService nav,
    IToastService toastService) : ObservableObject
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

        try
        {
            var response = await listingManagementService.GetMyListingsAsync(SelectedFilter, ct);
            Listings.Clear();
            foreach (var item in response.Listings)
            {
                Listings.Add(item);
            }

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
    private async Task RefreshListings(CancellationToken ct)
    {
        try
        {
            var response = await listingManagementService.GetMyListingsAsync(SelectedFilter, ct);
            Listings.Clear();
            foreach (var item in response.Listings)
            {
                Listings.Add(item);
            }

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
    private async Task FilterByStatus(string? status, CancellationToken ct)
    {
        SelectedFilter = status;
        await LoadListings(ct);
    }

    [RelayCommand]
    private async Task PublishListing(MyListingSummary item, CancellationToken ct)
    {
        try
        {
            await listingManagementService.UpdateStatusAsync(item.Id, "published", ct);
            await LoadListings(ct);
        }
        catch (Exception ex)
        {
            await toastService.ShowError(ex.Message);
        }
    }

    [RelayCommand]
    private async Task HideListing(MyListingSummary item, CancellationToken ct)
    {
        try
        {
            await listingManagementService.UpdateStatusAsync(item.Id, "hidden", ct);
            await LoadListings(ct);
        }
        catch (Exception ex)
        {
            await toastService.ShowError(ex.Message);
        }
    }

    [RelayCommand]
    private async Task ResubmitListing(MyListingSummary item, CancellationToken ct)
    {
        try
        {
            await listingManagementService.ResubmitForReviewAsync(item.Id, ct);
            await LoadListings(ct);
        }
        catch (Exception ex)
        {
            await toastService.ShowError(ex.Message);
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

        try
        {
            await listingManagementService.DeleteListingAsync(item.Id, ct);
            Listings.Remove(item);
            IsEmpty = Listings.Count == 0;
        }
        catch (Exception ex)
        {
            await toastService.ShowError(ex.Message);
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
        await nav.GoToAsync("create-edit-listing");
    }
}
