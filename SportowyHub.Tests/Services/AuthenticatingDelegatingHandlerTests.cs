using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;
using NSubstitute;
using Microsoft.Extensions.Logging;
using SportowyHub.Models;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;
using SportowyHub.Services.Navigation;

namespace SportowyHub.Tests.Services;

public class AuthenticatingDelegatingHandlerTests
{
    private readonly ITokenProvider _authService = Substitute.For<ITokenProvider>();
    private readonly INavigationService _navigationService = Substitute.For<INavigationService>();
    private readonly ILogger<AuthenticatingDelegatingHandler> _logger = Substitute.For<ILogger<AuthenticatingDelegatingHandler>>();

    private HttpClient CreateClient(
        HttpStatusCode initialStatus,
        HttpStatusCode? retryStatus = null)
    {
        var callCount = 0;
        var innerHandler = new DelegatingHandlerStub((request, ct) =>
        {
            callCount++;
            var status = callCount == 1 ? initialStatus : retryStatus ?? HttpStatusCode.OK;
            return Task.FromResult(new HttpResponseMessage(status));
        });

        var handler = new AuthenticatingDelegatingHandler(_authService, _navigationService, _logger)
        {
            InnerHandler = innerHandler
        };

        return new HttpClient(handler) { BaseAddress = new Uri("https://api.test.com") };
    }

    [Fact]
    public async Task SendAsync_On401_RefreshesTokenAndRetriesRequest()
    {
        _authService.RefreshTokenAsync(Arg.Any<CancellationToken>())
            .Returns(Result<LoginResponse>.Success(new LoginResponse("new-token", 3600, null, null)));
        _authService.GetTokenAsync()
            .Returns("new-token");
        _authService.GetTokenExpiryAsync()
            .Returns((DateTimeOffset?)null);

        using var client = CreateClient(HttpStatusCode.Unauthorized, HttpStatusCode.OK);
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/private/test");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "expired-token");

        var response = await client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await _authService.Received(1).RefreshTokenAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SendAsync_OnRefreshFailure_Returns401()
    {
        _authService.RefreshTokenAsync(Arg.Any<CancellationToken>())
            .Returns(Result<LoginResponse>.Failure("Session expired"));
        _authService.GetTokenExpiryAsync()
            .Returns((DateTimeOffset?)null);
        _authService.ClearAuthAsync()
            .Returns(Task.CompletedTask);

        using var client = CreateClient(HttpStatusCode.Unauthorized);
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/private/test");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "expired-token");

        var response = await client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        await _authService.Received(1).ClearAuthAsync();
    }

    [Fact]
    public async Task SendAsync_Non401Response_PassesThroughWithoutRefresh()
    {
        _authService.GetTokenExpiryAsync()
            .Returns((DateTimeOffset?)null);

        using var client = CreateClient(HttpStatusCode.OK);
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/private/test");

        var response = await client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await _authService.DidNotReceive().RefreshTokenAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SendAsync_ConcurrentRequests_RefreshIsSerializedBySemaphore()
    {
        var refreshCount = 0;
        _authService.RefreshTokenAsync(Arg.Any<CancellationToken>())
            .Returns(async callInfo =>
            {
                Interlocked.Increment(ref refreshCount);
                await Task.Delay(50);
                return Result<LoginResponse>.Success(new LoginResponse("new-token", 3600, null, null));
            });
        _authService.GetTokenAsync()
            .Returns("new-token");
        _authService.GetTokenExpiryAsync()
            .Returns((DateTimeOffset?)null);

        var callCount = 0;
        var innerHandler = new DelegatingHandlerStub((request, ct) =>
        {
            var count = Interlocked.Increment(ref callCount);
            var status = count <= 3 ? HttpStatusCode.Unauthorized : HttpStatusCode.OK;
            return Task.FromResult(new HttpResponseMessage(status));
        });

        var handler = new AuthenticatingDelegatingHandler(_authService, _navigationService, _logger)
        {
            InnerHandler = innerHandler
        };

        using var client = new HttpClient(handler) { BaseAddress = new Uri("https://api.test.com") };

        var tasks = Enumerable.Range(0, 3).Select(_ =>
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "/api/private/test");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "expired");
            return client.SendAsync(req);
        });

        await Task.WhenAll(tasks);

        await _authService.Received().RefreshTokenAsync(Arg.Any<CancellationToken>());
    }

    private class DelegatingHandlerStub(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler)
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) => handler(request, cancellationToken);
    }
}
