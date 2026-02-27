using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using SportowyHub.UITests.Config;

namespace SportowyHub.UITests.Pages;

public class AccountProfilePage
{
    private readonly AndroidDriver _driver;
    private readonly WebDriverWait _wait;

    public AccountProfilePage(AndroidDriver driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TestConfig.DefaultWaitSeconds));
    }

    public void TapSignOut()
    {
        var button = _wait.Until(d => d.FindElement(MobileBy.Id("SignOutButton")));
        button.Click();
    }

    /// <summary>
    /// Confirms the sign-out AlertDialog by tapping the confirm button.
    /// Tries localized button texts across all 4 supported languages.
    /// </summary>
    public void ConfirmSignOut()
    {
        Thread.Sleep(500);

        string[] confirmTexts = ["Wyloguj się", "Sign Out", "Вийти", "Выйти"];

        _wait.Until(d =>
        {
            foreach (var text in confirmTexts)
            {
                try
                {
                    var button = d.FindElement(MobileBy.XPath(
                        $"//android.widget.Button[@text='{text}']"));
                    button.Click();
                    return button;
                }
                catch (NoSuchElementException)
                {
                    // Try next language
                }
            }
            return null;
        });
    }
}
