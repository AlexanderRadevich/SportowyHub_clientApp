using System.Globalization;

namespace SportowyHub.Converters;

public class BoolToHeartConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isFavorited && isFavorited)
        {
            return "icon_heart_filled.png";
        }
        return "icon_heart_outline.png";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
