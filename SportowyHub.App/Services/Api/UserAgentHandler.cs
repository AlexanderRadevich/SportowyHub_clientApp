using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;

namespace SportowyHub.Services.Api;

public class UserAgentHandler : DelegatingHandler
{
    private readonly string _userAgent;

    public UserAgentHandler()
    {
        var version = AppInfo.Current.VersionString;
        var platform = DeviceInfo.Current.Platform;
        var osVersion = DeviceInfo.Current.VersionString;
        _userAgent = $"SportowyHub/{version} ({platform} {osVersion})";
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!request.Headers.UserAgent.Any())
        {
            request.Headers.TryAddWithoutValidation("User-Agent", _userAgent);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
