using SportowyHub.Models.Api;

namespace SportowyHub.Services.Listings;

public interface IListingsService
{
    Task<ListingsResponse> GetListingsAsync(int limit = 20, int offset = 0, CancellationToken ct = default);
    Task<ListingDetail> GetListingAsync(string id, CancellationToken ct = default);
    Task<SearchResponse> SearchAsync(
        string? query = null,
        int? categoryId = null,
        string? sport = null,
        string? city = null,
        float? priceMin = null,
        float? priceMax = null,
        string? sort = null,
        int limit = 30,
        int offset = 0,
        CancellationToken ct = default);
}
