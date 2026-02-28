using SportowyHub.Views.Auth;

namespace SportowyHub;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("register", typeof(RegisterPage));
        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("email-verification", typeof(EmailVerificationPage));
        Routing.RegisterRoute("account-profile", typeof(Views.Profile.AccountProfilePage));
        Routing.RegisterRoute("edit-profile", typeof(Views.Profile.EditProfilePage));
    }
}