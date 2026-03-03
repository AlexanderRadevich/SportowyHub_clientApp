namespace SportowyHub.Models.Api;

public record MediaItem(
    int Id,
    string ListingId,
    string Type,
    string MimeType,
    int Size,
    int Width,
    int Height,
    int SortOrder,
    MediaUrls Urls,
    string CreatedAt);

public record MediaUrls(
    string? Thumb160,
    string? Thumb320,
    string? Card640,
    string? Gallery1024,
    string? Gallery1920,
    string? Og1200x630);
