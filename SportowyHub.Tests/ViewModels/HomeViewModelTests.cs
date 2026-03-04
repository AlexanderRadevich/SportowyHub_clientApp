using FluentAssertions;
using NSubstitute;
using SportowyHub.Models;
using SportowyHub.Models.Api;
using SportowyHub.Services.Auth;
using SportowyHub.Services.Listings;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.Toast;
using SportowyHub.ViewModels;

namespace SportowyHub.Tests.ViewModels;

public class HomeViewModelTests
{
    private readonly IListingsService _listingsService = Substitute.For<IListingsService>();
    private readonly INavigationService _nav = Substitute.For<INavigationService>();
    private readonly IToastService _toastService = Substitute.For<IToastService>();
    private readonly IAuthService _authService = Substitute.For<IAuthService>();
    private readonly HomeViewModel _sut;

    public HomeViewModelTests()
    {
        _sut = new HomeViewModel(_listingsService, _nav, _toastService, _authService);
    }

    [Fact]
    public async Task GoToCreateListing_WhenUserNull_NavigatesToLoginWithReturnUrl()
    {
        _authService.GetCurrentUserAsync().Returns((UserInfo?)null);

        await _sut.GoToCreateListingCommand.ExecuteAsync(null);

        await _nav.Received(1).NavigateToLoginWithReturnUrlAsync("//home");
        await _nav.DidNotReceive().GoToAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task GoToCreateListing_WhenLoggedInAndVerified_NavigatesToCreateEditListing()
    {
        _authService.GetCurrentUserAsync().Returns(new UserInfo(1, "test@test.com", TrustLevels.PhoneVerified));

        await _sut.GoToCreateListingCommand.ExecuteAsync(null);

        await _nav.Received(1).GoToAsync("create-edit-listing");
    }

    [Fact]
    public async Task GoToCreateListing_WhenLoggedInButUnverified_ShowsToastError()
    {
        _authService.GetCurrentUserAsync().Returns(new UserInfo(1, "test@test.com", TrustLevels.Unverified));

        await _sut.GoToCreateListingCommand.ExecuteAsync(null);

        await _toastService.Received(1).ShowError(Arg.Any<string>());
        await _nav.DidNotReceive().GoToAsync(Arg.Any<string>());
    }
}
