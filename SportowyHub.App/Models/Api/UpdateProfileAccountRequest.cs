namespace SportowyHub.Models.Api;

public record UpdateProfileAccountRequest(
    string? FirstName,
    string? LastName,
    bool NotificationsEnabled,
    string? QuietHoursStart,
    string? QuietHoursEnd);
