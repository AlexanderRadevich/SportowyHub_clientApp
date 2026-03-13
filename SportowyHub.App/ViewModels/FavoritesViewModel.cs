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
    ITokenProvider authService,
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

        await favoritesService.LoadFavoriteIdsAsync(ct);
        var result = await favoritesService.GetFavoritesAsync(1, PageSize, ct);

        if (result.IsSuccess)
        {
            Favorites.Clear();
            foreach (var item in result.Data!.Items)
            {
                Favorites.Add(item);
            }

            _currentPage = 1;
            _totalPages = result.Data.Pages;
            IsEmpty = Favorites.Count == 0;
        }
        else
        {
            await toastService.ShowError(result.ErrorMessage!);
        }

        IsLoading = false;
    }

    [RelayCommand]
    private async Task RefreshFavorites(CancellationToken ct)
    {
        await favoritesService.LoadFavoriteIdsAsync(ct);
        var result = await favoritesService.GetFavoritesAsync(1, PageSize, ct);

        if (result.IsSuccess)
        {
            Favorites.Clear();
            foreach (var item in result.Data!.Items)
            {
                Favorites.Add(item);
            }

            _currentPage = 1;
            _totalPages = result.Data.Pages;
            IsEmpty = Favorites.Count == 0;
        }
        else
        {
            await toastService.ShowError(result.ErrorMessage!);
        }

        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task LoadMoreFavorites(CancellationToken ct)
    {
        if (IsLoading || _currentPage >= _totalPages)
        {
            return;
        }

        var result = await favoritesService.GetFavoritesAsync(_currentPage + 1, PageSize, ct);
        if (result.IsSuccess)
        {
            foreach (var item in result.Data!.Items)
            {
                Favorites.Add(item);
            }

            _currentPage = result.Data.Page;
            _totalPages = result.Data.Pages;
        }
        else
        {
            await toastService.ShowError(result.ErrorMessage!);
        }
    }

    [RelayCommand]
    private async Task RemoveFavorite(FavoriteItem item, CancellationToken ct)
    {
        Favorites.Remove(item);
        IsEmpty = Favorites.Count == 0;

        var result = await favoritesService.RemoveAsync(item.Id, ct);
        if (!result.IsSuccess)
        {
            Favorites.Add(item);
            IsEmpty = false;
            await toastService.ShowError(result.ErrorMessage!);
        }
    }

    [RelayCommand]
    private async Task GoToListingDetail(FavoriteItem item)
    {
        await nav.GoToListingDetailAsync(item.Id, item.Title, item.Price, item.Currency, item.City);
    }

    [RelayCommand]
    private async Task SignIn()
    {
        await nav.NavigateToLoginWithReturnUrlAsync("//favorites");
    }
}
