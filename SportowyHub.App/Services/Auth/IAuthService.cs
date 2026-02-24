using SportowyHub.Models.Api;

namespace SportowyHub.Services.Auth;

public interface IAuthService
{
    Task<AuthResult<LoginResponse>> LoginAsync(string email, string password);
    Task<AuthResult<RegisterResponse>> RegisterAsync(string email, string password);
    Task<string?> GetTokenAsync();
    Task<UserInfo?> GetCurrentUserAsync();
    Task<bool> IsLoggedInAsync();
    Task ClearAuthAsync();
}
