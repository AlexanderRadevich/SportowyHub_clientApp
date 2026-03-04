namespace SportowyHub.Services.Navigation;

public class ReturnUrlService : IReturnUrlService
{
    private string? _returnUrl;

    public bool HasReturnUrl => _returnUrl is not null;

    public void SetReturnUrl(string route)
    {
        _returnUrl = route;
    }

    public string? ConsumeReturnUrl()
    {
        var url = _returnUrl;
        _returnUrl = null;
        return url;
    }
}
