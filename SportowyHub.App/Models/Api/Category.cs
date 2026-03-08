namespace SportowyHub.Models.Api;

public record Category(int Id, string Slug, string Name, int SportId);

public record CategoriesResponse(List<Category> Categories, string? Locale = null);
