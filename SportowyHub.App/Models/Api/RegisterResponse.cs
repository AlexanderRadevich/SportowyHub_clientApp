namespace SportowyHub.Models.Api;

public record RegisterResponse(int Id, string Email, string TrustLevel, string? Locale = null);
