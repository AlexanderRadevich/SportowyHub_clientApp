using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Models.Api;
using SportowyHub.Resources.Strings;
using SportowyHub.Services.Auth;
using SportowyHub.Services.Toast;

namespace SportowyHub.ViewModels;

public partial class AccountProfileViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly IToastService _toastService;

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial bool HasError { get; set; }

    [ObservableProperty]
    public partial UserProfile? Profile { get; set; }

    public string DisplayName
    {
        get
        {
            var parts = new[] { Profile?.FirstName, Profile?.LastName }
                .Where(p => !string.IsNullOrWhiteSpace(p));
            var name = string.Join(" ", parts);
            return string.IsNullOrEmpty(name) ? Profile?.Email ?? "" : name;
        }
    }

    public string DisplayEmail => Profile?.Email ?? "";

    public AccountProfileViewModel(IAuthService authService, IToastService toastService)
    {
        _authService = authService;
        _toastService = toastService;
    }

    [RelayCommand]
    private async Task LoadProfile()
    {
        IsLoading = true;
        HasError = false;

        try
        {
            Profile = await _authService.GetProfileAsync();

            if (Profile is null)
                HasError = true;
        }
        catch (Exception ex)
        {
            HasError = true;
            await _toastService.ShowError(ex.Message);
        }

        OnPropertyChanged(nameof(DisplayName));
        OnPropertyChanged(nameof(DisplayEmail));

        IsLoading = false;
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

        try
        {
            await _authService.LogoutAsync();
        }
        catch (Exception ex)
        {
            await _authService.ClearAuthAsync();
            await _toastService.ShowError(ex.Message);
        }

        await Shell.Current.GoToAsync("..");
    }
}
