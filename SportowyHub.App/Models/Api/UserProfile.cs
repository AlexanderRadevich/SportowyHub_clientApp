namespace SportowyHub.Models.Api;

public record UserProfile(
    int Id,
    string Email,
    string? FirstName,
    string? LastName,
    string? Locale,
    string? AvatarUrl,
    bool NotificationsEnabled,
    string? QuietHoursStart,
    string? QuietHoursEnd,
    string? Phone,
    bool PhoneVerified,
    bool EmailVerified,
    string TrustLevel);
