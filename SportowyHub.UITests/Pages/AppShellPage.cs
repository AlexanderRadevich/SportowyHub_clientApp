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

    public void NavigateToTab(string automationId)
    {
        var tab = _wait.Until(d => d.FindElement(MobileBy.AccessibilityId(automationId)));
        tab.Click();
    }

    public void NavigateToHome() => NavigateToTab("TabHome");
    public void NavigateToSearch() => NavigateToTab("TabSearch");
    public void NavigateToFavorites() => NavigateToTab("TabFavorites");
    public void NavigateToProfile() => NavigateToTab("TabProfile");

    /// <summary>
    /// Waits for the shell to finish recreating after a language or theme change.
    /// </summary>
    public void WaitForShellRecreation()
    {
        Thread.Sleep(TestConfig.ShellRecreationWaitMs);
        // Re-verify the tab bar is present after recreation
        _wait.Until(d => d.FindElement(MobileBy.AccessibilityId("TabProfile")));
    }
}
