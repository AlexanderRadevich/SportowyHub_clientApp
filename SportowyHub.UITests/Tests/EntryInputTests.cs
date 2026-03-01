using SportowyHub.UITests.Helpers;
using SportowyHub.UITests.Pages;

namespace SportowyHub.UITests.Tests;

[Collection(AppiumDriverCollection.Name)]
public class EntryInputTests(AppiumDriverFixture fixture)
{
    private readonly AppShellPage _shell = new(fixture.Driver);
    private readonly SearchPage _search = new(fixture.Driver);
    private readonly ProfilePage _profile = new(fixture.Driver);
    private readonly LoginPage _login = new(fixture.Driver);
    private readonly RegisterPage _register = new(fixture.Driver);

    [Fact, TestPriority(1)]
    public void SearchEntry_AcceptsTypedText()
    {
        _shell.NavigateToSearch();

        _search.TypeSearch("running shoes");
        Assert.Equal("running shoes", _search.GetSearchText());

        _search.ClearSearch();
        _shell.NavigateToHome();
    }

    [Fact, TestPriority(2)]
    public void LoginEmailEntry_AcceptsTypedText()
    {
        _shell.NavigateToProfile();
        _profile.TapSignIn();

        _login.TypeEmail("test@example.com");
        Assert.Equal("test@example.com", _login.GetEmailText());

        _login.ClearEmail();
    }

    [Fact, TestPriority(3)]
    public void LoginPasswordEntry_AcceptsTypedText()
    {
        try
        {
            _login.TypePassword("Password123!");
            var passwordText = _login.GetPasswordText();
            Assert.Equal("Password123!".Length, passwordText.Length);

            _login.ClearPassword();
        }
        finally
        {
            try
            {
                if (fixture.Driver.IsKeyboardShown())
                {
                    fixture.Driver.HideKeyboard();
                }
            }
            catch { }
            fixture.Driver.Navigate().Back();
        }
    }

    [Fact, TestPriority(4)]
    public void RegisterEmailEntry_AcceptsTypedText()
    {
        _shell.NavigateToProfile();
        _profile.TapCreateAccount();

        _register.TypeEmail("new@example.com");
        Assert.Equal("new@example.com", _register.GetEmailText());

        _register.ClearEmail();
    }

    [Fact, TestPriority(5)]
    public void RegisterPhoneEntry_AcceptsTypedText()
    {
        _register.TypePhone("123456789");
        Assert.Equal("123456789", _register.GetPhoneText());

        _register.ClearPhone();
    }

    [Fact, TestPriority(6)]
    public void RegisterPasswordEntry_AcceptsTypedText()
    {
        _register.TypePassword("StrongPass1!");
        var passwordText = _register.GetPasswordText();
        Assert.Equal("StrongPass1!".Length, passwordText.Length);

        _register.ClearPassword();
    }

    [Fact, TestPriority(7)]
    public void RegisterConfirmPasswordEntry_AcceptsTypedText()
    {
        try
        {
            try { if (fixture.Driver.IsKeyboardShown()) { fixture.Driver.HideKeyboard(); } } catch { }

            _register.TypeConfirmPassword("StrongPass1!");
            var confirmText = _register.GetConfirmPasswordText();
            Assert.Equal("StrongPass1!".Length, confirmText.Length);

            _register.ClearConfirmPassword();
        }
        finally
        {
            try
            {
                if (fixture.Driver.IsKeyboardShown())
                {
                    fixture.Driver.HideKeyboard();
                }
            }
            catch { }
            fixture.Driver.Navigate().Back();
        }
    }
}
