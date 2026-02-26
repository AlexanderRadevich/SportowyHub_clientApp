using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using SportowyHub.UITests.Config;

namespace SportowyHub.UITests.Pages;

public class LoginPage
{
    private readonly AndroidDriver _driver;
    private readonly WebDriverWait _wait;

    public LoginPage(AndroidDriver driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TestConfig.DefaultWaitSeconds));
    }

    public bool IsHeadlineVisible()
    {
        try
        {
            _wait.Until(d => d.FindElement(MobileBy.Id("LoginHeadline")));
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }
}
