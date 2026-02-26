using NUnit.Framework;
using SportowyHub.UITests.Helpers;
using SportowyHub.UITests.Pages;

namespace SportowyHub.UITests.Tests;

[TestFixture]
public class AuthScreenTests : AppiumSetup
{
    private AppShellPage _shell = null!;
    private ProfilePage _profile = null!;
    private LoginPage _login = null!;
    private RegisterPage _register = null!;

    [OneTimeSetUp]
    public void SetUpPages()
    {
        _shell = new AppShellPage(Driver);
        _profile = new ProfilePage(Driver);
        _login = new LoginPage(Driver);
        _register = new RegisterPage(Driver);
    }

    [Test, Order(1)]
    public void TapSignIn_LoginScreenOpens()
    {
        _shell.NavigateToProfile();
        _profile.TapSignIn();

        Assert.That(_login.IsHeadlineVisible(), Is.True,
            "Login page headline should be visible after tapping Sign In");

        Driver.Navigate().Back();
    }

    [Test, Order(2)]
    public void TapCreateAccount_RegisterScreenOpens()
    {
        _shell.NavigateToProfile();
        _profile.TapCreateAccount();

        Assert.That(_register.IsHeadlineVisible(), Is.True,
            "Register page headline should be visible after tapping Create Account");

        Driver.Navigate().Back();
    }
}
