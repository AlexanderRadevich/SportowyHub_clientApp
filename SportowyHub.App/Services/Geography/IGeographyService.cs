using SportowyHub.Models;
using SportowyHub.Models.Api;

namespace SportowyHub.Services.Geography;

public interface IGeographyService
{
    Task<Result<List<GeographyAutocompleteItem>>> AutocompleteAsync(string query, string? locale = null, int? limit = null, CancellationToken ct = default);
    Task<Result<List<VoivodeshipItem>>> GetVoivodeshipsAsync(string? locale = null, CancellationToken ct = default);
    Task<Result<List<CityItem>>> GetCitiesAsync(int voivodeshipId, string? locale = null, CancellationToken ct = default);
}
