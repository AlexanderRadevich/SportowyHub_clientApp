using System.Globalization;

namespace SportowyHub.Converters;

public class BoolToChipColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var isSelected = value is true;
        var mode = parameter as string;

        if (Application.Current is null)
        {
            return mode == "text" ? Colors.Black : Colors.Transparent;
        }

        var isDark = Application.Current.RequestedTheme == AppTheme.Dark;

        if (mode == "text")
        {
            if (isSelected)
            {
                return Colors.White;
            }

            var key = isDark ? "TextPrimaryDark" : "TextPrimary";
            return Application.Current.Resources.TryGetValue(key, out var textColor)
                ? textColor
                : (isDark ? Colors.White : Colors.Black);
        }

        if (isSelected)
        {
            var key = isDark ? "PrimaryDark" : "Primary";
            return Application.Current.Resources.TryGetValue(key, out var primary) ? primary : Colors.Red;
        }

        {
            var key = isDark ? "SearchBarBackgroundDark" : "SearchBarBackground";
            return Application.Current.Resources.TryGetValue(key, out var bg) ? bg : Colors.LightGray;
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
