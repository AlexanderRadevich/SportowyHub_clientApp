## Why

The app currently has all ~34 user-facing strings hardcoded in English across XAML and C# files with no localization infrastructure. To reach a broader audience (Polish, Ukrainian, and Russian-speaking users), the app needs multilanguage support with automatic language detection from system settings and a sensible fallback (Polish as the default).

## What Changes

- Introduce a localization infrastructure using .NET MAUI's built-in resource-based localization (`.resx` files)
- Add a `LocalizationService` that detects the device's system language and selects the best match from supported languages, falling back to Polish (`pl`) if no match is found
- Create resource files for all four supported languages: Polish (`pl`), English (`en`), Ukrainian (`uk`), Russian (`ru`)
- Replace all ~34 hardcoded strings in XAML files and C# code-behind with localized resource references
- Provide a XAML markup extension or binding approach for consuming localized strings in XAML
- Translate all existing labels, placeholders, validation messages, and UI text into all four languages

## Capabilities

### New Capabilities
- `multilanguage-support`: Core i18n infrastructure — language detection from system settings, fallback logic (Polish default), resource file management, localization service, and XAML markup extension for consuming translations. Covers supported languages: Polish, English, Ukrainian, Russian.

### Modified Capabilities
- `shell-navigation`: Tab titles (Home, Search, Favorites, Profile) must reference localized strings instead of hardcoded English text
- `auth-screens`: All form labels, button text, placeholders, validation messages, and password strength indicators must use localized strings
- `search-ui`: Search placeholders, section headers ("Recent Searches", "Popular Searches"), and search suggestion text must use localized strings

## Impact

- **XAML files affected**: `AppShell.xaml`, `LoginPage.xaml`, `RegisterPage.xaml`, `HomePage.xaml`, `SearchPage.xaml`, `FavoritesPage.xaml`, `ProfilePage.xaml`
- **C# files affected**: `RegisterViewModel.cs` (validation messages, password strength labels), `SearchViewModel.cs` (search suggestions)
- **New files**: Resource files (`AppResources.resx`, `AppResources.pl.resx`, `AppResources.en.resx`, `AppResources.uk.resx`, `AppResources.ru.resx`), `LocalizationService`, XAML markup extension
- **Dependencies**: No new NuGet packages required — uses built-in .NET MAUI localization support (`Microsoft.Extensions.Localization` is included in the MAUI workload)
- **Platform behavior**: System language detection works across all target platforms (Android, iOS, macCatalyst, Windows) via `CultureInfo.CurrentUICulture`
