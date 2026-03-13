namespace SportowyHub.Models;

public record Result<T>
{
    public bool IsSuccess { get; init; }
    public T? Data { get; init; }
    public string? ErrorMessage { get; init; }
    public Dictionary<string, string>? FieldErrors { get; init; }
    public string? ErrorCode { get; init; }

    public static Result<T> Success(T data) => new() { IsSuccess = true, Data = data };

    public static Result<T> Failure(string message, Dictionary<string, string>? fieldErrors = null, string? errorCode = null) => new()
    {
        IsSuccess = false,
        ErrorMessage = message,
        FieldErrors = fieldErrors,
        ErrorCode = errorCode
    };
}
