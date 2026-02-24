using SportowyHub.ViewModels;

namespace SportowyHub.Views.Profile;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(ProfileViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
