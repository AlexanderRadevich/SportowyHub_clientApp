## Purpose

Defines the service interface and model for synchronizing the user's theme preferences (mode and color scheme) with the server-side private API.

## Requirements

### Requirement: IThemeSyncService interface
The app SHALL define an `IThemeSyncService` interface registered as singleton in DI with methods:
- `GetPreferencesAsync(CancellationToken ct)` returning `Task<ThemePreferences>`
- `UpdatePreferencesAsync(ThemePreferences preferences, CancellationToken ct)` returning `Task<ThemePreferences>`

#### Scenario: Service is injectable
- **WHEN** `IThemeSyncService` is resolved from DI
- **THEN** a singleton `ThemeSyncService` instance SHALL be returned

### Requirement: Get theme preferences
`GetPreferencesAsync` SHALL call `GET /api/private/theme/preferences` with Bearer auth.

#### Scenario: Fetch server theme preferences
- **WHEN** `GetPreferencesAsync()` is called
- **THEN** it SHALL return `ThemePreferences` with `ThemeMode` and `ColorScheme`

### Requirement: Update theme preferences
`UpdatePreferencesAsync` SHALL PATCH to `/api/private/theme/preferences` with Bearer auth and the preferences body.

#### Scenario: Update to dark mode
- **WHEN** `UpdatePreferencesAsync(new ThemePreferences("dark", "default"))` is called
- **THEN** it SHALL PATCH `{"theme_mode":"dark","color_scheme":"default"}` and return the updated preferences

### Requirement: ThemePreferences model
The app SHALL define a `ThemePreferences` record with: `ThemeMode` (string), `ColorScheme` (string). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize theme preferences
- **WHEN** the API returns `{"theme_mode":"light","color_scheme":"default"}`
- **THEN** it SHALL deserialize to `ThemePreferences` with ThemeMode="light" and ColorScheme="default"

#### Scenario: Serialize theme preferences
- **WHEN** `ThemePreferences("dark","default")` is serialized
- **THEN** the JSON SHALL be `{"theme_mode":"dark","color_scheme":"default"}`
