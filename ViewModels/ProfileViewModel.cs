using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Resources.Strings;

namespace SportowyHub.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
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
    private int _selectedLanguageIndex;

    [ObservableProperty]
    private int _selectedThemeIndex;

    private bool _initialized;

    public ProfileViewModel()
    {
        var langPref = Preferences.Get("app_language", "system");
        var langIdx = Array.IndexOf(LanguageCodes, langPref);
        _selectedLanguageIndex = langIdx >= 0 ? langIdx : 0;

        var themePref = Preferences.Get("app_theme", "system");
        var themeIdx = Array.IndexOf(ThemeCodes, themePref);
        _selectedThemeIndex = themeIdx >= 0 ? themeIdx : 0;

        _initialized = true;
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

            // Recreate Shell so IconTintColorBehavior re-applies with the new theme
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

        // Determine the culture to apply
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

        // Recreate Shell to refresh all {x:Static} bindings
        if (Application.Current?.Windows.Count > 0)
        {
            Application.Current.Windows[0].Page = new AppShell();
        }
    }

    [RelayCommand]
    private async Task SignInAsync()
    {
        await Shell.Current.GoToAsync("login");
    }

    [RelayCommand]
    private async Task CreateAccountAsync()
    {
        await Shell.Current.GoToAsync("register");
    }
}
