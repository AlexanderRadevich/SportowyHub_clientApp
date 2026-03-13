using System.Net;
using FluentAssertions;
using NSubstitute;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;

namespace SportowyHub.Tests.Services;

public class RequestProviderTests
{
    private readonly IHttpClientFactory _httpClientFactory = Substitute.For<IHttpClientFactory>();
    private readonly RequestProvider _sut;

    public RequestProviderTests()
    {
        _sut = new RequestProvider(_httpClientFactory);
    }

    [Fact]
    public async Task GetAsync_NullResponseBody_ThrowsInvalidOperationException()
    {
        var handler = new FakeHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null")
        });
        var client = new HttpClient(handler) { BaseAddress = new Uri("https://test.local") };
        _httpClientFactory.CreateClient("Api").Returns(client);

        var act = () => _sut.GetAsync<ListingsResponse>("/api/test");

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*ListingsResponse*");
    }

    [Fact]
    public async Task PostAsync_NullResponseBody_ThrowsInvalidOperationException()
    {
        var handler = new FakeHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null")
        });
        var client = new HttpClient(handler) { BaseAddress = new Uri("https://test.local") };
        _httpClientFactory.CreateClient("Api").Returns(client);

        var act = () => _sut.PostAsync<LoginRequest, LoginResponse>(
            "/api/test",
            new LoginRequest("test@test.com", "password"));

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*LoginResponse*");
    }

    private sealed class FakeHandler(HttpResponseMessage response) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(response);
    }
}
