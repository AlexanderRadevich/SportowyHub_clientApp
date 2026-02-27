using System.Text.Json;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Exceptions;

namespace SportowyHub.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IRequestProvider _requestProvider;

    private const string TokenKey = "auth_token";
    private const string UserKey = "auth_user";
    private const string RefreshTokenKey = "auth_refresh_token";
    private const string TokenExpiryKey = "auth_token_expiry";

    private static readonly Dictionary<string, string> RefreshTokenHeader = new()
    {
        ["X-Include-Refresh-Token"] = "true"
    };

    public AuthService(IRequestProvider requestProvider)
    {
        _requestProvider = requestProvider;
    }

    public async Task<AuthResult<LoginResponse>> LoginAsync(string email, string password)
    {
        try
        {
            var response = await _requestProvider.PostAsync<LoginRequest, LoginResponse>(
                "/api/v1/login",
                new LoginRequest(email, password),
                headers: RefreshTokenHeader);

            await StoreTokens(response);

            return AuthResult<LoginResponse>.Success(response);
        }
        catch (ServiceAuthenticationException ex)
        {
            var (errorMessage, fieldErrors, errorCode) = ParseErrorWithFields(ex.Content);
            return AuthResult<LoginResponse>.Failure(errorMessage, fieldErrors, errorCode);
        }
        catch (HttpRequestException ex)
        {
            var (errorMessage, fieldErrors, errorCode) = ParseErrorWithFields(ex.Message);
            return AuthResult<LoginResponse>.Failure(errorMessage, fieldErrors, errorCode);
        }
        catch (Exception)
        {
            return AuthResult<LoginResponse>.Failure("Connection error. Please check your internet connection and try again.");
        }
    }

    private async Task StoreTokens(LoginResponse response)
    {
        await SecureStorage.SetAsync(TokenKey, response.AccessToken);

        if (!string.IsNullOrEmpty(response.RefreshToken))
            await SecureStorage.SetAsync(RefreshTokenKey, response.RefreshToken);

        var expiry = DateTimeOffset.UtcNow.AddSeconds(response.ExpiresIn).ToString("O");
        await SecureStorage.SetAsync(TokenExpiryKey, expiry);
    }

    public async Task<AuthResult<RegisterResponse>> RegisterAsync(string email, string password, string passwordConfirm, string? phone = null)
    {
        try
        {
            var response = await _requestProvider.PostAsync<RegisterRequest, RegisterResponse>(
                "/api/v1/register",
                new RegisterRequest(email, password, passwordConfirm, phone));

            return AuthResult<RegisterResponse>.Success(response);
        }
        catch (HttpRequestException ex)
        {
            var (errorMessage, fieldErrors, errorCode) = ParseErrorWithFields(ex.Message);
            return AuthResult<RegisterResponse>.Failure(errorMessage, fieldErrors, errorCode);
        }
        catch (Exception)
        {
            return AuthResult<RegisterResponse>.Failure("Connection error. Please check your internet connection and try again.");
        }
    }

    public async Task<AuthResult<ResendVerificationResponse>> ResendVerificationAsync(string email)
    {
        try
        {
            var response = await _requestProvider.PostAsync<ResendVerificationRequest, ResendVerificationResponse>(
                "/api/v1/resend-verification",
                new ResendVerificationRequest(email));

            return AuthResult<ResendVerificationResponse>.Success(response);
        }
        catch (HttpRequestException ex)
        {
            var (errorMessage, _, errorCode) = ParseErrorWithFields(ex.Message);
            return AuthResult<ResendVerificationResponse>.Failure(errorMessage, errorCode: errorCode);
        }
        catch (Exception)
        {
            return AuthResult<ResendVerificationResponse>.Failure("Connection error. Please check your internet connection and try again.");
        }
    }

    public async Task<AuthResult<LoginResponse>> RefreshTokenAsync()
    {
        var refreshToken = await SecureStorage.GetAsync(RefreshTokenKey);
        if (string.IsNullOrEmpty(refreshToken))
            return AuthResult<LoginResponse>.Failure("No refresh token available.");

        try
        {
            var response = await _requestProvider.PostAsync<Dictionary<string, string>, LoginResponse>(
                "/api/v1/refresh",
                new Dictionary<string, string>(),
                token: refreshToken,
                headers: RefreshTokenHeader);

            await StoreTokens(response);

            return AuthResult<LoginResponse>.Success(response);
        }
        catch (Exception)
        {
            await ClearAuthAsync();
            return AuthResult<LoginResponse>.Failure("Session expired. Please log in again.");
        }
    }

    public async Task<string?> GetTokenAsync()
    {
        return await SecureStorage.GetAsync(TokenKey);
    }

    public async Task<UserInfo?> GetCurrentUserAsync()
    {
        var userJson = await SecureStorage.GetAsync(UserKey);
        if (string.IsNullOrEmpty(userJson))
            return null;

        return JsonSerializer.Deserialize(userJson, SportowyHubJsonContext.Default.UserInfo);
    }

    public async Task<bool> IsLoggedInAsync()
    {
        var token = await SecureStorage.GetAsync(TokenKey);
        return !string.IsNullOrEmpty(token);
    }

    public async Task LogoutAsync()
    {
        var refreshToken = await SecureStorage.GetAsync(RefreshTokenKey);
        if (!string.IsNullOrEmpty(refreshToken))
        {
            try
            {
                await _requestProvider.PostAsync<Dictionary<string, string>, object>(
                    "/api/v1/logout",
                    new Dictionary<string, string>(),
                    token: refreshToken);
            }
            catch
            {
                // Best-effort: server revocation failure must not block local sign-out
            }
        }

        await ClearAuthAsync();
    }

    public Task ClearAuthAsync()
    {
        SecureStorage.Remove(TokenKey);
        SecureStorage.Remove(UserKey);
        SecureStorage.Remove(RefreshTokenKey);
        SecureStorage.Remove(TokenExpiryKey);
        return Task.CompletedTask;
    }

    private static (string Message, Dictionary<string, string>? FieldErrors, string? ErrorCode) ParseErrorWithFields(string content)
    {
        try
        {
            var apiError = JsonSerializer.Deserialize(content, SportowyHubJsonContext.Default.ApiError);
            if (apiError?.Error != null)
            {
                return (apiError.Error.Message, apiError.Error.Violations, apiError.Error.Code);
            }
        }
        catch
        {
            // Ignore parse failures
        }

        return ("An unexpected error occurred. Please try again.", null, null);
    }
}
