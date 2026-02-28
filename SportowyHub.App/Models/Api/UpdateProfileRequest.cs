namespace SportowyHub.Models.Api;

public record UpdateProfileRequest(
    string? Phone,
    UpdateProfileAccountRequest? Account);
