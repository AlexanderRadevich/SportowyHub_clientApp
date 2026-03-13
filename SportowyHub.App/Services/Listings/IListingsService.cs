using SportowyHub.Models;
using SportowyHub.Models.Api;

namespace SportowyHub.Services.Listings;

public interface IListingsService
{
    Task<Result<ListingsResponse>> GetListingsAsync(int limit = 20, int offset = 0, CancellationToken ct = default);
    Task<Result<ListingDetail>> GetListingAsync(string id, CancellationToken ct = default);
    Task<Result<SearchResponse>> SearchAsync(
        string? query = null,
        int? categoryId = null,
        string? sport = null,
        int? cityId = null,
        int? voivodeshipId = null,
        float? priceMin = null,
        float? priceMax = null,
        string? condition = null,
        string? sort = null,
        int limit = 30,
        int offset = 0,
        CancellationToken ct = default);
}
