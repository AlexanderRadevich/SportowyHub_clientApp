namespace SportowyHub.Models.Api;

public record ApiError(ErrorDetail Error);

public record ErrorDetail(string Code, string Message);
