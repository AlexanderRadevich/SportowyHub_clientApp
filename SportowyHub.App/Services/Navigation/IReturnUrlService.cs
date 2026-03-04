namespace SportowyHub.Services.Navigation;

public interface IReturnUrlService
{
    void SetReturnUrl(string route);
    string? ConsumeReturnUrl();
    bool HasReturnUrl { get; }
}
