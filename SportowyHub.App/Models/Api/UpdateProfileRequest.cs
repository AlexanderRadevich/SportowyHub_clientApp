namespace SportowyHub.Models.Api;

public record UpdateProfileRequest(
    string? Phone,
    string? Locale,
    UpdateProfileAccountRequest? Account);
