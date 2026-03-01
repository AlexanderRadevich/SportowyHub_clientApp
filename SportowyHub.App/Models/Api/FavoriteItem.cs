namespace SportowyHub.Models.Api;

public record FavoriteItem(
    string Id,
    string Title,
    string? Price,
    string? Currency,
    string? City,
    string Status,
    string? Slug,
    int SerialId);
