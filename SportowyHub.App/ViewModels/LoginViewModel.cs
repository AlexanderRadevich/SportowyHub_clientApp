using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Resources.Strings;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.Toast;

namespace SportowyHub.ViewModels;

public partial class LoginViewModel(
    IAuthService authService,
    INavigationService nav,
    IToastService toastService) : ObservableObject
{
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

    [ObservableProperty]
    public partial bool IsGoogleLoading { get; set; }

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
    private async Task Login(CancellationToken ct)
    {
        LoginError = string.Empty;
        EmailError = string.Empty;
        IsLoading = true;

        try
        {
            var result = await authService.LoginAsync(Email, Password, ct);

            if (result.IsSuccess)
            {
                await nav.GoToAsync("..");
                await nav.GoToAsync("//home");
                return;
            }

            if (result.ErrorCode == "EMAIL_NOT_VERIFIED")
            {
                await nav.GoToAsync($"email-verification?email={Uri.EscapeDataString(Email)}");
                return;
            }

            if (result.FieldErrors?.TryGetValue("email", out var emailErr) == true)
            {
                EmailError = emailErr;
            }

            LoginError = result.ErrorMessage ?? "Login failed.";
            await toastService.ShowError(LoginError);
        }
        catch (Exception ex)
        {
            LoginError = "Connection error. Please try again.";
            await toastService.ShowError(ex.Message);
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
    private async Task OAuthLoginWithGoogle(CancellationToken ct)
    {
        IsGoogleLoading = true;
        LoginError = string.Empty;

        try
        {
            var authUrl = new Uri(
                "https://accounts.google.com/o/oauth2/v2/auth" +
                $"?client_id={Uri.EscapeDataString(ApiConfig.GoogleClientId)}" +
                $"&redirect_uri={Uri.EscapeDataString($"{ApiConfig.OAuthCallbackScheme}:/")}" +
                "&response_type=id_token" +
                "&scope=openid%20email%20profile" +
                $"&nonce={Guid.NewGuid():N}");

            var callbackUrl = new Uri($"{ApiConfig.OAuthCallbackScheme}:/");

            var authResult = await WebAuthenticator.Default.AuthenticateAsync(authUrl, callbackUrl);

            var idToken = authResult.IdToken
                          ?? (authResult.Properties.TryGetValue("id_token", out var token) ? token : null);

            if (string.IsNullOrEmpty(idToken))
            {
                await toastService.ShowError(AppResources.OAuthErrorFailed);
                return;
            }

            var result = await authService.OAuthLoginAsync("google", idToken, null, ct);

            if (result.IsSuccess)
            {
                await nav.GoToAsync("..");
                await nav.GoToAsync("//home");
                return;
            }

            LoginError = result.ErrorMessage ?? AppResources.OAuthErrorFailed;
            await toastService.ShowError(LoginError);
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            await toastService.ShowError(ex.Message);
        }
        finally
        {
            IsGoogleLoading = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToRegister()
    {
        await nav.GoToAsync("../register");
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();
}
