namespace SportowyHub.Models.Api;

public record LoginResponse(
    string AccessToken,
    int ExpiresIn,
    string TokenType,
    string? RefreshToken = null,
    string? Locale = null,
    LoginUser? User = null);

public record LoginUser(int Id, string Email, string TrustLevel);

public record UserInfo(int Id, string Email, string TrustLevel);
