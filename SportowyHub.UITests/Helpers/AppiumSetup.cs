using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using SportowyHub.UITests.Config;

namespace SportowyHub.UITests.Helpers;

[TestFixture]
public class AppiumSetup
{
    protected AndroidDriver Driver { get; private set; } = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var options = new AppiumOptions
        {
            PlatformName = TestConfig.PlatformName,
            AutomationName = TestConfig.AutomationName
        };
        options.AddAdditionalAppiumOption("appPackage", TestConfig.AppPackage);
        options.AddAdditionalAppiumOption("appActivity", TestConfig.AppActivity);
        options.AddAdditionalAppiumOption("noReset", true);

        Driver = new AndroidDriver(TestConfig.AppiumServerUrl, options, TimeSpan.FromSeconds(60));
        Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(TestConfig.DefaultWaitSeconds);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Driver?.Quit();
    }
}
