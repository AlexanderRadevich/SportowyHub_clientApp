using OpenQA.Selenium.Appium.Android;
using SkiaSharp;

namespace SportowyHub.UITests.Helpers;

public static class ScreenshotHelper
{
    /// <summary>
    /// Captures a screenshot from the Appium driver and returns the raw PNG bytes.
    /// </summary>
    public static byte[] CaptureScreenshot(AndroidDriver driver)
    {
        var screenshot = driver.GetScreenshot();
        return screenshot.AsByteArray;
    }

    /// <summary>
    /// Gets the RGB color of a pixel at the given coordinates from a PNG screenshot.
    /// </summary>
    public static (int R, int G, int B) GetPixelColor(byte[] pngBytes, int x, int y)
    {
        using var bitmap = SKBitmap.Decode(pngBytes);
        if (x < 0 || x >= bitmap.Width || y < 0 || y >= bitmap.Height)
            throw new ArgumentOutOfRangeException($"Pixel ({x}, {y}) out of bounds ({bitmap.Width}x{bitmap.Height})");

        var pixel = bitmap.GetPixel(x, y);
        return (pixel.Red, pixel.Green, pixel.Blue);
    }

    /// <summary>
    /// Samples the average RGB color of a rectangular region from a PNG screenshot.
    /// </summary>
    public static (int R, int G, int B) GetAverageColor(byte[] pngBytes, int x, int y, int width, int height)
    {
        using var bitmap = SKBitmap.Decode(pngBytes);

        long totalR = 0, totalG = 0, totalB = 0;
        int count = 0;

        int stepX = Math.Max(1, width / 5);
        int stepY = Math.Max(1, height / 5);

        for (int sx = x; sx < x + width && sx < bitmap.Width; sx += stepX)
        {
            for (int sy = y; sy < y + height && sy < bitmap.Height; sy += stepY)
            {
                var pixel = bitmap.GetPixel(sx, sy);
                totalR += pixel.Red;
                totalG += pixel.Green;
                totalB += pixel.Blue;
                count++;
            }
        }

        return count > 0
            ? ((int)(totalR / count), (int)(totalG / count), (int)(totalB / count))
            : (0, 0, 0);
    }

    /// <summary>
    /// Returns the screenshot dimensions (width, height).
    /// </summary>
    public static (int Width, int Height) GetScreenshotSize(byte[] pngBytes)
    {
        using var bitmap = SKBitmap.Decode(pngBytes);
        return (bitmap.Width, bitmap.Height);
    }
}
