using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;
using SportowyHub.Services.Favorites;

namespace SportowyHub.Tests.Services;

public class FavoritesServiceTests
{
    private readonly IRequestProvider _requestProvider = Substitute.For<IRequestProvider>();
    private readonly ITokenProvider _authService = Substitute.For<ITokenProvider>();
    private readonly FavoritesService _sut;

    public FavoritesServiceTests()
    {
        _authService.GetTokenAsync().Returns("test-token");
        _sut = new FavoritesService(_requestProvider, _authService, NullLogger<FavoritesService>.Instance);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task AddAsync_NullOrEmptyListingId_ThrowsArgumentException(string? listingId)
    {
        var act = () => _sut.AddAsync(listingId!);

        await act.Should().ThrowAsync<ArgumentException>();
    }
}
