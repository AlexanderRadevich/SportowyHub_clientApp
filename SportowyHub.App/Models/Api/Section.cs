namespace SportowyHub.Models.Api;

public record Section(int Id, string Slug, string Name);

public record SectionsResponse(List<Section> Sports, string? Locale = null);
