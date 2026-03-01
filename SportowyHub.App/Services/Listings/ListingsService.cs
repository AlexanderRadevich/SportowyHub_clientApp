using System.Globalization;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;

namespace SportowyHub.Services.Listings;

public class ListingsService(IRequestProvider requestProvider) : IListingsService
{
    public async Task<ListingsResponse> GetListingsAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
    {
        var uri = $"/api/v1/listings?limit={limit}&offset={offset}";
        return await requestProvider.GetAsync<ListingsResponse>(uri, ct: ct);
    }

    public async Task<ListingDetail> GetListingAsync(string id, CancellationToken ct = default)
    {
        var uri = $"/api/v1/listings/{Uri.EscapeDataString(id)}";
        return await requestProvider.GetAsync<ListingDetail>(uri, ct: ct);
    }

    public async Task<SearchResponse> SearchAsync(
        string? query = null,
        int? categoryId = null,
        string? sport = null,
        string? city = null,
        float? priceMin = null,
        float? priceMax = null,
        string? sort = null,
        int limit = 30,
        int offset = 0,
        CancellationToken ct = default)
    {
        var parameters = new List<string>
        {
            $"limit={limit}",
            $"offset={offset}"
        };

        if (!string.IsNullOrWhiteSpace(query))
        {
            parameters.Add($"q={Uri.EscapeDataString(query)}");
        }

        if (categoryId.HasValue)
        {
            parameters.Add($"category_id={categoryId.Value}");
        }

        if (!string.IsNullOrWhiteSpace(sport))
        {
            parameters.Add($"sport={Uri.EscapeDataString(sport)}");
        }

        if (!string.IsNullOrWhiteSpace(city))
        {
            parameters.Add($"city={Uri.EscapeDataString(city)}");
        }

        if (priceMin.HasValue)
        {
            parameters.Add($"price_min={priceMin.Value.ToString(CultureInfo.InvariantCulture)}");
        }

        if (priceMax.HasValue)
        {
            parameters.Add($"price_max={priceMax.Value.ToString(CultureInfo.InvariantCulture)}");
        }

        if (!string.IsNullOrWhiteSpace(sort))
        {
            parameters.Add($"sort={Uri.EscapeDataString(sort)}");
        }

        var uri = $"/api/v1/search?{string.Join("&", parameters)}";
        return await requestProvider.GetAsync<SearchResponse>(uri, ct: ct);
    }
}
