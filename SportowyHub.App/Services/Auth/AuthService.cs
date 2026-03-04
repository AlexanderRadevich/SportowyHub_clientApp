using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Exceptions;

namespace SportowyHub.Services.Auth;

public class AuthService(IRequestProvider requestProvider, IHttpClientFactory httpClientFactory) : IAuthService
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

        if (response.User is not null)
        {
            var userInfo = new UserInfo(response.User.Id, response.User.Email, response.User.TrustLevel);
            var userJson = JsonSerializer.Serialize(userInfo, SportowyHubJsonContext.Default.UserInfo);
            await SecureStorage.SetAsync(UserKey, userJson);
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
        try
        {
            var token = await SecureStorage.GetAsync(TokenKey);
            if (!string.IsNullOrEmpty(token))
            {
                await requestProvider.PostAsync("/api/v1/logout", token);
            }
        }
        catch
        {
        }
        finally
        {
            await ClearAuthAsync();
        }
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

    public async Task<UpdateProfileResponse?> UpdateProfileAsync(UpdateProfileRequest request, CancellationToken ct = default)
    {
        var token = await SecureStorage.GetAsync(TokenKey);
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        return await requestProvider.PatchAsync<UpdateProfileRequest, UpdateProfileResponse>("/api/private/profile", request, token, ct);
    }

    public async Task<AuthResult<LoginResponse>> OAuthLoginAsync(string provider, string? idToken, string? accessToken, CancellationToken ct = default)
    {
        try
        {
            var response = await requestProvider.PostAsync<OAuthLoginRequest, LoginResponse>(
                $"/api/v1/auth/oauth/{Uri.EscapeDataString(provider)}",
                new OAuthLoginRequest(idToken, accessToken),
                headers: _refreshTokenHeader,
                ct: ct);

            await StoreTokens(response);

            return AuthResult<LoginResponse>.Success(response);
        }
        catch (ServiceAuthenticationException ex)
        {
            var (errorMessage, fieldErrors, errorCode) = ApiErrorParser.Parse(ex.Content, "OAuth login failed. Please try again.");
            return AuthResult<LoginResponse>.Failure(errorMessage, fieldErrors, errorCode);
        }
        catch (HttpRequestException ex)
        {
            var (errorMessage, fieldErrors, errorCode) = ApiErrorParser.Parse(ex.Message, "OAuth login failed. Please try again.");
            return AuthResult<LoginResponse>.Failure(errorMessage, fieldErrors, errorCode);
        }
        catch (Exception)
        {
            return AuthResult<LoginResponse>.Failure("Connection error. Please check your internet connection and try again.");
        }
    }

    public async Task<AvatarResponse?> UploadAvatarAsync(Stream imageStream, string fileName, CancellationToken ct = default)
    {
        var token = await SecureStorage.GetAsync(TokenKey);
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        using var content = new MultipartFormDataContent();
        using var streamContent = new StreamContent(imageStream);
        content.Add(streamContent, "file", fileName);

        return await requestProvider.PostMultipartAsync<AvatarResponse>("/api/private/profile/avatar", content, token, ct);
    }

    public async Task DeleteAvatarAsync(CancellationToken ct = default)
    {
        var token = await SecureStorage.GetAsync(TokenKey);
        if (string.IsNullOrEmpty(token))
        {
            return;
        }

        await requestProvider.DeleteAsync("/api/private/profile/avatar", token, ct);
    }

    public Task ClearAuthAsync()
    {
        SecureStorage.Remove(TokenKey);
        SecureStorage.Remove(UserKey);
        SecureStorage.Remove(RefreshTokenKey);
        SecureStorage.Remove(TokenExpiryKey);
        return Task.CompletedTask;
    }

    public async Task<AuthResult<LoginResponse>> GoogleSignInAsync(CancellationToken ct = default)
    {
        var idToken = await AcquireGoogleIdTokenAsync(ct);
        if (string.IsNullOrEmpty(idToken))
        {
            return AuthResult<LoginResponse>.Failure("Google sign-in failed.");
        }

        return await OAuthLoginAsync("google", idToken, null, ct);
    }

    public async Task<string?> AcquireGoogleIdTokenAsync(CancellationToken ct = default)
    {
        var codeVerifier = GeneratePkceCodeVerifier();
        var codeChallenge = GeneratePkceCodeChallenge(codeVerifier);
        var redirectUri = ApiConfig.OAuthRedirectUri;

        var authUrl = new Uri(
            $"{ApiConfig.GoogleAuthUrl}" +
            $"?client_id={Uri.EscapeDataString(ApiConfig.GoogleClientId)}" +
            $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
            "&response_type=code" +
            "&scope=openid%20email%20profile" +
            $"&code_challenge={codeChallenge}" +
            "&code_challenge_method=S256");

        var authResult = await WebAuthenticator.Default.AuthenticateAsync(authUrl, new Uri(redirectUri));

        authResult.Properties.TryGetValue("code", out var code);
        if (string.IsNullOrEmpty(code))
        {
            return null;
        }

        return await ExchangeCodeForIdTokenAsync(code, codeVerifier, redirectUri, ct);
    }

    private async Task<string?> ExchangeCodeForIdTokenAsync(string code, string codeVerifier, string redirectUri, CancellationToken ct)
    {
        using var client = httpClientFactory.CreateClient();
        using var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["code"] = code,
            ["client_id"] = ApiConfig.GoogleClientId,
            ["redirect_uri"] = redirectUri,
            ["grant_type"] = "authorization_code",
            ["code_verifier"] = codeVerifier
        });

        var response = await client.PostAsync(ApiConfig.GoogleTokenUrl, content, ct);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        using var json = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync(ct), cancellationToken: ct);
        return json.RootElement.TryGetProperty("id_token", out var tokenElement)
            ? tokenElement.GetString()
            : null;
    }

    private static string GeneratePkceCodeVerifier()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Base64UrlEncode(bytes);
    }

    private static string GeneratePkceCodeChallenge(string codeVerifier)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier));
        return Base64UrlEncode(hash);
    }

    private static string Base64UrlEncode(byte[] bytes)
    {
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}
