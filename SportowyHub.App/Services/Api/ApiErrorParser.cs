using System.Text.Json;

namespace SportowyHub.Services.Api;

public static class ApiErrorParser
{
    public static (string Message, Dictionary<string, string>? FieldErrors, string? ErrorCode) Parse(string content, string fallbackMessage)
    {
        try
        {
            var apiError = JsonSerializer.Deserialize(content, SportowyHubJsonContext.Default.ApiError);
            if (apiError?.Error != null)
            {
                return (apiError.Error.Message, apiError.Error.Violations, apiError.Error.Code);
            }
        }
        catch (JsonException)
        {
        }

        return (fallbackMessage, null, null);
    }
}
