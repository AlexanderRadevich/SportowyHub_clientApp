using FluentAssertions;
using NSubstitute;
using SportowyHub.Services.Auth;
using SportowyHub.Services.Favorites;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.Toast;
using SportowyHub.ViewModels;

namespace SportowyHub.Tests.ViewModels;

public class ProfileViewModelTests
{
    [Fact(Skip = "ProfileViewModel constructor calls Preferences.Get() which requires MAUI runtime")]
    public async Task SignIn_NavigatesToLoginWithProfileReturnUrl()
    {
        var authService = Substitute.For<IAuthService>();
        var nav = Substitute.For<INavigationService>();
        var toastService = Substitute.For<IToastService>();
        var favoritesService = Substitute.For<IFavoritesService>();
        var sut = new ProfileViewModel(authService, nav, toastService, favoritesService);

        await sut.SignInCommand.ExecuteAsync(null);

        await nav.Received(1).NavigateToLoginWithReturnUrlAsync("//profile");
    }
}
