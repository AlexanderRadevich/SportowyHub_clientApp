using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SportowyHub.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateAccountCommand))]
    private string _email = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateAccountCommand))]
    private string _password = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateAccountCommand))]
    private string _confirmPassword = string.Empty;

    [ObservableProperty]
    private string _emailError = string.Empty;

    [ObservableProperty]
    private string _passwordStrength = string.Empty;

    [ObservableProperty]
    private Color _passwordStrengthColor = Colors.Transparent;

    [ObservableProperty]
    private string _confirmPasswordError = string.Empty;

    [ObservableProperty]
    private bool _isPasswordVisible;

    [ObservableProperty]
    private bool _isConfirmPasswordVisible;

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

        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        EmailError = Regex.IsMatch(Email, emailPattern)
            ? string.Empty
            : "Please enter a valid email address";
    }

    private void EvaluatePasswordStrength()
    {
        if (string.IsNullOrEmpty(Password))
        {
            PasswordStrength = string.Empty;
            PasswordStrengthColor = Colors.Transparent;
            return;
        }

        bool hasLetters = Regex.IsMatch(Password, @"[a-zA-Z]");
        bool hasDigits = Regex.IsMatch(Password, @"\d");
        bool hasSpecial = Regex.IsMatch(Password, @"[^a-zA-Z\d]");

        if (Password.Length >= 8 && hasLetters && hasDigits && hasSpecial)
        {
            PasswordStrength = "Strong";
            PasswordStrengthColor = Color.FromArgb("#16A34A");
        }
        else if (Password.Length >= 6 && hasLetters && hasDigits)
        {
            PasswordStrength = "Medium";
            PasswordStrengthColor = Color.FromArgb("#F59E0B");
        }
        else
        {
            PasswordStrength = "Weak";
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
            : "Passwords do not match";
    }

    private bool CanCreateAccount()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            return false;

        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(Email, emailPattern))
            return false;

        if (Password.Length < 6)
            return false;

        if (Password != ConfirmPassword)
            return false;

        return true;
    }

    [RelayCommand(CanExecute = nameof(CanCreateAccount))]
    private async Task CreateAccount()
    {
        // UI-only MVP — no backend call
        await Shell.Current.GoToAsync("..");
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
}