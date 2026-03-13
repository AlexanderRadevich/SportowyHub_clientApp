using SportowyHub.Models;

namespace SportowyHub.Models.Api;

public record AuthResult<T> : Result<T>
{
    public new static AuthResult<T> Success(T data) => new()
    {
        IsSuccess = true,
        Data = data
    };

    public new static AuthResult<T> Failure(string message, Dictionary<string, string>? fieldErrors = null, string? errorCode = null) => new()
    {
        IsSuccess = false,
        ErrorMessage = message,
        FieldErrors = fieldErrors,
        ErrorCode = errorCode
    };
}
