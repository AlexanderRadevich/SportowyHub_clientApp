### Requirement: Theme picker with three options
The Theme row in the Settings section SHALL contain a `Picker` control offering three options: Light (`SettingsThemeLight`), Dark (`SettingsThemeDark`), and System (`SettingsThemeSystem`). All option labels SHALL be sourced from `AppResources` localized resources. The default selection SHALL be "System".

#### Scenario: Theme picker displays three options
- **WHEN** the user taps the Theme row
- **THEN** a platform-native picker SHALL display three localized options: Light, Dark, and System

#### Scenario: Theme picker shows current selection
- **WHEN** the profile hub is displayed
- **THEN** the Theme row SHALL display the currently active theme option name on the right side

### Requirement: Theme selection applies immediately
When the user selects a theme option, the app SHALL immediately apply the theme by setting `Application.Current.UserAppTheme` to `AppTheme.Light`, `AppTheme.Dark`, or `AppTheme.Unspecified` (for System). The change SHALL be visible across all pages without requiring a restart or navigation.

#### Scenario: User selects Light theme
- **WHEN** the user selects "Light" from the theme picker
- **THEN** `Application.Current.UserAppTheme` SHALL be set to `AppTheme.Light` and all `AppThemeBinding` values SHALL resolve to their Light variants immediately

#### Scenario: User selects Dark theme
- **WHEN** the user selects "Dark" from the theme picker
- **THEN** `Application.Current.UserAppTheme` SHALL be set to `AppTheme.Dark` and all `AppThemeBinding` values SHALL resolve to their Dark variants immediately

#### Scenario: User selects System theme
- **WHEN** the user selects "System" from the theme picker
- **THEN** `Application.Current.UserAppTheme` SHALL be set to `AppTheme.Unspecified` and the app SHALL follow the device's current system theme

### Requirement: Theme preference persistence
The selected theme SHALL be persisted using `Preferences` with the key `app_theme`. The stored values SHALL be `"light"`, `"dark"`, or `"system"`. The default value SHALL be `"system"`.

#### Scenario: Theme preference is saved on selection
- **WHEN** the user selects a theme option
- **THEN** the selection SHALL be saved to `Preferences` under key `app_theme`

#### Scenario: Theme preference is restored on app launch
- **WHEN** the app starts and `Preferences` contains a stored `app_theme` value
- **THEN** the app SHALL apply the stored theme before any UI is rendered

#### Scenario: No stored theme defaults to system
- **WHEN** the app starts and no `app_theme` value exists in `Preferences`
- **THEN** the app SHALL use `AppTheme.Unspecified` (follow system theme)
