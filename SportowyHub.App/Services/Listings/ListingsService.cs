using System.Globalization;
using Microsoft.Extensions.Logging;
using SportowyHub.Models;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;

namespace SportowyHub.Services.Listings;

public class ListingsService(IRequestProvider requestProvider, ILogger<ListingsService> logger) : IListingsService
{
    public async Task<Result<ListingsResponse>> GetListingsAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
    {
        try
        {
            var uri = $"/api/v1/listings?limit={limit}&offset={offset}";
            var response = await requestProvider.GetAsync<ListingsResponse>(uri, ct: ct);
            return Result<ListingsResponse>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to get listings");
            return Result<ListingsResponse>.Failure(ex.Message);
        }
    }

    public async Task<Result<ListingDetail>> GetListingAsync(string id, CancellationToken ct = default)
    {
        try
        {
            var uri = $"/api/v1/listings/{Uri.EscapeDataString(id)}";
            var response = await requestProvider.GetAsync<ListingDetail>(uri, ct: ct);
            return Result<ListingDetail>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to get listing {ListingId}", id);
            return Result<ListingDetail>.Failure(ex.Message);
        }
    }

    public async Task<Result<SearchResponse>> SearchAsync(
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
        CancellationToken ct = default)
    {
        try
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

            if (cityId.HasValue)
            {
                parameters.Add($"city_id={cityId.Value}");
            }

            if (voivodeshipId.HasValue)
            {
                parameters.Add($"voivodeship_id={voivodeshipId.Value}");
            }

            if (priceMin.HasValue)
            {
                parameters.Add($"price_min={priceMin.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (priceMax.HasValue)
            {
                parameters.Add($"price_max={priceMax.Value.ToString(CultureInfo.InvariantCulture)}");
            }

            if (!string.IsNullOrWhiteSpace(condition))
            {
                parameters.Add($"condition={Uri.EscapeDataString(condition)}");
            }

            if (!string.IsNullOrWhiteSpace(sort))
            {
                parameters.Add($"sort={Uri.EscapeDataString(sort)}");
            }

            var uri = $"/api/v1/search?{string.Join("&", parameters)}";
            var response = await requestProvider.GetAsync<SearchResponse>(uri, ct: ct);
            return Result<SearchResponse>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Search failed for query {Query}", query);
            return Result<SearchResponse>.Failure(ex.Message);
        }
    }
}
