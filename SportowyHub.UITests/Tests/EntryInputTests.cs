using NUnit.Framework;
using SportowyHub.UITests.Helpers;
using SportowyHub.UITests.Pages;

namespace SportowyHub.UITests.Tests;

[TestFixture]
public class EntryInputTests : AppiumSetup
{
    private AppShellPage _shell = null!;
    private SearchPage _search = null!;
    private ProfilePage _profile = null!;
    private LoginPage _login = null!;
    private RegisterPage _register = null!;

    [OneTimeSetUp]
    public void SetUpPages()
    {
        _shell = new AppShellPage(Driver);
        _search = new SearchPage(Driver);
        _profile = new ProfilePage(Driver);
        _login = new LoginPage(Driver);
        _register = new RegisterPage(Driver);
    }

    // --- Search screen ---

    [Test, Order(1)]
    public void SearchEntry_AcceptsTypedText()
    {
        _shell.NavigateToSearch();

        _search.TypeSearch("running shoes");
        Assert.That(_search.GetSearchText(), Is.EqualTo("running shoes"),
            "SearchEntry should contain the typed text");

        _search.ClearSearch();
        _shell.NavigateToHome();
    }

    // --- Login screen ---

    [Test, Order(2)]
    public void LoginEmailEntry_AcceptsTypedText()
    {
        _shell.NavigateToProfile();
        _profile.TapSignIn();

        _login.TypeEmail("test@example.com");
        Assert.That(_login.GetEmailText(), Is.EqualTo("test@example.com"),
            "LoginEmailEntry should contain the typed text");

        _login.ClearEmail();
    }

    [Test, Order(3)]
    public void LoginPasswordEntry_AcceptsTypedText()
    {
        _login.TypePassword("Password123!");
        Assert.That(_login.GetPasswordText(), Is.EqualTo("Password123!"),
            "LoginPasswordEntry should contain the typed text");

        _login.ClearPassword();
        Driver.Navigate().Back();
    }

    // --- Register screen ---

    [Test, Order(4)]
    public void RegisterEmailEntry_AcceptsTypedText()
    {
        _shell.NavigateToProfile();
        _profile.TapCreateAccount();

        _register.TypeEmail("new@example.com");
        Assert.That(_register.GetEmailText(), Is.EqualTo("new@example.com"),
            "RegisterEmailEntry should contain the typed text");

        _register.ClearEmail();
    }

    [Test, Order(5)]
    public void RegisterPhoneEntry_AcceptsTypedText()
    {
        _register.TypePhone("123456789");
        Assert.That(_register.GetPhoneText(), Is.EqualTo("123456789"),
            "RegisterPhoneEntry should contain the typed text");

        _register.ClearPhone();
    }

    [Test, Order(6)]
    public void RegisterPasswordEntry_AcceptsTypedText()
    {
        _register.TypePassword("StrongPass1!");
        Assert.That(_register.GetPasswordText(), Is.EqualTo("StrongPass1!"),
            "RegisterPasswordEntry should contain the typed text");

        _register.ClearPassword();
    }

    [Test, Order(7)]
    public void RegisterConfirmPasswordEntry_AcceptsTypedText()
    {
        _register.TypeConfirmPassword("StrongPass1!");
        Assert.That(_register.GetConfirmPasswordText(), Is.EqualTo("StrongPass1!"),
            "RegisterConfirmPasswordEntry should contain the typed text");

        _register.ClearConfirmPassword();
        Driver.Navigate().Back();
    }
}
