using System.Text.Json;
using FluentAssertions;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;

namespace SportowyHub.Tests.Models;

public class FavoriteActionResponseTests
{
    private static readonly JsonSerializerOptions _options = SportowyHubJsonContext.Default.Options;

    [Fact]
    public void Deserialize_WhenFavoritesCountPresent_PopulatesValue()
    {
        var json = """{"status":"added","favorites_count":42}""";

        var result = JsonSerializer.Deserialize<FavoriteActionResponse>(json, _options);

        result.Should().NotBeNull();
        result!.Status.Should().Be("added");
        result.FavoritesCount.Should().Be(42);
    }

    [Fact]
    public void Deserialize_WhenFavoritesCountAbsent_FavoritesCountIsNull()
    {
        var json = """{"status":"added"}""";

        var result = JsonSerializer.Deserialize<FavoriteActionResponse>(json, _options);

        result.Should().NotBeNull();
        result!.FavoritesCount.Should().BeNull();
    }

    [Fact]
    public void Deserialize_WhenFavoritesCountIsNull_FavoritesCountIsNull()
    {
        var json = """{"status":"added","favorites_count":null}""";

        var result = JsonSerializer.Deserialize<FavoriteActionResponse>(json, _options);

        result.Should().NotBeNull();
        result!.FavoritesCount.Should().BeNull();
    }

    [Fact]
    public void Deserialize_WhenFavoritesCountIsZero_PopulatesZero()
    {
        var json = """{"status":"removed","favorites_count":0}""";

        var result = JsonSerializer.Deserialize<FavoriteActionResponse>(json, _options);

        result.Should().NotBeNull();
        result!.FavoritesCount.Should().Be(0);
    }

    [Fact]
    public void Deserialize_WhenStatusIsPresent_PopulatesStatus()
    {
        var json = """{"status":"added","favorites_count":5}""";

        var result = JsonSerializer.Deserialize<FavoriteActionResponse>(json, _options);

        result!.Status.Should().Be("added");
    }
}
