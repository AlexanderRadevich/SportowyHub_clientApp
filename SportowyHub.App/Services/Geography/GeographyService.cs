using SportowyHub.Models.Api;
using SportowyHub.Services.Api;

namespace SportowyHub.Services.Geography;

internal class GeographyService(IRequestProvider requestProvider) : IGeographyService
{
    public async Task<List<GeographyAutocompleteItem>> AutocompleteAsync(string query, string? locale = null, int? limit = null, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return [];
        }

        var uri = $"/api/v1/geography/autocomplete?q={Uri.EscapeDataString(query)}";

        if (!string.IsNullOrWhiteSpace(locale))
        {
            uri += $"&locale={Uri.EscapeDataString(locale)}";
        }

        if (limit.HasValue)
        {
            uri += $"&limit={limit.Value}";
        }

        return await requestProvider.GetAsync<List<GeographyAutocompleteItem>>(uri, ct: ct);
    }

    public async Task<List<VoivodeshipItem>> GetVoivodeshipsAsync(string? locale = null, CancellationToken ct = default)
    {
        var uri = string.IsNullOrWhiteSpace(locale)
            ? "/api/v1/geography/voivodeships"
            : $"/api/v1/geography/voivodeships?locale={Uri.EscapeDataString(locale)}";

        return await requestProvider.GetAsync<List<VoivodeshipItem>>(uri, ct: ct);
    }

    public async Task<List<CityItem>> GetCitiesAsync(int voivodeshipId, string? locale = null, CancellationToken ct = default)
    {
        var uri = $"/api/v1/geography/cities?voivodeship_id={voivodeshipId}";

        if (!string.IsNullOrWhiteSpace(locale))
        {
            uri += $"&locale={Uri.EscapeDataString(locale)}";
        }

        return await requestProvider.GetAsync<List<CityItem>>(uri, ct: ct);
    }
}
