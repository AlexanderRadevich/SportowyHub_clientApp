using SportowyHub.Models.Api;

namespace SportowyHub.Services.Favorites;

public interface IFavoritesService
{
    Task LoadFavoriteIdsAsync(CancellationToken ct = default);
    bool IsFavorite(string listingId);
    Task<FavoritesListResponse> GetFavoritesAsync(int page = 1, int perPage = 20, CancellationToken ct = default);
    Task AddAsync(string listingId, CancellationToken ct = default);
    Task RemoveAsync(string listingId, CancellationToken ct = default);
    void ClearCache();
}
