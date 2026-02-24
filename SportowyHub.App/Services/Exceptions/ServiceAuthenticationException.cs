namespace SportowyHub.Services.Exceptions;

public class ServiceAuthenticationException : Exception
{
    public string Content { get; }

    public ServiceAuthenticationException(string content)
        : base("Authentication failed")
    {
        Content = content;
    }
}
