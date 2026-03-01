namespace SportowyHub.Models.Api;

public record ListingDetail(
    string Id,
    string? Slug,
    string Title,
    string? Description,
    string? Price,
    string? Currency,
    string? City,
    string? Region,
    string Status,
    int CategoryId,
    string? ContentLocale,
    string CreatedAt,
    string? PublishedAt,
    string? LastModeratorComment);
