using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Resources.Strings;
using SportowyHub.Services.Auth;

namespace SportowyHub.ViewModels;

public partial class EmailVerificationViewModel : ObservableObject, IQueryAttributable
{
    private readonly IAuthService _authService;
    private IDispatcherTimer? _cooldownTimer;

    public EmailVerificationViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Description))]
    private string _email = string.Empty;

    public string Description => string.Format(AppResources.EmailVerificationDescription, Email);

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ResendCommand))]
    private bool _isLoading;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private bool _isStatusError;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ResendCommand))]
    private int _cooldownSeconds;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("email", out var email) && email is string emailStr)
            Email = emailStr;
    }

    private bool CanResend() => !IsLoading && CooldownSeconds <= 0;

    [RelayCommand(CanExecute = nameof(CanResend))]
    private async Task Resend()
    {
        StatusMessage = string.Empty;
        IsLoading = true;

        try
        {
            var result = await _authService.ResendVerificationAsync(Email);

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
            }
        }
        catch
        {
            StatusMessage = AppResources.EmailVerificationError;
            IsStatusError = true;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task BackToLogin()
    {
        await Shell.Current.GoToAsync("../../login");
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
                _cooldownTimer.Stop();
        };
        _cooldownTimer.Start();
    }
}
