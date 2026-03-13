using SportowyHub.Models;
using SportowyHub.Models.Api;

namespace SportowyHub.Services.Sections;

public interface ISectionsService
{
    Task<Result<SectionsResponse>> GetSectionsAsync(string? locale = null, CancellationToken ct = default);
    Task<Result<CategoriesResponse>> GetCategoriesAsync(int sectionId, string? locale = null, CancellationToken ct = default);
}
