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
        PasteIntoEntry(entry, text);
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
        PasteIntoEntry(entry, text);
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

    public void TapLogin()
    {
        var button = _wait.Until(d => d.FindElement(MobileBy.Id("LoginButton")));
        button.Click();
    }

    /// <summary>
    /// Pastes text into a focused Entry via clipboard + Ctrl+V.
    /// This sends real Android KeyEvents through the input system,
    /// ensuring MAUI's TextWatcher fires and the binding updates.
    /// </summary>
    private void PasteIntoEntry(IWebElement entry, string text)
    {
        entry.Click();
        Thread.Sleep(300);

        _driver.SetClipboardText(text, "plaintext");

        // Ctrl+V: keycode 50 = KEYCODE_V, metastate 4096 = META_CTRL_ON
        _driver.ExecuteScript("mobile: pressKey", new Dictionary<string, object>
        {
            { "keycode", 50 },
            { "metastate", 4096 }
        });

        Thread.Sleep(500);
    }
}
