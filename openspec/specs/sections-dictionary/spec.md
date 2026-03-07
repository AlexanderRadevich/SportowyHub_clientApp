## Purpose

Defines the service interface and models for fetching the sports sections and categories dictionary from the public API, used for listing category selection.

## Requirements

### Requirement: ISectionsService interface
The app SHALL define an `ISectionsService` interface registered as singleton in DI with methods:
- `GetSectionsAsync(string? locale, CancellationToken ct)` returning `Task<SectionsResponse>`
- `GetCategoriesAsync(int sectionId, string? locale, CancellationToken ct)` returning `Task<CategoriesResponse>`

#### Scenario: Service is injectable
- **WHEN** `ISectionsService` is resolved from DI
- **THEN** a singleton `SectionsService` instance SHALL be returned

### Requirement: Get sections (sports)
`GetSectionsAsync` SHALL call `GET /api/v1/sports` with optional `locale` query parameter. No auth required.

#### Scenario: Fetch all sections
- **WHEN** `GetSectionsAsync(locale: "en")` is called
- **THEN** it SHALL call `GET /api/v1/sports?locale=en` and return `SectionsResponse` with a list of sports sections

#### Scenario: Fetch sections without locale
- **WHEN** `GetSectionsAsync(locale: null)` is called
- **THEN** it SHALL call `GET /api/v1/sports` without a locale parameter

### Requirement: Get categories for section
`GetCategoriesAsync` SHALL call `GET /api/v1/sports/{id}/categories` with optional `locale` query parameter. No auth required.

#### Scenario: Fetch categories for tennis
- **WHEN** `GetCategoriesAsync(sectionId: 1, locale: "en")` is called
- **THEN** it SHALL call `GET /api/v1/sports/1/categories?locale=en` and return `CategoriesResponse`

### Requirement: Section model
The app SHALL define a `Section` record with: `Id` (int), `Slug` (string), `Name` (string). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize section
- **WHEN** the API returns `{"id":1,"slug":"tennis","name":"Tennis"}`
- **THEN** it SHALL deserialize to `Section` with all fields mapped

### Requirement: SectionsResponse model
The app SHALL define a `SectionsResponse` record with: `Sections` (List&lt;Section&gt;), `Locale` (string?). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize sections list
- **WHEN** the API returns a sections list JSON
- **THEN** it SHALL deserialize to `SectionsResponse` with sections and locale

### Requirement: Category model
The app SHALL define a `Category` record with: `Id` (int), `Slug` (string), `Name` (string), `SectionId` (int). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize category
- **WHEN** the API returns `{"id":1,"slug":"rackets","name":"Tennis Rackets","section_id":1}`
- **THEN** it SHALL deserialize to `Category` with SectionId=1

### Requirement: CategoriesResponse model
The app SHALL define a `CategoriesResponse` record with: `Categories` (List&lt;Category&gt;), `Locale` (string?). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize categories list
- **WHEN** the API returns a categories list JSON
- **THEN** it SHALL deserialize to `CategoriesResponse` with categories and locale
