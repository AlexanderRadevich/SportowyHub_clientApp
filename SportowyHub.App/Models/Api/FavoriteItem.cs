using System.Text.Json.Serialization;
using SportowyHub.Services.Api;

namespace SportowyHub.Models.Api;

public record FavoriteItem(
    string Id,
    string Title,
    [property: JsonConverter(typeof(FlexibleDecimalConverter))] decimal? Price,
    string? Currency,
    string? City,
    string Status,
    string? Slug,
    int SerialId);
