using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Resources.Strings;
using SportowyHub.Services.Auth;

namespace SportowyHub.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly IAuthService _authService;

    public RegisterViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateAccountCommand))]
    public partial string Email { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateAccountCommand))]
    public partial string Password { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateAccountCommand))]
    public partial string ConfirmPassword { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateAccountCommand))]
    public partial string Phone { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string EmailError { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string PhoneError { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string PasswordStrength { get; set; } = string.Empty;

    [ObservableProperty]
    public partial Color PasswordStrengthColor { get; set; }

    [ObservableProperty]
    public partial string ConfirmPasswordError { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsPasswordVisible { get; set; }

    [ObservableProperty]
    public partial bool IsConfirmPasswordVisible { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateAccountCommand))]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial string RegisterError { get; set; } = string.Empty;

    partial void OnEmailChanged(string value)
    {
        ValidateEmail();
    }

    partial void OnPasswordChanged(string value)
    {
        EvaluatePasswordStrength();
        ValidateConfirmPassword();
    }

    partial void OnConfirmPasswordChanged(string value)
    {
        ValidateConfirmPassword();
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

    private void EvaluatePasswordStrength()
    {
        if (string.IsNullOrEmpty(Password))
        {
            PasswordStrength = string.Empty;
            PasswordStrengthColor = Colors.Transparent;
            return;
        }

        bool hasLetters = HasLettersRegex().IsMatch(Password);
        bool hasDigits = HasDigitsRegex().IsMatch(Password);
        bool hasSpecial = HasSpecialRegex().IsMatch(Password);

        if (Password.Length >= 10 && hasLetters && hasDigits && hasSpecial)
        {
            PasswordStrength = AppResources.PasswordStrengthStrong;
            PasswordStrengthColor = Color.FromArgb("#16A34A");
        }
        else if (Password.Length >= 8 && hasLetters && hasDigits)
        {
            PasswordStrength = AppResources.PasswordStrengthMedium;
            PasswordStrengthColor = Color.FromArgb("#F59E0B");
        }
        else
        {
            PasswordStrength = AppResources.PasswordStrengthWeak;
            PasswordStrengthColor = Color.FromArgb("#DC2626");
        }
    }

    private void ValidateConfirmPassword()
    {
        if (string.IsNullOrEmpty(ConfirmPassword))
        {
            ConfirmPasswordError = string.Empty;
            return;
        }

        ConfirmPasswordError = ConfirmPassword == Password
            ? string.Empty
            : AppResources.AuthPasswordsDoNotMatch;
    }

    private bool CanCreateAccount()
    {
        if (IsLoading)
            return false;

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Phone) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            return false;

        if (!EmailRegex().IsMatch(Email))
            return false;

        if (Password.Length < 8)
            return false;

        if (Password != ConfirmPassword)
            return false;

        return true;
    }

    [RelayCommand(CanExecute = nameof(CanCreateAccount))]
    private async Task CreateAccount()
    {
        RegisterError = string.Empty;
        EmailError = string.Empty;
        PhoneError = string.Empty;
        IsLoading = true;

        try
        {
            var registerResult = await _authService.RegisterAsync(Email, Password, ConfirmPassword, Phone);

            if (!registerResult.IsSuccess)
            {
                if (registerResult.FieldErrors?.TryGetValue("email", out var emailErr) == true)
                    EmailError = emailErr;
                if (registerResult.FieldErrors?.TryGetValue("phone", out var phoneErr) == true)
                    PhoneError = phoneErr;

                RegisterError = registerResult.ErrorMessage ?? "Registration failed.";
                IsLoading = false;
                return;
            }

            if (registerResult.Data!.TrustLevel == "TL0")
            {
                await Shell.Current.DisplayAlertAsync(
                    AppResources.AuthRegistrationSuccess,
                    AppResources.AuthRegistrationSuccessMessage,
                    "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.GoToAsync($"email-verification?email={Uri.EscapeDataString(Email)}");
            }
        }
        catch
        {
            RegisterError = "Connection error. Please try again.";
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void TogglePasswordVisibility()
    {
        IsPasswordVisible = !IsPasswordVisible;
    }

    [RelayCommand]
    private void ToggleConfirmPasswordVisibility()
    {
        IsConfirmPasswordVisible = !IsConfirmPasswordVisible;
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();

    [GeneratedRegex(@"[a-zA-Z]")]
    private static partial Regex HasLettersRegex();

    [GeneratedRegex(@"\d")]
    private static partial Regex HasDigitsRegex();

    [GeneratedRegex(@"[^a-zA-Z\d]")]
    private static partial Regex HasSpecialRegex();
}
