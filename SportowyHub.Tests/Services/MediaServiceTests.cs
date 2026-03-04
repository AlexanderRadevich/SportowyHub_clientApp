using FluentAssertions;
using NSubstitute;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;
using SportowyHub.Services.Media;

namespace SportowyHub.Tests.Services;

public class MediaServiceTests
{
    private readonly IRequestProvider _requestProvider = Substitute.For<IRequestProvider>();
    private readonly IAuthService _authService = Substitute.For<IAuthService>();
    private readonly MediaService _sut;

    private static readonly MediaItem _defaultMedia = new(
        Id: 1,
        ListingId: "listing-1",
        Type: "image",
        MimeType: "image/jpeg",
        Size: 1024,
        Width: 800,
        Height: 600,
        SortOrder: 0,
        Urls: new MediaUrls(null, null, null, null, null, null),
        CreatedAt: "2024-01-01");

    public MediaServiceTests()
    {
        _authService.GetTokenAsync().Returns("test-token");
        _sut = new MediaService(_requestProvider, _authService);
    }

    [Fact]
    public async Task UploadAsync_WhenCalled_UsesEndpointWithNoTrailingSlash()
    {
        string? capturedUri = null;

        _requestProvider
            .PostMultipartAsync<MediaItem>(
                Arg.Do<string>(uri => capturedUri = uri),
                Arg.Any<MultipartFormDataContent>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(_defaultMedia);

        using var stream = new MemoryStream([1, 2, 3]);
        await _sut.UploadAsync("listing-1", stream, "photo.jpg");

        capturedUri.Should().Be("/api/private/media");
    }
}
