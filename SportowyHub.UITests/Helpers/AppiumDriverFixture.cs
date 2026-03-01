using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using SportowyHub.UITests.Config;

namespace SportowyHub.UITests.Helpers;

public sealed class AppiumDriverFixture : IAsyncLifetime
{
    public AndroidDriver Driver { get; private set; } = null!;

    public ValueTask InitializeAsync()
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

        return ValueTask.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        Driver?.Quit();
        return ValueTask.CompletedTask;
    }
}
