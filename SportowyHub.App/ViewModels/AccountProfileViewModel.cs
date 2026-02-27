using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Resources.Strings;
using SportowyHub.Services.Auth;

namespace SportowyHub.ViewModels;

public partial class AccountProfileViewModel : ObservableObject
{
    private readonly IAuthService _authService;

    public AccountProfileViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private async Task SignOut()
    {
        var confirmed = await Application.Current!.Windows[0].Page!.DisplayAlertAsync(
            AppResources.SignOutConfirmTitle,
            AppResources.SignOutConfirmMessage,
            AppResources.SignOut,
            AppResources.Cancel);

        if (!confirmed)
            return;

        await _authService.LogoutAsync();
        await Shell.Current.GoToAsync("..");
    }
}
