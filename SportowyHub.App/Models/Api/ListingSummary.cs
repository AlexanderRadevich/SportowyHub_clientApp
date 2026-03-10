using System.Text.Json.Serialization;
using SportowyHub.Services.Api;

namespace SportowyHub.Models.Api;

public record ListingSummary(
    string Id,
    string? Slug,
    string Title,
    [property: JsonConverter(typeof(FlexibleDecimalConverter))] decimal? Price,
    string? Currency,
    string? City,
    int CategoryId,
    string? ContentLocale,
    string? PublishedAt,
    int ViewCount = 0);
