using SportowyHub.Models.Api;

namespace SportowyHub.Services.Sections;

public interface ISectionsService
{
    Task<SectionsResponse> GetSectionsAsync(string? locale = null, CancellationToken ct = default);
    Task<CategoriesResponse> GetCategoriesAsync(int sectionId, string? locale = null, CancellationToken ct = default);
}
