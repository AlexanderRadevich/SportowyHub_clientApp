using System.Text.Json;
using FluentAssertions;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;

namespace SportowyHub.Tests.Models;

public class ListingDeserializationTests
{
    private static readonly JsonSerializerOptions _options = SportowyHubJsonContext.Default.Options;

    [Fact]
    public void ListingSummary_WithViewCount_DeserializesCorrectly()
    {
        var json = """{"id":"abc","title":"Bike","category_id":1,"view_count":1234}""";

        var result = JsonSerializer.Deserialize<ListingSummary>(json, _options);

        result.Should().NotBeNull();
        result!.ViewCount.Should().Be(1234);
    }

    [Fact]
    public void ListingSummary_WithoutViewCount_DefaultsToZero()
    {
        var json = """{"id":"abc","title":"Bike","category_id":1}""";

        var result = JsonSerializer.Deserialize<ListingSummary>(json, _options);

        result.Should().NotBeNull();
        result!.ViewCount.Should().Be(0);
    }

    [Fact]
    public void ListingDetail_WithViewCount_DeserializesCorrectly()
    {
        var json = """{"id":"abc","title":"Bike","status":"published","category_id":1,"created_at":"2025-01-01","view_count":567}""";

        var result = JsonSerializer.Deserialize<ListingDetail>(json, _options);

        result.Should().NotBeNull();
        result!.ViewCount.Should().Be(567);
    }

    [Fact]
    public void ListingDetail_WithoutViewCount_DefaultsToZero()
    {
        var json = """{"id":"abc","title":"Bike","status":"published","category_id":1,"created_at":"2025-01-01"}""";

        var result = JsonSerializer.Deserialize<ListingDetail>(json, _options);

        result.Should().NotBeNull();
        result!.ViewCount.Should().Be(0);
    }
}
