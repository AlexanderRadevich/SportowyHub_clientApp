using FluentAssertions;
using NSubstitute;
using SportowyHub.Services.Auth;
using SportowyHub.Services.Favorites;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.Toast;
using SportowyHub.ViewModels;

namespace SportowyHub.Tests.ViewModels;

public class FavoritesViewModelTests
{
    private readonly IFavoritesService _favoritesService = Substitute.For<IFavoritesService>();
    private readonly IAuthService _authService = Substitute.For<IAuthService>();
    private readonly INavigationService _nav = Substitute.For<INavigationService>();
    private readonly IToastService _toastService = Substitute.For<IToastService>();
    private readonly FavoritesViewModel _sut;

    public FavoritesViewModelTests()
    {
        _sut = new FavoritesViewModel(_favoritesService, _authService, _nav, _toastService);
    }

    [Fact]
    public async Task SignIn_NavigatesToLoginWithFavoritesReturnUrl()
    {
        await _sut.SignInCommand.ExecuteAsync(null);

        await _nav.Received(1).NavigateToLoginWithReturnUrlAsync("//favorites");
    }
}
