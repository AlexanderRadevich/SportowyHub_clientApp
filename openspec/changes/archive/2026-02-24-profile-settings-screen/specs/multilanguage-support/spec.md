## MODIFIED Requirements

### Requirement: Automatic language detection from system settings
The app SHALL support two language modes: a user-selected language override and system-auto detection. On startup, the app SHALL read `Preferences.Get("app_language", "system")`. If the value is a specific culture code (`"pl"`, `"en"`, `"uk"`, `"ru"`), the app SHALL set `CultureInfo.CurrentUICulture` and `CultureInfo.CurrentCulture` to that culture. If the value is `"system"`, the app SHALL detect the device's current UI culture via `CultureInfo.CurrentUICulture`; if the detected language matches a supported language (pl, en, uk, ru), it SHALL use that language; otherwise it SHALL fall back to Polish (`pl`). This logic SHALL execute in the `App` constructor before `InitializeComponent()`.

#### Scenario: Stored language preference overrides system detection
- **WHEN** the app starts and `Preferences` contains `app_language` = `"en"`
- **THEN** the app SHALL set `CultureInfo.CurrentUICulture` to English (`en`) regardless of the device language

#### Scenario: System language mode with supported device language
- **WHEN** the app starts and `app_language` is `"system"` and the device language is English (en)
- **THEN** the app SHALL display all UI strings in English

#### Scenario: System language mode with unsupported device language
- **WHEN** the app starts and `app_language` is `"system"` and the device language is not pl, en, uk, or ru
- **THEN** the app SHALL fall back to Polish and display all UI strings in Polish

#### Scenario: No stored preference defaults to system detection
- **WHEN** the app starts and no `app_language` key exists in `Preferences`
- **THEN** the app SHALL behave identically to `"system"` mode (detect device language, fall back to Polish)

### Requirement: Resource key naming convention
All resource keys SHALL use PascalCase with a feature prefix. Tab-related keys SHALL use the `Tab` prefix. Auth-related keys SHALL use the `Auth` prefix. Password strength keys SHALL use the `PasswordStrength` prefix. Search-related keys SHALL use the `Search` prefix. Home-related keys SHALL use the `Home` prefix. Favorites-related keys SHALL use the `Favorites` prefix. Profile-related keys SHALL use the `Profile` prefix. Settings-related keys SHALL use the `Settings` prefix.

#### Scenario: Resource keys follow naming convention
- **WHEN** a new localizable string is added
- **THEN** its resource key SHALL follow the pattern `{FeaturePrefix}{DescriptiveName}` in PascalCase (e.g., `SettingsLanguage`, `SettingsThemeDark`, `ProfileAccountSection`)
