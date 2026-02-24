using NUnit.Framework;
using SportowyHub.UITests.Helpers;
using SportowyHub.UITests.Pages;

namespace SportowyHub.UITests.Tests;

[TestFixture]
public class SettingsTests : AppiumSetup
{
    private AppShellPage _shell = null!;
    private ProfilePage _profile = null!;

    [OneTimeSetUp]
    public void SetUpPages()
    {
        _shell = new AppShellPage(Driver);
        _profile = new ProfilePage(Driver);
    }

    [Test, Order(1)]
    public void NavigateToProfile_SettingsSectionIsVisible()
    {
        _shell.NavigateToProfile();

        Assert.That(_profile.IsSettingsSectionVisible(), Is.True,
            "Settings section with language and theme pickers should be visible on Profile page");
    }

    [Test, Order(2)]
    public void ChangeLanguageToEnglish_UITextUpdates()
    {
        // Select "English" (index 2: System=0, Polski=1, English=2)
        _profile.SelectLanguage(2);

        // Wait for AppShell to recreate after language change
        _shell.WaitForShellRecreation();

        // Navigate back to Profile tab after shell recreation
        _shell.NavigateToProfile();

        // Verify settings label shows English text
        var settingsLabel = _profile.GetSettingsLabelText();
        Assert.That(settingsLabel, Is.EqualTo("Settings"),
            $"Settings label should show 'Settings' in English, but got '{settingsLabel}'");

        // Verify language picker shows "English" as selected
        var pickerText = _profile.GetLanguagePickerText();
        Assert.That(pickerText, Is.EqualTo("English"),
            $"Language picker should show 'English', but got '{pickerText}'");
    }

    [Test, Order(3)]
    public void ChangeThemeToDark_ColorsUpdate()
    {
        // Select "Dark" (index 2: System=0, Light=1, Dark=2)
        _profile.SelectTheme(2);

        // Wait for AppShell to recreate after theme change
        _shell.WaitForShellRecreation();

        // Navigate back to Profile tab after shell recreation
        _shell.NavigateToProfile();

        // Capture screenshot for color verification
        var screenshot = ScreenshotHelper.CaptureScreenshot(Driver);
        var (width, height) = ScreenshotHelper.GetScreenshotSize(screenshot);

        // Verify dark background: sample the center of the page content area
        // The content area is roughly in the middle of the screen
        var (bgR, bgG, bgB) = ScreenshotHelper.GetAverageColor(
            screenshot,
            width / 4,       // x: 25% from left
            height / 2,      // y: center vertically
            width / 2,       // sample width: 50% of screen
            height / 10);    // sample height: 10% of screen

        Assert.That(bgR, Is.LessThan(50),
            $"Dark theme background red component should be < 50, got {bgR}");
        Assert.That(bgG, Is.LessThan(50),
            $"Dark theme background green component should be < 50, got {bgG}");
        Assert.That(bgB, Is.LessThan(50),
            $"Dark theme background blue component should be < 50, got {bgB}");

        // Verify tab bar icons are light/white in dark theme
        // Tab bar is at the bottom of the screen; sample the icon area
        var (iconR, iconG, iconB) = ScreenshotHelper.GetAverageColor(
            screenshot,
            width / 8,           // x: near the first tab icon
            height - height / 12, // y: near the bottom (tab bar area)
            width * 3 / 4,       // sample across most of the tab bar width
            height / 20);        // small vertical slice

        // In dark theme, the tab bar background should also be dark
        // and the active tab icon should use PrimaryDark (#FF3B4D) or white
        // We verify the overall tab bar area is dark (not light theme white)
        Assert.That(iconR + iconG + iconB < 400,
            Is.True,
            $"Dark theme tab bar area should have dark tones, got RGB({iconR},{iconG},{iconB})");

        // Verify theme picker retains "Dark" selection
        var pickerText = _profile.GetThemePickerText();
        Assert.That(pickerText, Is.EqualTo("Dark"),
            $"Theme picker should show 'Dark', but got '{pickerText}'");
    }
}
