using System.Net;

namespace SportowyHub.Services.Exceptions;

public class ServiceAuthenticationException(string content, HttpStatusCode statusCode)
    : Exception("Authentication failed")
{
    public string Content { get; } = content;
    public HttpStatusCode StatusCode { get; } = statusCode;
}
