using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Resources.Strings;
using SportowyHub.Services.Auth;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.Toast;

namespace SportowyHub.ViewModels;

public partial class EmailVerificationViewModel(
    IAuthService authService,
    INavigationService nav,
    IToastService toastService) : ObservableObject, IQueryAttributable
{
    private IDispatcherTimer? _cooldownTimer;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Description))]
    public partial string Email { get; set; } = string.Empty;

    public string Description => string.Format(AppResources.EmailVerificationDescription, Email);

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ResendCommand))]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial string StatusMessage { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsStatusError { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ResendCommand))]
    public partial int CooldownSeconds { get; set; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("email", out var email) && email is string emailStr)
        {
            Email = emailStr;
        }
    }

    private bool CanResend() => !IsLoading && CooldownSeconds <= 0;

    [RelayCommand(CanExecute = nameof(CanResend))]
    private async Task Resend(CancellationToken ct)
    {
        StatusMessage = string.Empty;
        IsLoading = true;

        try
        {
            var result = await authService.ResendVerificationAsync(Email, ct);

            if (result.IsSuccess)
            {
                StatusMessage = AppResources.EmailVerificationResent;
                IsStatusError = false;
                StartCooldown();
            }
            else
            {
                StatusMessage = result.ErrorMessage ?? AppResources.EmailVerificationError;
                IsStatusError = true;
                await toastService.ShowError(StatusMessage);
            }
        }
        catch (Exception ex)
        {
            StatusMessage = AppResources.EmailVerificationError;
            IsStatusError = true;
            await toastService.ShowError(ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task BackToLogin()
    {
        await nav.GoToAsync("../../login");
    }

    private void StartCooldown()
    {
        CooldownSeconds = 60;
        _cooldownTimer?.Stop();

        _cooldownTimer = Application.Current!.Dispatcher.CreateTimer();
        _cooldownTimer.Interval = TimeSpan.FromSeconds(1);
        _cooldownTimer.Tick += (_, _) =>
        {
            CooldownSeconds--;
            if (CooldownSeconds <= 0)
            {
                _cooldownTimer.Stop();
            }
        };
        _cooldownTimer.Start();
    }
}
