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

    public void TypeEmail(string text)
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("LoginEmailEntry")));
        entry.SendKeys(text);
    }

    public string GetEmailText()
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("LoginEmailEntry")));
        return entry.Text;
    }

    public void ClearEmail()
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("LoginEmailEntry")));
        entry.Clear();
    }

    public void TypePassword(string text)
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("LoginPasswordEntry")));
        entry.SendKeys(text);
    }

    public string GetPasswordText()
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("LoginPasswordEntry")));
        return entry.Text;
    }

    public void ClearPassword()
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("LoginPasswordEntry")));
        entry.Clear();
    }
}
