using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using SportowyHub.UITests.Config;

namespace SportowyHub.UITests.Pages;

public class SearchPage
{
    private readonly AndroidDriver _driver;
    private readonly WebDriverWait _wait;

    public SearchPage(AndroidDriver driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TestConfig.DefaultWaitSeconds));
    }

    public void TypeSearch(string text)
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("SearchEntry")));
        entry.SendKeys(text);
    }

    public string GetSearchText()
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("SearchEntry")));
        return entry.Text;
    }

    public void ClearSearch()
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("SearchEntry")));
        entry.Clear();
    }
}
