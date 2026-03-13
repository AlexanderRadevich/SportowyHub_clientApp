using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SportowyHub.Models;
using SportowyHub.Models.Api;
using SportowyHub.Services.Auth;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.Toast;
using SportowyHub.ViewModels;

namespace SportowyHub.Tests.ViewModels;

public class LoginViewModelTests
{
    private readonly IAuthService _authService = Substitute.For<IAuthService>();
    private readonly INavigationService _nav = Substitute.For<INavigationService>();
    private readonly IToastService _toastService = Substitute.For<IToastService>();
    private readonly IReturnUrlService _returnUrlService = Substitute.For<IReturnUrlService>();
    private readonly ILogger<LoginViewModel> _logger = Substitute.For<ILogger<LoginViewModel>>();
    private readonly LoginViewModel _sut;

    public LoginViewModelTests()
    {
        _sut = new LoginViewModel(_authService, _nav, _toastService, _returnUrlService, _logger);
    }

    [Fact]
    public async Task Login_WhenSuccessful_WithReturnUrl_NavigatesToReturnUrl()
    {
        _sut.Email = "test@test.com";
        _sut.Password = "password123";
        _authService.LoginAsync("test@test.com", "password123", Arg.Any<CancellationToken>())
            .Returns(Result<LoginResponse>.Success(new LoginResponse("token", 3600, "bearer")));
        _returnUrlService.ConsumeReturnUrl().Returns("//favorites");

        await _sut.LoginCommand.ExecuteAsync(null);

        await _nav.Received(1).GoToAsync("//favorites");
    }

    [Fact]
    public async Task Login_WhenSuccessful_WithoutReturnUrl_NavigatesToHome()
    {
        _sut.Email = "test@test.com";
        _sut.Password = "password123";
        _authService.LoginAsync("test@test.com", "password123", Arg.Any<CancellationToken>())
            .Returns(Result<LoginResponse>.Success(new LoginResponse("token", 3600, "bearer")));
        _returnUrlService.ConsumeReturnUrl().Returns((string?)null);

        await _sut.LoginCommand.ExecuteAsync(null);

        await _nav.Received(1).GoToAsync("//home");
    }

    [Fact]
    public async Task Login_WhenFailed_DoesNotConsumeReturnUrl()
    {
        _sut.Email = "test@test.com";
        _sut.Password = "wrong";
        _authService.LoginAsync("test@test.com", "wrong", Arg.Any<CancellationToken>())
            .Returns(Result<LoginResponse>.Failure("Invalid credentials"));

        await _sut.LoginCommand.ExecuteAsync(null);

        _returnUrlService.DidNotReceive().ConsumeReturnUrl();
    }

    [Fact]
    public async Task OAuthLoginWithGoogle_WhenSuccessful_WithReturnUrl_NavigatesToReturnUrl()
    {
        _authService.GoogleSignInAsync(Arg.Any<CancellationToken>())
            .Returns(Result<LoginResponse>.Success(new LoginResponse("token", 3600, "bearer")));
        _returnUrlService.ConsumeReturnUrl().Returns("//profile");

        await _sut.OAuthLoginWithGoogleCommand.ExecuteAsync(null);

        await _nav.Received(1).GoToAsync("//profile");
    }

    [Fact]
    public async Task OAuthLoginWithGoogle_WhenSuccessful_WithoutReturnUrl_NavigatesToHome()
    {
        _authService.GoogleSignInAsync(Arg.Any<CancellationToken>())
            .Returns(Result<LoginResponse>.Success(new LoginResponse("token", 3600, "bearer")));
        _returnUrlService.ConsumeReturnUrl().Returns((string?)null);

        await _sut.OAuthLoginWithGoogleCommand.ExecuteAsync(null);

        await _nav.Received(1).GoToAsync("//home");
    }
}
