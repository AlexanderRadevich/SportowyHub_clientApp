using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using SportowyHub.Models;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;
using SportowyHub.Services.ListingManagement;

namespace SportowyHub.Tests.Services;

public class ListingManagementServiceTests
{
    private readonly IRequestProvider _requestProvider = Substitute.For<IRequestProvider>();
    private readonly ITokenProvider _authService = Substitute.For<ITokenProvider>();
    private readonly ListingManagementService _sut;

    private static readonly CreateListingRequest _defaultRequest = new(
        CategoryId: 1,
        Title: "Test",
        Description: "Desc",
        Price: null,
        Currency: null,
        CityId: 1,
        VoivodeshipId: 1,
        LocationLatitude: null,
        LocationLongitude: null,
        Attributes: null,
        ContentLocale: null);

    private static readonly ListingDetail _defaultDetail = new(
        Id: "abc",
        Slug: null,
        Title: "Test",
        Description: null,
        Price: null,
        Currency: null,
        City: null,
        Region: null,
        Status: "draft",
        CategoryId: 1,
        ContentLocale: null,
        Condition: null,
        CreatedAt: "2024-01-01",
        PublishedAt: null,
        LastModeratorComment: null);

    public ListingManagementServiceTests()
    {
        _authService.GetTokenAsync().Returns("test-token");
        _sut = new ListingManagementService(_requestProvider, _authService, NullLogger<ListingManagementService>.Instance);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task UpdateListingAsync_NullOrEmptyId_ThrowsArgumentException(string? id)
    {
        var act = () => _sut.UpdateListingAsync(id!, new UpdateListingRequest(null, null, null, null, null, null, null, null, null, null, null));

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateListingAsync_WhenCalled_UsesEndpointWithNoTrailingSlash()
    {
        string? capturedUri = null;

        _requestProvider
            .PostAsync<CreateListingRequest, ListingDetail>(
                Arg.Do<string>(uri => capturedUri = uri),
                Arg.Any<CreateListingRequest>(),
                Arg.Any<string>(),
                Arg.Any<Dictionary<string, string>?>(),
                Arg.Any<CancellationToken>())
            .Returns(_defaultDetail);

        var result = await _sut.CreateListingAsync(_defaultRequest);

        result.IsSuccess.Should().BeTrue();
        capturedUri.Should().Be("/api/private/listings");
    }
}
