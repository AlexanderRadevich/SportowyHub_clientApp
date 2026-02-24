namespace SportowyHub.Models.Api;

public record LoginResponse(string Token, UserInfo User);

public record UserInfo(int Id, string Email, string TrustLevel);
