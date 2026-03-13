using SportowyHub.Models;
using SportowyHub.Models.Api;

namespace SportowyHub.Services.Auth;

public interface ITokenProvider
{
    Task<string?> GetTokenAsync();
    Task<DateTimeOffset?> GetTokenExpiryAsync();
    Task<Result<LoginResponse>> RefreshTokenAsync(CancellationToken ct = default);
    Task<UserInfo?> GetCurrentUserAsync();
    Task<bool> IsLoggedInAsync();
    Task ClearAuthAsync();
}
