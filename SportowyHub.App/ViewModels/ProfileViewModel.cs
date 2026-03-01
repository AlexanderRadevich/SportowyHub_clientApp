using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Resources.Strings;
using SportowyHub.Services.Auth;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.Toast;

namespace SportowyHub.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly INavigationService _nav;
    private readonly IToastService _toastService;

    private static readonly string[] LanguageCodes = ["system", "pl", "en", "uk", "ru"];
    private static readonly string[] ThemeCodes = ["system", "light", "dark"];

    public List<string> Languages { get; } =
    [
        AppResources.SettingsLanguageSystem,
        AppResources.SettingsLanguagePl,
        AppResources.SettingsLanguageEn,
        AppResources.SettingsLanguageUk,
        AppResources.SettingsLanguageRu
    ];

    public List<string> Themes { get; } =
    [
        AppResources.SettingsThemeSystem,
        AppResources.SettingsThemeLight,
        AppResources.SettingsThemeDark
    ];

    [ObservableProperty]
    public partial int SelectedLanguageIndex { get; set; }

    [ObservableProperty]
    public partial int SelectedThemeIndex { get; set; }

    [ObservableProperty]
    public partial bool IsLoggedIn { get; set; }

    private bool _initialized;

    public ProfileViewModel(IAuthService authService, INavigationService nav, IToastService toastService)
    {
        _authService = authService;
        _nav = nav;
        _toastService = toastService;

        var langPref = Preferences.Get("app_language", "system");
        var langIdx = Array.IndexOf(LanguageCodes, langPref);
        SelectedLanguageIndex = langIdx >= 0 ? langIdx : 0;

        var themePref = Preferences.Get("app_theme", "system");
        var themeIdx = Array.IndexOf(ThemeCodes, themePref);
        SelectedThemeIndex = themeIdx >= 0 ? themeIdx : 0;

        _initialized = true;
    }

    [RelayCommand]
    private async Task RefreshAuthState()
    {
        IsLoggedIn = await _authService.IsLoggedInAsync();
    }

    partial void OnSelectedThemeIndexChanged(int value)
    {
        if (!_initialized) return;

        var code = value >= 0 && value < ThemeCodes.Length ? ThemeCodes[value] : "system";
        Preferences.Set("app_theme", code);

        if (Application.Current is not null)
        {
            Application.Current.UserAppTheme = code switch
            {
                "light" => AppTheme.Light,
                "dark" => AppTheme.Dark,
                _ => AppTheme.Unspecified
            };

            if (Application.Current.Windows.Count > 0)
            {
                Application.Current.Windows[0].Page = new AppShell();
            }
        }
    }

    partial void OnSelectedLanguageIndexChanged(int value)
    {
        if (!_initialized) return;

        var code = value >= 0 && value < LanguageCodes.Length ? LanguageCodes[value] : "system";
        Preferences.Set("app_language", code);

        CultureInfo culture;
        if (code == "system")
        {
            var deviceLang = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;
            string[] supported = ["pl", "en", "uk", "ru"];
            culture = supported.Contains(deviceLang) ? new CultureInfo(deviceLang) : new CultureInfo("pl");
        }
        else
        {
            culture = new CultureInfo(code);
        }

        CultureInfo.CurrentUICulture = culture;
        CultureInfo.CurrentCulture = culture;

        if (Application.Current?.Windows.Count > 0)
        {
            Application.Current.Windows[0].Page = new AppShell();
        }
    }

    [RelayCommand]
    private async Task SignOut()
    {
        var confirmed = await _nav.DisplayAlertAsync(
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
            await _authService.LogoutAsync();
        }
        catch (Exception ex)
        {
            await _authService.ClearAuthAsync();
            await _toastService.ShowError(ex.Message);
        }

        await RefreshAuthState();
    }

    [RelayCommand]
    private async Task SignInAsync()
    {
        await _nav.GoToAsync("login");
    }

    [RelayCommand]
    private async Task CreateAccountAsync()
    {
        await _nav.GoToAsync("register");
    }

    [RelayCommand]
    private async Task GoToAccountProfile()
    {
        await _nav.GoToAsync("account-profile");
    }
}
