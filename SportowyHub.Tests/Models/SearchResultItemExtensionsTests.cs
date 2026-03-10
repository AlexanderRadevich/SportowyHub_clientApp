using FluentAssertions;
using SportowyHub.Models.Api;

namespace SportowyHub.Tests.Models;

public class SearchResultItemExtensionsTests
{
    private static SearchResultItem CreateItem(
        string id = "abc",
        string slug = "test-slug",
        string title = "Ball",
        float? price = 29.99f,
        string? currency = "PLN",
        string? city = "Warsaw",
        string categoryId = "5",
        string? publishedAt = "2025-01-01",
        int viewCount = 150,
        List<SearchAttribute>? attributes = null) =>
        new(
            Id: id,
            Slug: slug,
            SerialId: 1,
            Title: title,
            Description: "desc",
            CategoryId: categoryId,
            CategoryPath: "sports",
            Sport: "football",
            Price: price,
            Currency: currency,
            City: city,
            Region: "Mazowieckie",
            Status: "active",
            OwnerTrustLevel: "verified",
            CreatedAt: "2025-01-01",
            PublishedAt: publishedAt,
            ViewCount: viewCount,
            Attributes: attributes);

    [Fact]
    public void ToListingSummary_MapsAllFields()
    {
        var item = CreateItem();

        var result = item.ToListingSummary();

        result.Id.Should().Be("abc");
        result.Slug.Should().Be("test-slug");
        result.Title.Should().Be("Ball");
        result.Price.Should().Be(29.99m);
        result.Currency.Should().Be("PLN");
        result.City.Should().Be("Warsaw");
        result.CategoryId.Should().Be(5);
        result.PublishedAt.Should().Be("2025-01-01");
        result.ViewCount.Should().Be(150);
        result.ContentLocale.Should().BeNull();
    }

    [Fact]
    public void ToListingSummary_NullOptionalFields_MapsToNull()
    {
        var item = CreateItem(price: null, currency: null, city: null, publishedAt: null);

        var result = item.ToListingSummary();

        result.Price.Should().BeNull();
        result.Currency.Should().BeNull();
        result.City.Should().BeNull();
        result.PublishedAt.Should().BeNull();
    }

    [Fact]
    public void ToListingSummary_CategoryIdStringToInt()
    {
        var item = CreateItem(categoryId: "42");

        var result = item.ToListingSummary();

        result.CategoryId.Should().Be(42);
    }

    [Fact]
    public void ExtractCondition_NewCondition_ReturnsNewBadge()
    {
        var item = CreateItem(attributes: [new SearchAttribute("condition", "new")]);

        var (hasCondition, text, badgeColor) = item.ExtractCondition();

        hasCondition.Should().BeTrue();
        text.Should().NotBeNullOrEmpty();
        badgeColor.Should().Be(Color.FromArgb("#1A1A1A"));
    }

    [Fact]
    public void ExtractCondition_UsedCondition_ReturnsUsedBadge()
    {
        var item = CreateItem(attributes: [new SearchAttribute("condition", "used")]);

        var (hasCondition, text, badgeColor) = item.ExtractCondition();

        hasCondition.Should().BeTrue();
        text.Should().NotBeNullOrEmpty();
        badgeColor.Should().Be(Color.FromArgb("#F59E0B"));
    }

    [Fact]
    public void ExtractCondition_NoConditionAttribute_ReturnsFalse()
    {
        var item = CreateItem(attributes: [new SearchAttribute("brand", "Nike")]);

        var (hasCondition, text, _) = item.ExtractCondition();

        hasCondition.Should().BeFalse();
        text.Should().BeNull();
    }

    [Fact]
    public void ExtractCondition_NullAttributes_ReturnsFalse()
    {
        var item = CreateItem(attributes: null);

        var (hasCondition, text, _) = item.ExtractCondition();

        hasCondition.Should().BeFalse();
        text.Should().BeNull();
    }
}
