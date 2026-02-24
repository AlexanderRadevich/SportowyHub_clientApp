namespace SportowyHub.UITests.Config;

public static class TestConfig
{
    public static readonly Uri AppiumServerUrl = new("http://127.0.0.1:4723");

    public const string PlatformName = "Android";
    public const string AutomationName = "UiAutomator2";
    public const string AppPackage = "com.companyname.sportowyhub";
    public const string AppActivity = "crc649608e60ac3eeeee4.MainActivity";

    /// <summary>
    /// Timeout in seconds for waiting on elements.
    /// </summary>
    public const int DefaultWaitSeconds = 10;

    /// <summary>
    /// Extra wait in milliseconds after AppShell recreation (language/theme change).
    /// </summary>
    public const int ShellRecreationWaitMs = 3000;
}
