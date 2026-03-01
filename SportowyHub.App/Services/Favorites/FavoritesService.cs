using System.Net;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;

namespace SportowyHub.Services.Favorites;

internal class FavoritesService(IRequestProvider requestProvider, IAuthService authService) : IFavoritesService
{
    private readonly HashSet<string> _favoriteIds = [];

    public async Task LoadFavoriteIdsAsync(CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            _favoriteIds.Clear();
            return;
        }

        var response = await requestProvider.GetAsync<FavoritesIdsResponse>("/api/private/favorites/ids", token, ct);
        _favoriteIds.Clear();
        foreach (var id in response.Ids)
        {
            _favoriteIds.Add(id);
        }
    }

    public bool IsFavorite(string listingId) => _favoriteIds.Contains(listingId);

    public async Task<FavoritesListResponse> GetFavoritesAsync(int page = 1, int perPage = 20, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync();
        return await requestProvider.GetAsync<FavoritesListResponse>(
            $"/api/private/favorites?page={page}&per_page={perPage}", token ?? "", ct);
    }

    public async Task AddAsync(string listingId, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync();
        try
        {
            await requestProvider.PostAsync<Dictionary<string, string>, FavoriteActionResponse>(
                $"/api/private/favorites/{Uri.EscapeDataString(listingId)}", new(), token ?? "", ct: ct);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
        {
        }

        _favoriteIds.Add(listingId);
    }

    public async Task RemoveAsync(string listingId, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync();
        await requestProvider.DeleteAsync(
            $"/api/private/favorites/{Uri.EscapeDataString(listingId)}", token ?? "", ct);
        _favoriteIds.Remove(listingId);
    }

    public void ClearCache() => _favoriteIds.Clear();
}
