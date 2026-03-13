using Microsoft.Extensions.Logging;
using SportowyHub.Models;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;

namespace SportowyHub.Services.Geography;

internal class GeographyService(IRequestProvider requestProvider, ILogger<GeographyService> logger) : IGeographyService
{
    public async Task<Result<List<GeographyAutocompleteItem>>> AutocompleteAsync(string query, string? locale = null, int? limit = null, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return Result<List<GeographyAutocompleteItem>>.Success([]);
        }

        try
        {
            var uri = $"/api/v1/geography/autocomplete?q={Uri.EscapeDataString(query)}";

            if (!string.IsNullOrWhiteSpace(locale))
            {
                uri += $"&locale={Uri.EscapeDataString(locale)}";
            }

            if (limit.HasValue)
            {
                uri += $"&limit={limit.Value}";
            }

            var response = await requestProvider.GetAsync<List<GeographyAutocompleteItem>>(uri, ct: ct);
            return Result<List<GeographyAutocompleteItem>>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to autocomplete geography for query {Query}", query);
            return Result<List<GeographyAutocompleteItem>>.Failure(ex.Message);
        }
    }

    public async Task<Result<List<VoivodeshipItem>>> GetVoivodeshipsAsync(string? locale = null, CancellationToken ct = default)
    {
        try
        {
            var uri = string.IsNullOrWhiteSpace(locale)
                ? "/api/v1/geography/voivodeships"
                : $"/api/v1/geography/voivodeships?locale={Uri.EscapeDataString(locale)}";

            var response = await requestProvider.GetAsync<List<VoivodeshipItem>>(uri, ct: ct);
            return Result<List<VoivodeshipItem>>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to get voivodeships");
            return Result<List<VoivodeshipItem>>.Failure(ex.Message);
        }
    }

    public async Task<Result<List<CityItem>>> GetCitiesAsync(int voivodeshipId, string? locale = null, CancellationToken ct = default)
    {
        try
        {
            var uri = $"/api/v1/geography/cities?voivodeship_id={voivodeshipId}";

            if (!string.IsNullOrWhiteSpace(locale))
            {
                uri += $"&locale={Uri.EscapeDataString(locale)}";
            }

            var response = await requestProvider.GetAsync<List<CityItem>>(uri, ct: ct);
            return Result<List<CityItem>>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to get cities for voivodeship {VoivodeshipId}", voivodeshipId);
            return Result<List<CityItem>>.Failure(ex.Message);
        }
    }
}
