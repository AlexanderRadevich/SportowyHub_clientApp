using SportowyHub.UITests.Config;
using SportowyHub.UITests.Helpers;
using SportowyHub.UITests.Pages;

namespace SportowyHub.UITests.Tests;

[Collection(AppiumDriverCollection.Name)]
public class LoginSignOutTests(AppiumDriverFixture fixture)
{
    private readonly AppShellPage _shell = new(fixture.Driver);
    private readonly ProfilePage _profile = new(fixture.Driver);
    private readonly LoginPage _login = new(fixture.Driver);
    private readonly AccountProfilePage _accountProfile = new(fixture.Driver);

    [Fact, TestPriority(1)]
    public void Login_WithValidCredentials_NavigatesToHome()
    {
        _shell.NavigateToProfile();

        Assert.True(_profile.IsSignInRowVisible(),
            "Should start in logged-out state with Sign In row visible");

        _profile.TapSignIn();

        Assert.True(_login.IsHeadlineVisible(),
            "Login page should be visible");

        _login.TypeEmail(TestConfig.TestEmail);
        _login.TypePassword(TestConfig.TestPassword);
        _login.TapLogin();

        Thread.Sleep(2000);

        ToastHelper.AssertNoErrorToast(fixture.Driver);

        _shell.NavigateToHome();
    }

    [Fact, TestPriority(2)]
    public void SignOut_FromAccountProfile_ReturnsToLoggedOutState()
    {
        _shell.NavigateToProfile();

        _profile.TapAccountProfile();

        _accountProfile.TapSignOut();
        _accountProfile.ConfirmSignOut();

        Thread.Sleep(2000);

        ToastHelper.AssertNoErrorToast(fixture.Driver);

        Assert.True(_profile.IsSignInRowVisible(),
            "After sign-out, Sign In row should be visible on Profile page");
    }
}
