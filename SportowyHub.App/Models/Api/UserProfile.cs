namespace SportowyHub.Models.Api;

using System.Text.Json.Serialization;
using SportowyHub.Services.Api;

public record UserProfile(
    int Id,
    string Email,
    bool EmailVerified,
    string? EmailVerifiedAt,
    string? Phone,
    bool PhoneVerified,
    string? PhoneVerifiedAt,
    string TrustLevel,
    [property: JsonConverter(typeof(FlexibleDecimalConverter))] decimal ReputationScore,
    OauthLinked? OauthLinked,
    string? LastLoginAt,
    string? LastActivityAt,
    UserAccount? Account);
