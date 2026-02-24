## ADDED Requirements

### Requirement: Language picker with five options
The Language row in the Settings section SHALL contain a `Picker` control offering five options: System (`SettingsLanguageSystem`), Polski (`SettingsLanguagePl`), English (`SettingsLanguageEn`), Українська (`SettingsLanguageUk`), and Русский (`SettingsLanguageRu`). All option labels SHALL be sourced from `AppResources` localized resources. The default selection SHALL be "System".

#### Scenario: Language picker displays five options
- **WHEN** the user taps the Language row
- **THEN** a platform-native picker SHALL display five localized options: System, Polski, English, Українська, Русский

#### Scenario: Language picker shows current selection
- **WHEN** the profile hub is displayed
- **THEN** the Language row SHALL display the currently active language option name on the right side

### Requirement: Language selection applies immediately without restart
When the user selects a language, the app SHALL:
1. Set `CultureInfo.CurrentUICulture` and `CultureInfo.CurrentCulture` to the selected culture
2. Save the selection to `Preferences`
3. Recreate the app shell by assigning a new `AppShell` instance to the current window's `Page` property

This SHALL cause all pages to re-render with the new language strings.

#### Scenario: User selects Polski
- **WHEN** the user selects "Polski" from the language picker
- **THEN** the app SHALL switch to Polish (`pl`) culture and reload all UI strings in Polish

#### Scenario: User selects English
- **WHEN** the user selects "English" from the language picker
- **THEN** the app SHALL switch to English (`en`) culture and reload all UI strings in English

#### Scenario: User selects Українська
- **WHEN** the user selects "Українська" from the language picker
- **THEN** the app SHALL switch to Ukrainian (`uk`) culture and reload all UI strings in Ukrainian

#### Scenario: User selects Русский
- **WHEN** the user selects "Русский" from the language picker
- **THEN** the app SHALL switch to Russian (`ru`) culture and reload all UI strings in Russian

#### Scenario: User selects System
- **WHEN** the user selects "System" from the language picker
- **THEN** the app SHALL detect the device language and apply the matching supported language, or fall back to Polish if unsupported

### Requirement: Language preference persistence
The selected language SHALL be persisted using `Preferences` with the key `app_language`. The stored values SHALL be `"system"`, `"pl"`, `"en"`, `"uk"`, or `"ru"`. The default value SHALL be `"system"`.

#### Scenario: Language preference is saved on selection
- **WHEN** the user selects a language option
- **THEN** the selection SHALL be saved to `Preferences` under key `app_language`

#### Scenario: Language preference is restored on app launch
- **WHEN** the app starts and `Preferences` contains a stored `app_language` value other than `"system"`
- **THEN** the app SHALL set `CultureInfo.CurrentUICulture` to the stored culture before any UI is rendered

#### Scenario: No stored language defaults to system detection
- **WHEN** the app starts and no `app_language` value exists in `Preferences` (or value is `"system"`)
- **THEN** the app SHALL use the existing system language detection logic (detect device language, fall back to Polish if unsupported)

### Requirement: Language resource keys for picker options
The following resource keys SHALL exist in all `.resx` files: `SettingsLanguageSystem`, `SettingsLanguagePl`, `SettingsLanguageEn`, `SettingsLanguageUk`, `SettingsLanguageRu`. Each key SHALL contain the language name as displayed to the user.

#### Scenario: Language option labels are localized
- **WHEN** the language picker is displayed
- **THEN** all option labels SHALL be sourced from `AppResources` localized resources
