using FluentAssertions;
using NSubstitute;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;
using SportowyHub.Services.Messaging;

namespace SportowyHub.Tests.Services;

public class MessagingServiceTests
{
    private readonly IRequestProvider _requestProvider = Substitute.For<IRequestProvider>();
    private readonly IAuthService _authService = Substitute.For<IAuthService>();
    private readonly MessagingService _sut;

    private static readonly Conversation _defaultConversation = new(
        Id: 1,
        ListingId: "listing-1",
        ListingTitle: null,
        BuyerId: 10,
        SellerId: 20,
        CreatedAt: "2024-01-01",
        UpdatedAt: "2024-01-01");

    public MessagingServiceTests()
    {
        _authService.GetTokenAsync().Returns("test-token");
        _sut = new MessagingService(_requestProvider, _authService);
    }

    [Fact]
    public async Task CreateConversationAsync_WhenCalled_UsesEndpointWithNoTrailingSlash()
    {
        string? capturedUri = null;

        _requestProvider
            .PostAsync<CreateConversationRequest, Conversation>(
                Arg.Do<string>(uri => capturedUri = uri),
                Arg.Any<CreateConversationRequest>(),
                Arg.Any<string>(),
                Arg.Any<Dictionary<string, string>?>(),
                Arg.Any<CancellationToken>())
            .Returns(_defaultConversation);

        await _sut.CreateConversationAsync("listing-1");

        capturedUri.Should().Be("/api/private/conversations");
    }
}
