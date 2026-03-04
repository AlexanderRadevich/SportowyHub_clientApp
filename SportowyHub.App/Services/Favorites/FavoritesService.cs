using System.Collections.Concurrent;
using System.Net;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;

namespace SportowyHub.Services.Favorites;

internal class FavoritesService(IRequestProvider requestProvider, IAuthService authService) : IFavoritesService
{
    private ConcurrentDictionary<string, byte> _favoriteIds = new();

    public async Task LoadFavoriteIdsAsync(CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            _favoriteIds = new();
            return;
        }

        var response = await requestProvider.GetAsync<FavoritesIdsResponse>("/api/private/favorites/ids", token, ct);
        _favoriteIds = new(response.Ids.Select(id => new KeyValuePair<string, byte>(id, 0)));
    }

    public bool IsFavorite(string listingId)
    {
        return _favoriteIds.ContainsKey(listingId);
    }

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

        _favoriteIds.TryAdd(listingId, 0);
    }

    public async Task RemoveAsync(string listingId, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync();
        await requestProvider.DeleteAsync(
            $"/api/private/favorites/{Uri.EscapeDataString(listingId)}", token ?? "", ct);
        _favoriteIds.TryRemove(listingId, out _);
    }

    public void ClearCache()
    {
        _favoriteIds = new();
    }
}
