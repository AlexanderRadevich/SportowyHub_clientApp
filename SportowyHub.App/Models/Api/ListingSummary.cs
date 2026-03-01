namespace SportowyHub.Models.Api;

public record ListingSummary(
    string Id,
    string? Slug,
    string Title,
    string? Price,
    string? Currency,
    string? City,
    int CategoryId,
    string? ContentLocale,
    string? PublishedAt);
