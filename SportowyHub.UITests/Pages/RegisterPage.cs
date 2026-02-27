using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using SportowyHub.UITests.Config;

namespace SportowyHub.UITests.Pages;

public class RegisterPage
{
    private readonly AndroidDriver _driver;
    private readonly WebDriverWait _wait;

    public RegisterPage(AndroidDriver driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TestConfig.DefaultWaitSeconds));
    }

    public bool IsHeadlineVisible()
    {
        try
        {
            _wait.Until(d => d.FindElement(MobileBy.Id("RegisterHeadline")));
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    public void TypeEmail(string text)
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("RegisterEmailEntry")));
        entry.SendKeys(text);
    }

    public string GetEmailText()
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("RegisterEmailEntry")));
        return entry.Text;
    }

    public void ClearEmail()
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("RegisterEmailEntry")));
        entry.Clear();
    }

    public void TypePhone(string text)
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("RegisterPhoneEntry")));
        entry.SendKeys(text);
    }

    public string GetPhoneText()
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("RegisterPhoneEntry")));
        return entry.Text;
    }

    public void ClearPhone()
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("RegisterPhoneEntry")));
        entry.Clear();
    }

    public void TypePassword(string text)
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("RegisterPasswordEntry")));
        entry.SendKeys(text);
    }

    public string GetPasswordText()
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("RegisterPasswordEntry")));
        return entry.Text;
    }

    public void ClearPassword()
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("RegisterPasswordEntry")));
        entry.Clear();
    }

    public void TypeConfirmPassword(string text)
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("RegisterConfirmPasswordEntry")));
        entry.SendKeys(text);
    }

    public string GetConfirmPasswordText()
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("RegisterConfirmPasswordEntry")));
        return entry.Text;
    }

    public void ClearConfirmPassword()
    {
        var entry = _wait.Until(d => d.FindElement(MobileBy.Id("RegisterConfirmPasswordEntry")));
        entry.Clear();
    }
}
