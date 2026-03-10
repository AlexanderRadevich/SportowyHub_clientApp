using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Platform;

namespace SportowyHub.Helpers;

public static class StatusBarHelper
{
    private static readonly Color LightBackground = Color.FromArgb("#FFFFFF");
    private static readonly Color DarkBackground = Color.FromArgb("#121212");

    public static void Apply(AppTheme theme)
    {
#if ANDROID
        if (!OperatingSystem.IsAndroidVersionAtLeast(23))
        {
            return;
        }
#endif

#if ANDROID || IOS
        var effectiveTheme = theme == AppTheme.Unspecified
            ? Application.Current?.RequestedTheme ?? AppTheme.Light
            : theme;

        var color = effectiveTheme == AppTheme.Dark ? DarkBackground : LightBackground;
        var style = effectiveTheme == AppTheme.Dark ? StatusBarStyle.LightContent : StatusBarStyle.DarkContent;

        StatusBar.SetColor(color);
        StatusBar.SetStyle(style);
#endif
    }
}
