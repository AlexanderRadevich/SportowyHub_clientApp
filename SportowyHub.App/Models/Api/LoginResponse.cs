namespace SportowyHub.Models.Api;

public record LoginResponse(
    string AccessToken,
    int ExpiresIn,
    string TokenType,
    string? RefreshToken = null,
    string? Locale = null);

public record UserInfo(int Id, string Email, string TrustLevel);
