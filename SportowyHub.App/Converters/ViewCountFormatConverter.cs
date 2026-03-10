using System.Globalization;

namespace SportowyHub.Converters;

public class ViewCountFormatConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not int count || count <= 0)
        {
            return "0";
        }

        return count switch
        {
            >= 999_950 => FormatCompact(count, 1_000_000, "M"),
            >= 1_000 => FormatCompact(count, 1_000, "k"),
            _ => count.ToString(CultureInfo.InvariantCulture)
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    private static string FormatCompact(int count, int divisor, string suffix)
    {
        var scaled = (double)count / divisor;
        var formatted = scaled % 1 == 0
            ? ((int)scaled).ToString(CultureInfo.InvariantCulture)
            : scaled.ToString("0.#", CultureInfo.InvariantCulture);
        return $"{formatted}{suffix}";
    }
}
