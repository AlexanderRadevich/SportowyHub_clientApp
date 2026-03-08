using System.Globalization;

namespace SportowyHub.Converters;

public class CategoryToColorConverter : IValueConverter
{
    private static readonly string[] LightKeys =
    [
        "PlaceholderCat0", "PlaceholderCat1", "PlaceholderCat2", "PlaceholderCat3",
        "PlaceholderCat4", "PlaceholderCat5", "PlaceholderCat6", "PlaceholderCat7",
        "PlaceholderCat8", "PlaceholderCat9"
    ];

    private static readonly string[] DarkKeys =
    [
        "PlaceholderCat0Dark", "PlaceholderCat1Dark", "PlaceholderCat2Dark", "PlaceholderCat3Dark",
        "PlaceholderCat4Dark", "PlaceholderCat5Dark", "PlaceholderCat6Dark", "PlaceholderCat7Dark",
        "PlaceholderCat8Dark", "PlaceholderCat9Dark"
    ];

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (Application.Current is null)
        {
            return Colors.LightGray;
        }

        var isDark = Application.Current.RequestedTheme == AppTheme.Dark;
        var keys = isDark ? DarkKeys : LightKeys;
        var fallbackKey = isDark ? "PlaceholderCatDefaultDark" : "PlaceholderCatDefault";

        if (value is int categoryId && categoryId >= 0)
        {
            var index = categoryId % keys.Length;
            if (Application.Current.Resources.TryGetValue(keys[index], out var color))
            {
                return color;
            }
        }

        return Application.Current.Resources.TryGetValue(fallbackKey, out var fallback)
            ? fallback
            : Colors.LightGray;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
