using SportowyHub.Models.Api;

namespace SportowyHub.Services.Auth;

public interface IAuthService
{
    Task<AuthResult<LoginResponse>> LoginAsync(string email, string password);
    Task<AuthResult<RegisterResponse>> RegisterAsync(string email, string password, string passwordConfirm, string? phone = null);
    Task<AuthResult<ResendVerificationResponse>> ResendVerificationAsync(string email);
    Task<AuthResult<LoginResponse>> RefreshTokenAsync();
    Task<string?> GetTokenAsync();
    Task<UserInfo?> GetCurrentUserAsync();
    Task<bool> IsLoggedInAsync();
    Task ClearAuthAsync();
    Task LogoutAsync();
}
