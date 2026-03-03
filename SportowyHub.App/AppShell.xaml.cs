using SportowyHub.Views.Auth;
using SportowyHub.Views.MyListings;

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
        Routing.RegisterRoute("listing-detail", typeof(Views.Listings.ListingDetailPage));
        Routing.RegisterRoute("my-listings", typeof(MyListingsPage));
        Routing.RegisterRoute("create-edit-listing", typeof(CreateEditListingPage));
    }
}