using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using SportowyHub.UITests.Config;

namespace SportowyHub.UITests.Pages;

public class AppShellPage
{
    private readonly AndroidDriver _driver;
    private readonly WebDriverWait _wait;

    public AppShellPage(AndroidDriver driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TestConfig.DefaultWaitSeconds));
    }

    /// <summary>
    /// Navigates to a tab by its content-desc (which is the localized tab title on Android).
    /// </summary>
    public void NavigateToTab(string contentDesc)
    {
        var tab = _wait.Until(d => d.FindElement(MobileBy.AccessibilityId(contentDesc)));
        tab.Click();
    }

    /// <summary>
    /// Finds and clicks a tab by trying multiple localized names.
    /// Useful when the current UI language is unknown.
    /// </summary>
    public void NavigateToTabByNames(params string[] possibleNames)
    {
        _wait.Until(d =>
        {
            foreach (var name in possibleNames)
            {
                try
                {
                    var tab = d.FindElement(MobileBy.AccessibilityId(name));
                    tab.Click();
                    return tab;
                }
                catch (NoSuchElementException)
                {
                    // Try next name
                }
            }
            return null;
        });
    }

    public void NavigateToHome() => NavigateToTabByNames("Strona główna", "Home");
    public void NavigateToSearch() => NavigateToTabByNames("Szukaj", "Search");
    public void NavigateToFavorites() => NavigateToTabByNames("Ulubione", "Favorites");
    public void NavigateToProfile() => NavigateToTabByNames("Profil", "Profile");

    /// <summary>
    /// Waits for the shell to finish recreating after a language or theme change.
    /// </summary>
    public void WaitForShellRecreation()
    {
        Thread.Sleep(TestConfig.ShellRecreationWaitMs);
    }
}
