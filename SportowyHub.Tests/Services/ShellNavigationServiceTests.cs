using FluentAssertions;
using NSubstitute;
using SportowyHub.Services.Navigation;

namespace SportowyHub.Tests.Services;

public class ShellNavigationServiceTests
{
    private readonly IReturnUrlService _returnUrlService = Substitute.For<IReturnUrlService>();
    private readonly ShellNavigationService _sut;

    public ShellNavigationServiceTests()
    {
        _sut = new ShellNavigationService(_returnUrlService);
    }

    [Fact]
    public void Constructor_AcceptsReturnUrlService()
    {
        _sut.Should().NotBeNull();
    }

    [Fact]
    public async Task NavigateToLoginWithReturnUrlAsync_SetsReturnUrlBeforeNavigating()
    {
        try
        {
            await _sut.NavigateToLoginWithReturnUrlAsync("//favorites");
        }
        catch (NullReferenceException)
        {
        }

        _returnUrlService.Received(1).SetReturnUrl("//favorites");
    }
}
