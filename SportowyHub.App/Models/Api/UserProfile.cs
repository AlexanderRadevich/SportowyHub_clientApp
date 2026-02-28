namespace SportowyHub.Models.Api;

public record UserProfile(
    int Id,
    string Email,
    bool EmailVerified,
    string? EmailVerifiedAt,
    string? Phone,
    bool PhoneVerified,
    string? PhoneVerifiedAt,
    string TrustLevel,
    int ReputationScore,
    OauthLinked? OauthLinked,
    string? LastLoginAt,
    string? LastActivityAt,
    UserAccount? Account,
    string? Locale);
