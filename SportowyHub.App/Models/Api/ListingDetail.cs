using System.Text.Json.Serialization;
using SportowyHub.Services.Api;

namespace SportowyHub.Models.Api;

public record ListingDetail(
    string Id,
    string? Slug,
    string Title,
    string? Description,
    [property: JsonConverter(typeof(FlexibleDecimalConverter))] decimal? Price,
    string? Currency,
    string? City,
    string? Region,
    string Status,
    int CategoryId,
    string? ContentLocale,
    string? Condition,
    string CreatedAt,
    string? PublishedAt,
    string? LastModeratorComment,
    int ViewCount = 0);
