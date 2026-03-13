using SportowyHub.Models;
using SportowyHub.Models.Api;

namespace SportowyHub.Services.Auth;

public interface IAuthService
{
    Task<Result<LoginResponse>> LoginAsync(string email, string password, CancellationToken ct = default);
    Task<Result<RegisterResponse>> RegisterAsync(string email, string password, string passwordConfirm, string? phone = null, CancellationToken ct = default);
    Task<Result<LoginResponse>> OAuthLoginAsync(string provider, string? idToken, string? accessToken, CancellationToken ct = default);
    Task<string?> AcquireGoogleIdTokenAsync(CancellationToken ct = default);
    Task<Result<LoginResponse>> GoogleSignInAsync(CancellationToken ct = default);
    Task<Result<ResendVerificationResponse>> ResendVerificationAsync(string email, CancellationToken ct = default);
    Task LogoutAsync();
}
