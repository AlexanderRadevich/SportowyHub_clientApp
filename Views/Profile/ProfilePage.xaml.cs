namespace SportowyHub.Views.Profile;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
    }

    private async void OnCreateAccountClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("register");
    }

    private async void OnSignInClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("login");
    }
}