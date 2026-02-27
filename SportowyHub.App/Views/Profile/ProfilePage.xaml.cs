using SportowyHub.ViewModels;

namespace SportowyHub.Views.Profile;

public partial class ProfilePage : ContentPage
{
    private readonly ProfileViewModel _viewModel;

    public ProfilePage(ProfileViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = viewModel;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.RefreshAuthStateCommand.Execute(null);
    }
}
