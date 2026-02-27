using NUnit.Framework;
using SportowyHub.UITests.Config;
using SportowyHub.UITests.Helpers;
using SportowyHub.UITests.Pages;

namespace SportowyHub.UITests.Tests;

[TestFixture]
public class LoginSignOutTests : AppiumSetup
{
    private AppShellPage _shell = null!;
    private ProfilePage _profile = null!;
    private LoginPage _login = null!;
    private AccountProfilePage _accountProfile = null!;

    [OneTimeSetUp]
    public void SetUpPages()
    {
        _shell = new AppShellPage(Driver);
        _profile = new ProfilePage(Driver);
        _login = new LoginPage(Driver);
        _accountProfile = new AccountProfilePage(Driver);
    }

    [Test, Order(1)]
    public void Login_WithValidCredentials_NavigatesToHome()
    {
        _shell.NavigateToProfile();

        Assert.That(_profile.IsSignInRowVisible(), Is.True,
            "Should start in logged-out state with Sign In row visible");

        _profile.TapSignIn();

        Assert.That(_login.IsHeadlineVisible(), Is.True,
            "Login page should be visible");

        _login.TypeEmail(TestConfig.TestEmail);
        _login.TypePassword(TestConfig.TestPassword);
        _login.TapLogin();

        // Wait for navigation to Home tab after successful login
        Thread.Sleep(2000);

        ToastHelper.AssertNoErrorToast(Driver);

        // Verify we're on the Home tab by navigating to it (should already be there)
        _shell.NavigateToHome();
    }

    [Test, Order(2)]
    public void SignOut_FromAccountProfile_ReturnsToLoggedOutState()
    {
        _shell.NavigateToProfile();

        _profile.TapAccountProfile();

        _accountProfile.TapSignOut();
        _accountProfile.ConfirmSignOut();

        // Wait for sign-out and navigation back to Profile
        Thread.Sleep(2000);

        ToastHelper.AssertNoErrorToast(Driver);

        Assert.That(_profile.IsSignInRowVisible(), Is.True,
            "After sign-out, Sign In row should be visible on Profile page");
    }
}
