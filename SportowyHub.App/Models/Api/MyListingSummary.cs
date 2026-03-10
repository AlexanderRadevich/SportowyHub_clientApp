using System.Text.Json.Serialization;
using SportowyHub.Services.Api;

namespace SportowyHub.Models.Api;

public record MyListingSummary(
    string Id,
    string? Slug,
    string Title,
    string Status,
    [property: JsonConverter(typeof(FlexibleDecimalConverter))] decimal? Price,
    string? Currency,
    string? ContentLocale,
    string CreatedAt,
    string? PublishedAt,
    int ViewCount = 0);
