using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using SportowyHub.UITests.Config;

namespace SportowyHub.UITests.Pages;

public class ProfilePage
{
    private readonly AndroidDriver _driver;
    private readonly WebDriverWait _wait;

    public ProfilePage(AndroidDriver driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TestConfig.DefaultWaitSeconds));
    }

    private AppiumElement FindLanguagePicker() =>
        (AppiumElement)_wait.Until(d => d.FindElement(MobileBy.Id("LanguagePicker")));

    private AppiumElement FindThemePicker() =>
        (AppiumElement)_wait.Until(d => d.FindElement(MobileBy.Id("ThemePicker")));

    private AppiumElement FindSettingsLabel() =>
        (AppiumElement)_wait.Until(d => d.FindElement(MobileBy.Id("SettingsLabel")));

    private AppiumElement FindLanguageLabel() =>
        (AppiumElement)_wait.Until(d => d.FindElement(MobileBy.Id("LanguageLabel")));

    private AppiumElement FindThemeLabel() =>
        (AppiumElement)_wait.Until(d => d.FindElement(MobileBy.Id("ThemeLabel")));

    public bool IsSettingsSectionVisible()
    {
        try
        {
            FindLanguagePicker();
            FindThemePicker();
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    public string GetSettingsLabelText() => FindSettingsLabel().Text;
    public string GetLanguageLabelText() => FindLanguageLabel().Text;
    public string GetThemeLabelText() => FindThemeLabel().Text;

    /// <summary>
    /// Selects an item from the language picker by tapping it and choosing from the native dialog.
    /// On Android, MAUI Pickers open a native spinner/dialog.
    /// </summary>
    public void SelectLanguage(int index)
    {
        var picker = FindLanguagePicker();
        picker.Click();
        SelectPickerItem(index);
    }

    /// <summary>
    /// Selects an item from the theme picker.
    /// </summary>
    public void SelectTheme(int index)
    {
        var picker = FindThemePicker();
        picker.Click();
        SelectPickerItem(index);
    }

    /// <summary>
    /// Gets the currently displayed text of the language picker.
    /// </summary>
    public string GetLanguagePickerText()
    {
        var picker = FindLanguagePicker();
        return picker.Text;
    }

    /// <summary>
    /// Gets the currently displayed text of the theme picker.
    /// </summary>
    public string GetThemePickerText()
    {
        var picker = FindThemePicker();
        return picker.Text;
    }

    /// <summary>
    /// Selects an item from the Android native picker dialog by index.
    /// MAUI Pickers on Android use an AlertDialog with a ListView of options.
    /// </summary>
    private void SelectPickerItem(int index)
    {
        // Wait for the picker dialog to appear
        Thread.Sleep(500);

        // Android MAUI picker shows items in a ListView within an AlertDialog.
        // Find all items in the picker dialog and tap the one at the desired index.
        var items = _wait.Until(d =>
        {
            var elements = d.FindElements(MobileBy.ClassName("android.widget.CheckedTextView"));
            return elements.Count > index ? elements : null;
        });

        if (items != null && items.Count > index)
        {
            items[index].Click();
        }
    }
}
