namespace SportowyHub.Models.Api;

public record UserAccount(
    string? FirstName,
    string? LastName,
    string? FullName,
    string? AvatarUrl,
    string? AvatarThumbnailUrl,
    bool NotificationsEnabled,
    string? QuietHoursStart,
    string? QuietHoursEnd,
    string? Locale,
    int BalanceGrosze,
    string? BalanceUpdatedAt);
