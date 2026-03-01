using SportowyHub.UITests.Helpers;
using SportowyHub.UITests.Pages;

namespace SportowyHub.UITests.Tests;

[Collection(AppiumDriverCollection.Name)]
public class AuthScreenTests(AppiumDriverFixture fixture)
{
    private readonly AppShellPage _shell = new(fixture.Driver);
    private readonly ProfilePage _profile = new(fixture.Driver);
    private readonly LoginPage _login = new(fixture.Driver);
    private readonly RegisterPage _register = new(fixture.Driver);

    [Fact, TestPriority(1)]
    public void TapSignIn_LoginScreenOpens()
    {
        _shell.NavigateToProfile();
        _profile.TapSignIn();

        Assert.True(_login.IsHeadlineVisible(),
            "Login page headline should be visible after tapping Sign In");

        fixture.Driver.Navigate().Back();
    }

    [Fact, TestPriority(2)]
    public void TapCreateAccount_RegisterScreenOpens()
    {
        _shell.NavigateToProfile();
        _profile.TapCreateAccount();

        Assert.True(_register.IsHeadlineVisible(),
            "Register page headline should be visible after tapping Create Account");

        fixture.Driver.Navigate().Back();
    }
}
