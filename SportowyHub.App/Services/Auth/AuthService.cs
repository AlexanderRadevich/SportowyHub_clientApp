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
                new LoginRequest(email, password));

            await SecureStorage.SetAsync(TokenKey, response.Token);
            var userJson = JsonSerializer.Serialize(response.User, SportowyHubJsonContext.Default.UserInfo);
            await SecureStorage.SetAsync(UserKey, userJson);

            return AuthResult<LoginResponse>.Success(response);
        }
        catch (ServiceAuthenticationException ex)
        {
            var errorMessage = ParseErrorMessage(ex.Content);
            return AuthResult<LoginResponse>.Failure(errorMessage);
        }
        catch (HttpRequestException ex)
        {
            var errorMessage = ParseErrorMessage(ex.Message);
            return AuthResult<LoginResponse>.Failure(errorMessage);
        }
        catch (Exception)
        {
            return AuthResult<LoginResponse>.Failure("Connection error. Please check your internet connection and try again.");
        }
    }

    public async Task<AuthResult<RegisterResponse>> RegisterAsync(string email, string password)
    {
        try
        {
            var response = await _requestProvider.PostAsync<RegisterRequest, RegisterResponse>(
                "/api/v1/register",
                new RegisterRequest(email, password));

            return AuthResult<RegisterResponse>.Success(response);
        }
        catch (HttpRequestException ex)
        {
            var (errorMessage, fieldErrors) = ParseErrorWithFields(ex.Message);
            return AuthResult<RegisterResponse>.Failure(errorMessage, fieldErrors);
        }
        catch (Exception)
        {
            return AuthResult<RegisterResponse>.Failure("Connection error. Please check your internet connection and try again.");
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

    public Task ClearAuthAsync()
    {
        SecureStorage.Remove(TokenKey);
        SecureStorage.Remove(UserKey);
        return Task.CompletedTask;
    }

    private static string ParseErrorMessage(string content)
    {
        try
        {
            var apiError = JsonSerializer.Deserialize(content, SportowyHubJsonContext.Default.ApiError);
            if (apiError?.Error?.Message is { } message)
                return message;
        }
        catch
        {
            // Ignore parse failures
        }

        return "An unexpected error occurred. Please try again.";
    }

    private static (string Message, Dictionary<string, string>? FieldErrors) ParseErrorWithFields(string content)
    {
        try
        {
            var apiError = JsonSerializer.Deserialize(content, SportowyHubJsonContext.Default.ApiError);
            if (apiError?.Error != null)
            {
                return (apiError.Error.Message, null);
            }
        }
        catch
        {
            // Ignore parse failures
        }

        return ("An unexpected error occurred. Please try again.", null);
    }
}
