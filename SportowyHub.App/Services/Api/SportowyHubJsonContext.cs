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
[JsonSerializable(typeof(Dictionary<string, string>))]
public partial class SportowyHubJsonContext : JsonSerializerContext;
