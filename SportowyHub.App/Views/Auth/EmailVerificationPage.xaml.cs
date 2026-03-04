using SportowyHub.ViewModels;

namespace SportowyHub.Views.Auth;

public partial class EmailVerificationPage : ContentPage
{
    private readonly EmailVerificationViewModel _viewModel;

    public EmailVerificationPage(EmailVerificationViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.StopCooldownTimer();
    }
}
