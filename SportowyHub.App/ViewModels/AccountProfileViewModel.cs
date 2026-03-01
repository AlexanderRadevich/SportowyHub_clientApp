using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Models.Api;
using SportowyHub.Resources.Strings;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.Toast;

namespace SportowyHub.ViewModels;

public partial class AccountProfileViewModel(
    IAuthService authService,
    INavigationService nav,
    IToastService toastService) : ObservableObject
{
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
            if (!string.IsNullOrWhiteSpace(Profile?.Account?.FullName))
            {
                return Profile.Account.FullName;
            }

            var parts = new[] { Profile?.Account?.FirstName, Profile?.Account?.LastName }
                .Where(p => !string.IsNullOrWhiteSpace(p));
            var name = string.Join(" ", parts);
            return string.IsNullOrEmpty(name) ? Profile?.Email ?? "" : name;
        }
    }

    public string DisplayEmail => Profile?.Email ?? "";

    public string FormattedBalance
    {
        get
        {
            var grosze = Profile?.Account?.BalanceGrosze ?? 0;
            return $"{grosze / 100.0:F2} zł";
        }
    }

    public string TrustInfo
    {
        get
        {
            var level = Profile?.TrustLevel ?? "";
            var rep = Profile?.ReputationScore ?? 0;
            return $"{level} · {rep} rep";
        }
    }

    public bool IsGoogleLinked => Profile?.OauthLinked?.Google ?? false;

    public bool HasName => Profile?.Account is { } a
        && (!string.IsNullOrWhiteSpace(a.FullName)
            || !string.IsNullOrWhiteSpace(a.FirstName)
            || !string.IsNullOrWhiteSpace(a.LastName));

    [RelayCommand]
    private async Task LoadProfile(CancellationToken ct)
    {
        IsLoading = true;
        HasError = false;

        try
        {
            Profile = await authService.GetProfileAsync(ct);

            if (Profile is null)
            {
                HasError = true;
            }
        }
        catch (Exception ex)
        {
            HasError = true;
            await toastService.ShowError(ex.Message);
        }
        finally
        {
            OnPropertyChanged(nameof(DisplayName));
            OnPropertyChanged(nameof(DisplayEmail));
            OnPropertyChanged(nameof(FormattedBalance));
            OnPropertyChanged(nameof(TrustInfo));
            OnPropertyChanged(nameof(IsGoogleLinked));
            OnPropertyChanged(nameof(HasName));
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task EditProfile()
    {
        if (Profile is null) return;

        var json = JsonSerializer.Serialize(Profile, SportowyHubJsonContext.Default.UserProfile);
        await nav.GoToAsync($"edit-profile?profile={Uri.EscapeDataString(json)}");
    }

    [RelayCommand]
    private async Task SignOut()
    {
        var confirmed = await nav.DisplayAlertAsync(
            AppResources.SignOutConfirmTitle,
            AppResources.SignOutConfirmMessage,
            AppResources.SignOut,
            AppResources.Cancel);

        if (!confirmed)
        {
            return;
        }

        try
        {
            await authService.LogoutAsync();
        }
        catch (Exception ex)
        {
            await authService.ClearAuthAsync();
            await toastService.ShowError(ex.Message);
        }

        await nav.GoBackAsync();
    }
}
