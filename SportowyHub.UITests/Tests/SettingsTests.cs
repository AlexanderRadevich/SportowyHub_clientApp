using SportowyHub.UITests.Helpers;
using SportowyHub.UITests.Pages;

namespace SportowyHub.UITests.Tests;

[Collection(AppiumDriverCollection.Name)]
public class SettingsTests(AppiumDriverFixture fixture)
{
    private readonly AppShellPage _shell = new(fixture.Driver);
    private readonly ProfilePage _profile = new(fixture.Driver);

    [Fact, TestPriority(1)]
    public void NavigateToProfile_SettingsSectionIsVisible()
    {
        _shell.NavigateToProfile();

        Assert.True(_profile.IsSettingsSectionVisible(),
            "Settings section with language and theme pickers should be visible on Profile page");
    }

    [Fact, TestPriority(2)]
    public void ChangeLanguageToEnglish_UITextUpdates()
    {
        _profile.SelectLanguage(2);

        _shell.WaitForShellRecreation();

        _shell.NavigateToProfile();

        var settingsLabel = _profile.GetSettingsLabelText();
        Assert.Equal("Settings", settingsLabel);

        var pickerText = _profile.GetLanguagePickerText();
        Assert.Equal("English", pickerText);
    }

    [Fact, TestPriority(3)]
    public void ChangeThemeToDark_ColorsUpdate()
    {
        _profile.SelectTheme(2);

        _shell.WaitForShellRecreation();

        _shell.NavigateToProfile();

        var screenshot = ScreenshotHelper.CaptureScreenshot(fixture.Driver);
        var (width, height) = ScreenshotHelper.GetScreenshotSize(screenshot);

        var (bgR, bgG, bgB) = ScreenshotHelper.GetAverageColor(
            screenshot,
            width / 4,
            height / 2,
            width / 2,
            height / 10);

        Assert.True(bgR < 60,
            $"Dark theme background red component should be < 60, got {bgR}");
        Assert.True(bgG < 60,
            $"Dark theme background green component should be < 60, got {bgG}");
        Assert.True(bgB < 60,
            $"Dark theme background blue component should be < 60, got {bgB}");

        var (iconR, iconG, iconB) = ScreenshotHelper.GetAverageColor(
            screenshot,
            width / 8,
            height - height / 12,
            width * 3 / 4,
            height / 20);

        Assert.True(iconR + iconG + iconB < 400,
            $"Dark theme tab bar area should have dark tones, got RGB({iconR},{iconG},{iconB})");

        var pickerText = _profile.GetThemePickerText();
        Assert.Equal("Dark", pickerText);
    }
}
