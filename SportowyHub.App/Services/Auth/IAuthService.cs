using SportowyHub.Models.Api;

namespace SportowyHub.Services.Auth;

public interface IAuthService
{
    Task<AuthResult<LoginResponse>> LoginAsync(string email, string password, CancellationToken ct = default);
    Task<AuthResult<RegisterResponse>> RegisterAsync(string email, string password, string passwordConfirm, string? phone = null, CancellationToken ct = default);
    Task<AuthResult<ResendVerificationResponse>> ResendVerificationAsync(string email, CancellationToken ct = default);
    Task<AuthResult<LoginResponse>> RefreshTokenAsync(CancellationToken ct = default);
    Task<string?> GetTokenAsync();
    Task<UserInfo?> GetCurrentUserAsync();
    Task<bool> IsLoggedInAsync();
    Task ClearAuthAsync();
    Task LogoutAsync();
    Task<UserProfile?> GetProfileAsync(CancellationToken ct = default);
    Task<UserProfile?> UpdateProfileAsync(UpdateProfileRequest request, CancellationToken ct = default);
}
