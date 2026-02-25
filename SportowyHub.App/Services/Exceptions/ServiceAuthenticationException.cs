using System.Net;

namespace SportowyHub.Services.Exceptions;

public class ServiceAuthenticationException : Exception
{
    public string Content { get; }
    public HttpStatusCode StatusCode { get; }

    public ServiceAuthenticationException(string content, HttpStatusCode statusCode)
        : base("Authentication failed")
    {
        Content = content;
        StatusCode = statusCode;
    }
}
