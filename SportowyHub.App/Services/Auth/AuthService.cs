using System.Text.Json;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Exceptions;

namespace SportowyHub.Services.Auth;

public class AuthService(IRequestProvider requestProvider) : IAuthService
{
    private const string TokenKey = "auth_token";
    private const string UserKey = "auth_user";
    private const string RefreshTokenKey = "auth_refresh_token";
    private const string TokenExpiryKey = "auth_token_expiry";

    private static readonly Dictionary<string, string> _refreshTokenHeader = new()
    {
        ["X-Include-Refresh-Token"] = "true"
    };

    public async Task<AuthResult<LoginResponse>> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        try
        {
            var response = await requestProvider.PostAsync<LoginRequest, LoginResponse>(
                "/api/v1/login",
                new LoginRequest(email, password),
                headers: _refreshTokenHeader,
                ct: ct);

            await StoreTokens(response);

            return AuthResult<LoginResponse>.Success(response);
        }
        catch (ServiceAuthenticationException ex)
        {
            var (errorMessage, fieldErrors, errorCode) = ApiErrorParser.Parse(ex.Content, "An unexpected error occurred. Please try again.");
            return AuthResult<LoginResponse>.Failure(errorMessage, fieldErrors, errorCode);
        }
        catch (HttpRequestException ex)
        {
            var (errorMessage, fieldErrors, errorCode) = ApiErrorParser.Parse(ex.Message, "An unexpected error occurred. Please try again.");
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
        {
            await SecureStorage.SetAsync(RefreshTokenKey, response.RefreshToken);
        }

        var expiry = DateTimeOffset.UtcNow.AddSeconds(response.ExpiresIn).ToString("O");
        await SecureStorage.SetAsync(TokenExpiryKey, expiry);
    }

    public async Task<AuthResult<RegisterResponse>> RegisterAsync(string email, string password, string passwordConfirm, string? phone = null, CancellationToken ct = default)
    {
        try
        {
            var response = await requestProvider.PostAsync<RegisterRequest, RegisterResponse>(
                "/api/v1/register",
                new RegisterRequest(email, password, passwordConfirm, phone),
                ct: ct);

            return AuthResult<RegisterResponse>.Success(response);
        }
        catch (HttpRequestException ex)
        {
            var (errorMessage, fieldErrors, errorCode) = ApiErrorParser.Parse(ex.Message, "An unexpected error occurred. Please try again.");
            return AuthResult<RegisterResponse>.Failure(errorMessage, fieldErrors, errorCode);
        }
        catch (Exception)
        {
            return AuthResult<RegisterResponse>.Failure("Connection error. Please check your internet connection and try again.");
        }
    }

    public async Task<AuthResult<ResendVerificationResponse>> ResendVerificationAsync(string email, CancellationToken ct = default)
    {
        try
        {
            var response = await requestProvider.PostAsync<ResendVerificationRequest, ResendVerificationResponse>(
                "/api/v1/resend-verification",
                new ResendVerificationRequest(email),
                ct: ct);

            return AuthResult<ResendVerificationResponse>.Success(response);
        }
        catch (HttpRequestException ex)
        {
            var (errorMessage, _, errorCode) = ApiErrorParser.Parse(ex.Message, "An unexpected error occurred. Please try again.");
            return AuthResult<ResendVerificationResponse>.Failure(errorMessage, errorCode: errorCode);
        }
        catch (Exception)
        {
            return AuthResult<ResendVerificationResponse>.Failure("Connection error. Please check your internet connection and try again.");
        }
    }

    public async Task<AuthResult<LoginResponse>> RefreshTokenAsync(CancellationToken ct = default)
    {
        var refreshToken = await SecureStorage.GetAsync(RefreshTokenKey);
        if (string.IsNullOrEmpty(refreshToken))
        {
            return AuthResult<LoginResponse>.Failure("No refresh token available.");
        }

        try
        {
            var response = await requestProvider.PostAsync<Dictionary<string, string>, LoginResponse>(
                "/api/v1/refresh",
                new Dictionary<string, string>(),
                token: refreshToken,
                headers: _refreshTokenHeader,
                ct: ct);

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
        {
            return null;
        }

        return JsonSerializer.Deserialize(userJson, SportowyHubJsonContext.Default.UserInfo);
    }

    public async Task<bool> IsLoggedInAsync()
    {
        var token = await SecureStorage.GetAsync(TokenKey);
        return !string.IsNullOrEmpty(token);
    }

    public async Task LogoutAsync()
    {
        await SecureStorage.GetAsync(RefreshTokenKey);
        await ClearAuthAsync();
    }

    public async Task<UserProfile?> GetProfileAsync(CancellationToken ct = default)
    {
        var token = await SecureStorage.GetAsync(TokenKey);
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        return await requestProvider.GetAsync<UserProfile>("/api/private/profile", token, ct);
    }

    public async Task<UserProfile?> UpdateProfileAsync(UpdateProfileRequest request, CancellationToken ct = default)
    {
        var token = await SecureStorage.GetAsync(TokenKey);
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        return await requestProvider.PutAsync<UpdateProfileRequest, UserProfile>("/api/private/profile", request, token, ct);
    }

    public Task ClearAuthAsync()
    {
        SecureStorage.Remove(TokenKey);
        SecureStorage.Remove(UserKey);
        SecureStorage.Remove(RefreshTokenKey);
        SecureStorage.Remove(TokenExpiryKey);
        return Task.CompletedTask;
    }
}
