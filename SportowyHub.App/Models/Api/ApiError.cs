namespace SportowyHub.Models.Api;

public record ApiError(ErrorDetail Error, string? Locale = null);

public record ErrorDetail(string Code, string Message, Dictionary<string, string>? Violations = null);
