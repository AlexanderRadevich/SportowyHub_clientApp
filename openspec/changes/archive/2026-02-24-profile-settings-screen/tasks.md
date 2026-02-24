## 1. Resource Strings

- [x] 1.1 Add new resource keys to `AppResources.resx` (Polish base): `ProfileAccountSection`, `ProfileSettingsSection`, `SettingsLanguage`, `SettingsTheme`, `SettingsThemeLight`, `SettingsThemeDark`, `SettingsThemeSystem`, `SettingsLanguageSystem`, `SettingsLanguagePl`, `SettingsLanguageEn`, `SettingsLanguageUk`, `SettingsLanguageRu`
- [x] 1.2 Add the same keys with English translations to `AppResources.en.resx`
- [x] 1.3 Add the same keys with Ukrainian translations to `AppResources.uk.resx`
- [x] 1.4 Add the same keys with Russian translations to `AppResources.ru.resx`

## 2. App Startup — Theme & Language Preferences

- [x] 2.1 Update `App.xaml.cs` constructor to read `Preferences.Get("app_theme", "system")` and set `UserAppTheme` to `AppTheme.Light`, `AppTheme.Dark`, or `AppTheme.Unspecified` before `InitializeComponent()`
- [x] 2.2 Update `App.xaml.cs` constructor to read `Preferences.Get("app_language", "system")` and, if not `"system"`, set `CultureInfo.CurrentUICulture`/`CurrentCulture` to the stored culture before the existing system detection logic

## 3. ProfileViewModel

- [x] 3.1 Create `ViewModels/ProfileViewModel.cs` using `CommunityToolkit.Mvvm` with `ObservableObject` base class
- [x] 3.2 Add `SignInCommand` (`RelayCommand`) that navigates to the Login page via `Shell.Current.GoToAsync`
- [x] 3.3 Add `CreateAccountCommand` (`RelayCommand`) that navigates to the Register page via `Shell.Current.GoToAsync`
- [x] 3.4 Add language picker properties: `Languages` (list of display names), `SelectedLanguageIndex` (int, bound to Picker). Initialize from `Preferences.Get("app_language", "system")` mapping to the correct index
- [x] 3.5 Add theme picker properties: `Themes` (list of display names), `SelectedThemeIndex` (int, bound to Picker). Initialize from `Preferences.Get("app_theme", "system")` mapping to the correct index
- [x] 3.6 Implement `OnSelectedThemeIndexChanged`: map index to `"system"`/`"light"`/`"dark"`, save to `Preferences`, set `Application.Current.UserAppTheme`
- [x] 3.7 Implement `OnSelectedLanguageIndexChanged`: map index to culture code, save to `Preferences`, set `CultureInfo`, recreate Shell via `Application.Current.Windows[0].Page = new AppShell()`

## 4. Profile Hub Page

- [x] 4.1 Redesign `Views/Profile/ProfilePage.xaml`: replace centered welcome layout with `ScrollView` > `VerticalStackLayout` containing logo, Account section (header + Sign In / Create Account rows with chevrons), and Settings section (header + Language picker row + Theme picker row)
- [x] 4.2 Update `Views/Profile/ProfilePage.xaml.cs` code-behind: remove old `OnCreateAccountClicked`/`OnSignInClicked` event handlers, set `BindingContext` to `ProfileViewModel`
- [x] 4.3 Register `ProfileViewModel` in DI container in `MauiProgram.cs` (if DI is used) or instantiate directly in code-behind

## 5. Styles

- [x] 5.1 Add a `ProfileRow` implicit or named style in `Styles.xaml` if needed for consistent row height (48px min), padding (16px horizontal), and bottom border separator — or define inline in ProfilePage.xaml if simpler

## 6. Verification

- [x] 6.1 Verify Profile tab displays grouped-list layout with Account and Settings sections
- [x] 6.2 Verify Sign In and Create Account rows navigate to Login and Register pages respectively
- [x] 6.3 Verify theme picker changes theme immediately (Light, Dark, System) and persists across app restarts
- [x] 6.4 Verify language picker changes language immediately and persists across app restarts
- [x] 6.5 Verify all strings display correctly in all 4 languages (pl, en, uk, ru)
- [x] 6.6 Verify default state (no stored preferences) behaves as system-auto for both theme and language
