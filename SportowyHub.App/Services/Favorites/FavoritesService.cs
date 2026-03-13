using System.Collections.Concurrent;
using System.Net;
using Microsoft.Extensions.Logging;
using SportowyHub.Models;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;

namespace SportowyHub.Services.Favorites;

internal class FavoritesService(IRequestProvider requestProvider, ITokenProvider authService, ILogger<FavoritesService> logger) : IFavoritesService
{
    private readonly ConcurrentDictionary<string, byte> _favoriteIds = new();

    public async Task<Result<bool>> LoadFavoriteIdsAsync(CancellationToken ct = default)
    {
        try
        {
            var token = await authService.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                _favoriteIds.Clear();
                return Result<bool>.Success(true);
            }

            var response = await requestProvider.GetAsync<FavoritesIdsResponse>("/api/private/favorites/ids", token, ct);
            _favoriteIds.Clear();
            foreach (var id in response.Ids)
            {
                _favoriteIds.TryAdd(id, 0);
            }

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to load favorite IDs");
            return Result<bool>.Failure(ex.Message);
        }
    }

    public bool IsFavorite(string listingId)
    {
        return _favoriteIds.ContainsKey(listingId);
    }

    public async Task<Result<FavoritesListResponse>> GetFavoritesAsync(int page = 1, int perPage = 20, CancellationToken ct = default)
    {
        try
        {
            var token = await authService.GetTokenAsync();
            var response = await requestProvider.GetAsync<FavoritesListResponse>(
                $"/api/private/favorites?page={page}&per_page={perPage}", token ?? "", ct);
            return Result<FavoritesListResponse>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to get favorites");
            return Result<FavoritesListResponse>.Failure(ex.Message);
        }
    }

    public async Task<Result<bool>> AddAsync(string listingId, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(listingId);
        try
        {
            var token = await authService.GetTokenAsync();
            try
            {
                await requestProvider.PostAsync<Dictionary<string, string>, FavoriteActionResponse>(
                    $"/api/private/favorites/{Uri.EscapeDataString(listingId)}", new(), token ?? "", ct: ct);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
            {
                logger.LogWarning(ex, "Favorite already exists for listing {ListingId}", listingId);
            }

            _favoriteIds.TryAdd(listingId, 0);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to add favorite {ListingId}", listingId);
            return Result<bool>.Failure(ex.Message);
        }
    }

    public async Task<Result<bool>> RemoveAsync(string listingId, CancellationToken ct = default)
    {
        try
        {
            var token = await authService.GetTokenAsync();
            await requestProvider.DeleteAsync(
                $"/api/private/favorites/{Uri.EscapeDataString(listingId)}", token ?? "", ct);
            _favoriteIds.TryRemove(listingId, out _);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to remove favorite {ListingId}", listingId);
            return Result<bool>.Failure(ex.Message);
        }
    }

    public void ClearCache()
    {
        _favoriteIds.Clear();
    }
}
