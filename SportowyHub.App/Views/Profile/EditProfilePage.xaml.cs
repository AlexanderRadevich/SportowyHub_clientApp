using SportowyHub.ViewModels;

namespace SportowyHub.Views.Profile;

public partial class EditProfilePage : ContentPage
{
    public EditProfilePage(EditProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
