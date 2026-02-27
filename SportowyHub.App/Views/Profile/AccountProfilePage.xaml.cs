using SportowyHub.ViewModels;

namespace SportowyHub.Views.Profile;

public partial class AccountProfilePage : ContentPage
{
    private readonly AccountProfileViewModel _viewModel;

    public AccountProfilePage(AccountProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadProfileCommand.Execute(null);
    }
}
