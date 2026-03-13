using SportowyHub.Models;
using SportowyHub.Models.Api;

namespace SportowyHub.Services.Favorites;

public interface IFavoritesService
{
    Task<Result<bool>> LoadFavoriteIdsAsync(CancellationToken ct = default);
    bool IsFavorite(string listingId);
    Task<Result<FavoritesListResponse>> GetFavoritesAsync(int page = 1, int perPage = 20, CancellationToken ct = default);
    Task<Result<bool>> AddAsync(string listingId, CancellationToken ct = default);
    Task<Result<bool>> RemoveAsync(string listingId, CancellationToken ct = default);
    void ClearCache();
}
