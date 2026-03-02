using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Models.Api;
using SportowyHub.Services.Auth;
using SportowyHub.Services.Favorites;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.Toast;

namespace SportowyHub.ViewModels;

public partial class FavoritesViewModel(
    IFavoritesService favoritesService,
    IAuthService authService,
    INavigationService nav,
    IToastService toastService) : ObservableObject
{
    private const int PageSize = 20;
    private int _currentPage;
    private int _totalPages;

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial bool IsRefreshing { get; set; }

    [ObservableProperty]
    public partial bool IsLoggedIn { get; set; }

    [ObservableProperty]
    public partial bool IsEmpty { get; set; }

    public ObservableCollection<FavoriteItem> Favorites { get; } = [];

    [RelayCommand]
    private async Task Appearing(CancellationToken ct)
    {
        IsLoggedIn = await authService.IsLoggedInAsync();
        if (!IsLoggedIn)
        {
            Favorites.Clear();
            IsEmpty = false;
            return;
        }

        if (Favorites.Count == 0)
        {
            await LoadFavorites(ct);
        }
    }

    [RelayCommand]
    private async Task LoadFavorites(CancellationToken ct)
    {
        IsLoading = true;
        IsEmpty = false;

        try
        {
            await favoritesService.LoadFavoriteIdsAsync(ct);
            var response = await favoritesService.GetFavoritesAsync(1, PageSize, ct);

            Favorites.Clear();
            foreach (var item in response.Items)
            {
                Favorites.Add(item);
            }

            _currentPage = 1;
            _totalPages = response.Pages;
            IsEmpty = Favorites.Count == 0;
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
    private async Task RefreshFavorites(CancellationToken ct)
    {
        try
        {
            await favoritesService.LoadFavoriteIdsAsync(ct);
            var response = await favoritesService.GetFavoritesAsync(1, PageSize, ct);

            Favorites.Clear();
            foreach (var item in response.Items)
            {
                Favorites.Add(item);
            }

            _currentPage = 1;
            _totalPages = response.Pages;
            IsEmpty = Favorites.Count == 0;
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
    private async Task LoadMoreFavorites(CancellationToken ct)
    {
        if (IsLoading || _currentPage >= _totalPages)
        {
            return;
        }

        try
        {
            var response = await favoritesService.GetFavoritesAsync(_currentPage + 1, PageSize, ct);
            foreach (var item in response.Items)
            {
                Favorites.Add(item);
            }

            _currentPage = response.Page;
            _totalPages = response.Pages;
        }
        catch (Exception ex)
        {
            await toastService.ShowError(ex.Message);
        }
    }

    [RelayCommand]
    private async Task RemoveFavorite(FavoriteItem item, CancellationToken ct)
    {
        Favorites.Remove(item);
        IsEmpty = Favorites.Count == 0;

        try
        {
            await favoritesService.RemoveAsync(item.Id, ct);
        }
        catch (Exception ex)
        {
            Favorites.Add(item);
            IsEmpty = false;
            await toastService.ShowError(ex.Message);
        }
    }

    [RelayCommand]
    private async Task GoToListingDetail(FavoriteItem item)
    {
        var query = $"listing-detail?id={Uri.EscapeDataString(item.Id)}" +
                    $"&title={Uri.EscapeDataString(item.Title)}" +
                    $"&price={Uri.EscapeDataString(item.Price ?? string.Empty)}" +
                    $"&currency={Uri.EscapeDataString(item.Currency ?? string.Empty)}" +
                    $"&city={Uri.EscapeDataString(item.City ?? string.Empty)}";
        await nav.GoToAsync(query);
    }

    [RelayCommand]
    private async Task SignIn()
    {
        await nav.GoToAsync("login");
    }
}
