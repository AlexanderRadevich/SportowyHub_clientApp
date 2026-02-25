using SportowyHub.ViewModels;

namespace SportowyHub.Views.Auth;

public partial class EmailVerificationPage : ContentPage
{
    public EmailVerificationPage(EmailVerificationViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
