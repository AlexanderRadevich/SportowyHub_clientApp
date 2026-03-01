namespace SportowyHub.Models.Api;

public record SearchResultItem(
    string Id,
    string Slug,
    int SerialId,
    string Title,
    string Description,
    string CategoryId,
    string CategoryPath,
    string Sport,
    float? Price,
    string? Currency,
    string? City,
    string? Region,
    string Status,
    string OwnerTrustLevel,
    string CreatedAt,
    string? PublishedAt,
    GeoLocation? Location = null,
    List<SearchAttribute>? Attributes = null);

public record GeoLocation(double Lat, double Lon);

public record SearchAttribute(string Key, string Value);
