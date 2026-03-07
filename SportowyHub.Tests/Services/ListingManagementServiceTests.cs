using FluentAssertions;
using NSubstitute;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;
using SportowyHub.Services.ListingManagement;

namespace SportowyHub.Tests.Services;

public class ListingManagementServiceTests
{
    private readonly IRequestProvider _requestProvider = Substitute.For<IRequestProvider>();
    private readonly IAuthService _authService = Substitute.For<IAuthService>();
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
        _sut = new ListingManagementService(_requestProvider, _authService);
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

        await _sut.CreateListingAsync(_defaultRequest);

        capturedUri.Should().Be("/api/private/listings");
    }
}
