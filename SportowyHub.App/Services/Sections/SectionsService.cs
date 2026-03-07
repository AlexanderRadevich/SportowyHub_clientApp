using SportowyHub.Models.Api;
using SportowyHub.Services.Api;

namespace SportowyHub.Services.Sections;

internal class SectionsService(IRequestProvider requestProvider) : ISectionsService
{
    public async Task<SectionsResponse> GetSectionsAsync(string? locale = null, CancellationToken ct = default)
    {
        var uri = string.IsNullOrWhiteSpace(locale)
            ? "/api/v1/sports"
            : $"/api/v1/sports?locale={Uri.EscapeDataString(locale)}";
        return await requestProvider.GetAsync<SectionsResponse>(uri, ct: ct);
    }

    public async Task<CategoriesResponse> GetCategoriesAsync(int sectionId, string? locale = null, CancellationToken ct = default)
    {
        var uri = string.IsNullOrWhiteSpace(locale)
            ? $"/api/v1/sports/{sectionId}/categories"
            : $"/api/v1/sports/{sectionId}/categories?locale={Uri.EscapeDataString(locale)}";
        return await requestProvider.GetAsync<CategoriesResponse>(uri, ct: ct);
    }
}
