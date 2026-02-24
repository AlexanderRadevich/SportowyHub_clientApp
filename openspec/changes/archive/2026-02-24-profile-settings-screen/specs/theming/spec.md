## MODIFIED Requirements

### Requirement: Automatic light/dark theme switching
The app SHALL support three theme modes: Light, Dark, and System. In System mode, the app SHALL follow the device's system theme preference using `AppThemeBinding`. In Light or Dark mode, the app SHALL override the system preference by setting `Application.Current.UserAppTheme`. The active theme mode SHALL be determined by the user's persisted preference in `Preferences` (key: `app_theme`), defaulting to System (`AppTheme.Unspecified`) if no preference is stored. The theme preference SHALL be applied in the `App` constructor before `InitializeComponent()`.

#### Scenario: Device is in light mode with System theme selected
- **WHEN** the device OS is set to light mode and the user's theme preference is "system"
- **THEN** all `AppThemeBinding` values SHALL resolve to their Light variants

#### Scenario: Device is in dark mode with System theme selected
- **WHEN** the device OS is set to dark mode and the user's theme preference is "system"
- **THEN** all `AppThemeBinding` values SHALL resolve to their Dark variants

#### Scenario: User has selected Light theme override
- **WHEN** the user's theme preference is "light" regardless of device OS setting
- **THEN** `Application.Current.UserAppTheme` SHALL be `AppTheme.Light` and all `AppThemeBinding` values SHALL resolve to their Light variants

#### Scenario: User has selected Dark theme override
- **WHEN** the user's theme preference is "dark" regardless of device OS setting
- **THEN** `Application.Current.UserAppTheme` SHALL be `AppTheme.Dark` and all `AppThemeBinding` values SHALL resolve to their Dark variants

#### Scenario: Theme override applied before UI loads
- **WHEN** the app starts with a stored theme preference
- **THEN** `UserAppTheme` SHALL be set before `InitializeComponent()` in the `App` constructor
