using System.Text.Json;
using System.Text.Json.Serialization;
using SportowyHub.Models.Api;

namespace SportowyHub.Services.Api;

[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(LoginRequest))]
[JsonSerializable(typeof(LoginResponse))]
[JsonSerializable(typeof(RegisterRequest))]
[JsonSerializable(typeof(RegisterResponse))]
[JsonSerializable(typeof(ApiError))]
[JsonSerializable(typeof(UserInfo))]
[JsonSerializable(typeof(ResendVerificationRequest))]
[JsonSerializable(typeof(ResendVerificationResponse))]
[JsonSerializable(typeof(UserProfile))]
[JsonSerializable(typeof(UserAccount))]
[JsonSerializable(typeof(OauthLinked))]
[JsonSerializable(typeof(UpdateProfileRequest))]
[JsonSerializable(typeof(UpdateProfileAccountRequest))]
[JsonSerializable(typeof(ListingSummary))]
[JsonSerializable(typeof(ListingsResponse))]
[JsonSerializable(typeof(ListingDetail))]
[JsonSerializable(typeof(SearchResultItem))]
[JsonSerializable(typeof(GeoLocation))]
[JsonSerializable(typeof(SearchAttribute))]
[JsonSerializable(typeof(SearchResponse))]
[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSerializable(typeof(List<string>))]
public partial class SportowyHubJsonContext : JsonSerializerContext;
