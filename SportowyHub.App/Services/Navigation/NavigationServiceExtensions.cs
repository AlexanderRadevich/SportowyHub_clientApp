using System.Globalization;

namespace SportowyHub.Services.Navigation;

public static class NavigationServiceExtensions
{
    public static Task GoToListingDetailAsync(
        this INavigationService nav,
        string id,
        string title,
        decimal? price,
        string? currency,
        string? city)
    {
        var priceStr = price?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
        var query = $"listing-detail?id={Uri.EscapeDataString(id)}" +
                    $"&title={Uri.EscapeDataString(title)}" +
                    $"&price={Uri.EscapeDataString(priceStr)}" +
                    $"&currency={Uri.EscapeDataString(currency ?? string.Empty)}" +
                    $"&city={Uri.EscapeDataString(city ?? string.Empty)}";
        return nav.GoToAsync(query);
    }
}
