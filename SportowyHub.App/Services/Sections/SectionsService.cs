using Microsoft.Extensions.Logging;
using SportowyHub.Models;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;

namespace SportowyHub.Services.Sections;

internal class SectionsService(IRequestProvider requestProvider, ILogger<SectionsService> logger) : ISectionsService
{
    public async Task<Result<SectionsResponse>> GetSectionsAsync(string? locale = null, CancellationToken ct = default)
    {
        try
        {
            var uri = string.IsNullOrWhiteSpace(locale)
                ? "/api/v1/sports"
                : $"/api/v1/sports?locale={Uri.EscapeDataString(locale)}";
            var response = await requestProvider.GetAsync<SectionsResponse>(uri, ct: ct);
            return Result<SectionsResponse>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to get sections");
            return Result<SectionsResponse>.Failure(ex.Message);
        }
    }

    public async Task<Result<CategoriesResponse>> GetCategoriesAsync(int sectionId, string? locale = null, CancellationToken ct = default)
    {
        try
        {
            var uri = string.IsNullOrWhiteSpace(locale)
                ? $"/api/v1/sports/{sectionId}/categories"
                : $"/api/v1/sports/{sectionId}/categories?locale={Uri.EscapeDataString(locale)}";
            var response = await requestProvider.GetAsync<CategoriesResponse>(uri, ct: ct);
            return Result<CategoriesResponse>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to get categories for section {SectionId}", sectionId);
            return Result<CategoriesResponse>.Failure(ex.Message);
        }
    }
}
