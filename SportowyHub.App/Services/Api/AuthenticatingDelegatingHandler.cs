using Microsoft.Extensions.Logging;
using SportowyHub.Services.Auth;
using SportowyHub.Services.Navigation;

namespace SportowyHub.Services.Api;

public class AuthenticatingDelegatingHandler(
    ITokenProvider authService,
    INavigationService navigationService,
    ILogger<AuthenticatingDelegatingHandler> logger) : DelegatingHandler
{
    private static readonly SemaphoreSlim _refreshSemaphore = new(1, 1);
    private const string RefreshEndpoint = "/api/v1/refresh";
    private const string SkipRefreshHeader = "X-Skip-Token-Refresh";

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        await RefreshIfExpiringAsync(request, cancellationToken);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
        {
            return response;
        }

        if (IsRefreshRequest(request) || request.Headers.Contains(SkipRefreshHeader))
        {
            return response;
        }

        logger.LogDebug("Received 401 for {RequestUri}, attempting token refresh", request.RequestUri);

        var refreshed = await TryRefreshTokenAsync(cancellationToken);
        if (!refreshed)
        {
            logger.LogWarning("Token refresh failed, redirecting to login");
            await RedirectToLoginAsync();
            return response;
        }

        var retryRequest = await CloneRequestAsync(request);
        var newToken = await authService.GetTokenAsync();
        if (!string.IsNullOrEmpty(newToken))
        {
            retryRequest.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", newToken);
        }

        retryRequest.Headers.TryAddWithoutValidation(SkipRefreshHeader, "true");

        response.Dispose();
        return await base.SendAsync(retryRequest, cancellationToken);
    }

    private async Task<bool> TryRefreshTokenAsync(CancellationToken cancellationToken)
    {
        var acquired = await _refreshSemaphore.WaitAsync(TimeSpan.FromSeconds(10), cancellationToken);
        if (!acquired)
        {
            logger.LogWarning("Timed out waiting for token refresh semaphore");
            return false;
        }

        try
        {
            var result = await authService.RefreshTokenAsync(cancellationToken);
            return result.IsSuccess;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Token refresh threw an exception");
            return false;
        }
        finally
        {
            _refreshSemaphore.Release();
        }
    }

    private async Task RedirectToLoginAsync()
    {
        await authService.ClearAuthAsync();
        try
        {
            await navigationService.GoToAsync("//login");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to redirect to login after token refresh failure");
        }
    }

    private async Task RefreshIfExpiringAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (IsRefreshRequest(request) || request.Headers.Contains(SkipRefreshHeader))
        {
            return;
        }

        if (request.Headers.Authorization?.Scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase) != true)
        {
            return;
        }

        var expiry = await authService.GetTokenExpiryAsync();
        if (expiry is null || expiry.Value > DateTimeOffset.UtcNow.AddMinutes(1))
        {
            return;
        }

        logger.LogDebug("Token expiring soon, proactively refreshing");
        var refreshed = await TryRefreshTokenAsync(cancellationToken);
        if (refreshed)
        {
            var newToken = await authService.GetTokenAsync();
            if (!string.IsNullOrEmpty(newToken))
            {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", newToken);
            }
        }
    }

    private static bool IsRefreshRequest(HttpRequestMessage request)
    {
        return request.RequestUri?.AbsolutePath.Contains(RefreshEndpoint, StringComparison.OrdinalIgnoreCase) == true;
    }

    private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage original)
    {
        var clone = new HttpRequestMessage(original.Method, original.RequestUri);

        if (original.Content is not null)
        {
            var content = await original.Content.ReadAsByteArrayAsync();
            clone.Content = new ByteArrayContent(content);

            foreach (var header in original.Content.Headers)
            {
                clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        foreach (var header in original.Headers)
        {
            if (!string.Equals(header.Key, "Authorization", StringComparison.OrdinalIgnoreCase))
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        return clone;
    }
}
