namespace SportowyHub.Models.Api;

public static class SearchResultItemExtensions
{
    private static readonly Color NewBadgeColor = Color.FromArgb("#1A1A1A");
    private static readonly Color UsedBadgeColor = Color.FromArgb("#F59E0B");

    public static ListingSummary ToListingSummary(this SearchResultItem item) =>
        new(
            Id: item.Id,
            Slug: item.Slug,
            Title: item.Title,
            Price: item.Price.HasValue ? (decimal)item.Price.Value : null,
            Currency: item.Currency,
            City: item.City,
            CategoryId: int.TryParse(item.CategoryId, out var catId) ? catId : 0,
            ContentLocale: null,
            PublishedAt: item.PublishedAt,
            ViewCount: item.ViewCount);

    public static (bool HasCondition, string? Text, Color BadgeColor) ExtractCondition(this SearchResultItem item)
    {
        var conditionAttr = item.Attributes?.FirstOrDefault(a => a.Key == "condition");
        if (conditionAttr is null)
        {
            return (false, null, Colors.Transparent);
        }

        return conditionAttr.Value switch
        {
            "new" => (true, Resources.Strings.AppResources.FilterConditionNew, NewBadgeColor),
            "used" => (true, Resources.Strings.AppResources.FilterConditionUsed, UsedBadgeColor),
            _ => (false, null, Colors.Transparent)
        };
    }
}
