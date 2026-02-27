using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Resources.Strings;
using SportowyHub.Services.Auth;

namespace SportowyHub.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService _authService;

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    public partial string Email { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    public partial string Password { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsPasswordVisible { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial string LoginError { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string EmailError { get; set; } = string.Empty;

    partial void OnEmailChanged(string value)
    {
        ValidateEmail();
    }

    private void ValidateEmail()
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            EmailError = string.Empty;
            return;
        }

        EmailError = EmailRegex().IsMatch(Email)
            ? string.Empty
            : AppResources.AuthInvalidEmail;
    }

    private bool CanLogin()
    {
        return !IsLoading
            && !string.IsNullOrWhiteSpace(Email)
            && !string.IsNullOrWhiteSpace(Password);
    }

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task Login()
    {
        LoginError = string.Empty;
        EmailError = string.Empty;
        IsLoading = true;

        try
        {
            var result = await _authService.LoginAsync(Email, Password);

            if (result.IsSuccess)
            {
                await Shell.Current.GoToAsync("..");
                Shell.Current.CurrentItem = Shell.Current.Items[0];
                return;
            }

            if (result.ErrorCode == "EMAIL_NOT_VERIFIED")
            {
                await Shell.Current.GoToAsync($"email-verification?email={Uri.EscapeDataString(Email)}");
                return;
            }

            if (result.FieldErrors?.TryGetValue("email", out var emailErr) == true)
            {
                EmailError = emailErr;
            }

            LoginError = result.ErrorMessage ?? "Login failed.";
        }
        catch
        {
            LoginError = "Connection error. Please try again.";
        }

        IsLoading = false;
    }

    [RelayCommand]
    private void TogglePasswordVisibility()
    {
        IsPasswordVisible = !IsPasswordVisible;
    }

    [RelayCommand]
    private async Task NavigateToRegister()
    {
        await Shell.Current.GoToAsync("../register");
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();
}
