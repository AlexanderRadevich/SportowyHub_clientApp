# Geography Autocomplete

## Purpose

TBD — Defines the service interface and models for geography autocomplete lookups (voivodeships, cities) from the public API.

## Requirements

### Requirement: IGeographyService interface
The app SHALL define an `IGeographyService` interface in `Services/Geography/` registered as singleton in DI with method:
- `AutocompleteAsync(string query, string? locale, int? limit, CancellationToken ct)` returning `Task<List<GeographyAutocompleteItem>>`

#### Scenario: Service is injectable
- **WHEN** `IGeographyService` is resolved from DI
- **THEN** a singleton `GeographyService` instance SHALL be returned

### Requirement: GeographyService autocomplete implementation
`GeographyService` SHALL call `GET /api/v1/geography/autocomplete` with query parameters `q` (required), `locale` (optional), and `limit` (optional). No auth required. The response is a JSON array (not an envelope).

#### Scenario: Autocomplete with all parameters
- **WHEN** `AutocompleteAsync(query: "War", locale: "pl", limit: 10, ct)` is called
- **THEN** it SHALL call `GET /api/v1/geography/autocomplete?q=War&locale=pl&limit=10`

#### Scenario: Autocomplete with required parameter only
- **WHEN** `AutocompleteAsync(query: "Maz", locale: null, limit: null, ct)` is called
- **THEN** it SHALL call `GET /api/v1/geography/autocomplete?q=Maz`

#### Scenario: Empty query returns empty list
- **WHEN** `AutocompleteAsync(query: "", ...)` is called
- **THEN** it SHALL return an empty list without making an API call

### Requirement: GeographyAutocompleteItem model
The app SHALL define a `GeographyAutocompleteItem` record with properties: `Type` (string — `"voivodeship"`, `"city"`, or `"separator"`), `VoivodeshipId` (int?), `CityId` (int?), `Name` (string?), `Label` (string?). The type SHALL be registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize voivodeship item
- **WHEN** the API returns `{"type":"voivodeship","voivodeship_id":7,"name":"Mazowieckie"}`
- **THEN** it SHALL deserialize to `GeographyAutocompleteItem` with Type="voivodeship", VoivodeshipId=7, Name="Mazowieckie", CityId=null, Label=null

#### Scenario: Deserialize city item
- **WHEN** the API returns `{"type":"city","voivodeship_id":7,"city_id":42,"label":"Mazowieckie -> Warszawa"}`
- **THEN** it SHALL deserialize to `GeographyAutocompleteItem` with Type="city", VoivodeshipId=7, CityId=42, Label="Mazowieckie -> Warszawa", Name=null

#### Scenario: Deserialize separator item
- **WHEN** the API returns `{"type":"separator"}`
- **THEN** it SHALL deserialize to `GeographyAutocompleteItem` with Type="separator" and all other fields null
