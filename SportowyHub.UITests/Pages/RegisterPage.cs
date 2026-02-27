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
        PasteIntoEntry(entry, text);
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
        PasteIntoEntry(entry, text);
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
        PasteIntoEntry(entry, text);
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
        PasteIntoEntry(entry, text);
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
