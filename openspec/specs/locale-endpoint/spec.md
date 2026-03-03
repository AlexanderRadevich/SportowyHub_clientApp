## Purpose

Defines the service interface and model for fetching available locale information from the public API, supporting multi-language configuration in the app.

## Requirements

### Requirement: ILocaleService interface
The app SHALL define an `ILocaleService` interface registered as singleton in DI with method:
- `GetLocaleInfoAsync(CancellationToken ct)` returning `Task<LocaleInfo>`

#### Scenario: Service is injectable
- **WHEN** `ILocaleService` is resolved from DI
- **THEN** a singleton `LocaleService` instance SHALL be returned

### Requirement: Get locale info
`GetLocaleInfoAsync` SHALL call `GET /api/v1/locale`. No auth required.

#### Scenario: Fetch available locales
- **WHEN** `GetLocaleInfoAsync()` is called
- **THEN** it SHALL return `LocaleInfo` with the current locale, available locales list, and default locale

### Requirement: LocaleInfo model
The app SHALL define a `LocaleInfo` record with: `Locale` (string), `AvailableLocales` (List&lt;string&gt;), `DefaultLocale` (string). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize locale info
- **WHEN** the API returns `{"locale":"en","available_locales":["en","pl","ru","uk"],"default_locale":"en"}`
- **THEN** it SHALL deserialize to `LocaleInfo` with AvailableLocales containing 4 items
