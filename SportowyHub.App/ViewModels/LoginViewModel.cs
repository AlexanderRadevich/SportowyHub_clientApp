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
    private string _email = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _isPasswordVisible;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private bool _isLoading;

    [ObservableProperty]
    private string _loginError = string.Empty;

    [ObservableProperty]
    private string _emailError = string.Empty;

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

        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        EmailError = Regex.IsMatch(Email, emailPattern)
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
        IsLoading = true;

        try
        {
            var result = await _authService.LoginAsync(Email, Password);

            if (result.IsSuccess)
            {
                await Shell.Current.GoToAsync("//");
            }
            else
            {
                LoginError = result.ErrorMessage ?? "Login failed.";
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void TogglePasswordVisibility()
    {
        IsPasswordVisible = !IsPasswordVisible;
    }

    [RelayCommand]
    private async Task NavigateToRegister()
    {
        await Shell.Current.GoToAsync("//register");
    }
}
