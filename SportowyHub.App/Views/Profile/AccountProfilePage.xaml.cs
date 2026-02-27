using SportowyHub.ViewModels;

namespace SportowyHub.Views.Profile;

public partial class AccountProfilePage : ContentPage
{
    public AccountProfilePage(AccountProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
