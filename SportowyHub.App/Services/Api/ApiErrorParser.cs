using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace SportowyHub.Services.Api;

public class ApiErrorParser(ILogger<ApiErrorParser> logger)
{
    public (string Message, Dictionary<string, string>? FieldErrors, string? ErrorCode) Parse(string content, string fallbackMessage)
    {
        try
        {
            var apiError = JsonSerializer.Deserialize(content, SportowyHubJsonContext.Default.ApiError);
            if (apiError?.Error != null)
            {
                return (apiError.Error.Message, apiError.Error.Violations, apiError.Error.Code);
            }
        }
        catch (JsonException ex)
        {
            logger.LogWarning(ex, "Failed to parse API error response");
        }

        return (fallbackMessage, null, null);
    }
}
