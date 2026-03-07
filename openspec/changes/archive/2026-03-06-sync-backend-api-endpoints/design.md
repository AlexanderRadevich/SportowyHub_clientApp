## Context

The MAUI client's API layer was built against an earlier backend snapshot. The Symfony backend has since shipped FEATURE-134 through FEATURE-140, introducing a geography autocomplete endpoint, profile geography fields, and a listing `condition` field. Critically, the `SectionsService` calls `/api/v1/sections` while the backend route has always been `/api/v1/sports` — this is currently broken at runtime.

Affected layers: model records, service classes, JSON source-gen context, view models, XAML pages, and unit tests.

## Goals / Non-Goals

**Goals:**
- Fix the broken sections endpoint URLs (`/sections` → `/sports`)
- Add `VoivodeshipId` and `CityId` to `UserAccount` and `UpdateProfileAccountRequest`
- Add `Condition` to `ListingDetail`
- Introduce `IGeographyService` with an `AutocompleteAsync` method and matching models
- Wire geography autocomplete into the edit-profile page as a voivodeship/city picker
- Keep JSON serialization working via `SportowyHubJsonContext` source generation

**Non-Goals:**
- Full geography picker UI with Elasticsearch-style type-ahead in listing creation (existing voivodeship/city dropdowns in CreateEditListing remain as-is)
- Replacing legacy `City`/`Region` string fields on `ListingDetail` with FK-based geography (tracked separately in `refactor-049-listing-remove-city-region`)
- Rate-limit handling or retry logic for autocomplete (Polly resilience is a separate concern)
- Admin API endpoints

## Decisions

### D1: Fix sections URLs in-place rather than renaming the service

Change the URL strings in `SectionsService` from `/api/v1/sections` to `/api/v1/sports`. Keep the C# type names (`ISectionsService`, `SectionsService`, `SectionsResponse`, `Section`) unchanged — they match the domain concept of "sport sections" used throughout the app. Renaming every reference to "Sports" would be a large churn with no functional benefit.

**Alternative considered:** Rename to `ISportsService` / `SportsService` — rejected because the app uses "sections" as a higher-level UI concept that groups sports, and renaming would touch dozens of files for no user-visible gain.

### D2: Add geography fields to existing records with default values

Extend `UserAccount` and `UpdateProfileAccountRequest` by appending nullable `int?` properties (`VoivodeshipId`, `CityId`). Use default parameter values so existing call sites continue to compile without changes.

**Alternative considered:** Create new record types — rejected because these are simple additive changes and new types would duplicate 90% of the properties.

### D3: New `IGeographyService` in `Services/Geography/`

Create a new `IGeographyService` interface with a single `AutocompleteAsync(string query, string? locale, int? limit, CancellationToken ct)` method. The implementation calls `GET /api/v1/geography/autocomplete`. Models:

- `GeographyAutocompleteItem` — a discriminated record with `Type` (string: `"voivodeship"`, `"city"`, `"separator"`), `VoivodeshipId` (int?), `CityId` (int?), `Name` (string?), `Label` (string?)
- Response is a plain `List<GeographyAutocompleteItem>` (the backend returns a JSON array, not an envelope)

Register as singleton in DI — the service is stateless.

**Alternative considered:** Add autocomplete to an existing service (e.g., `ListingsService`) — rejected because geography is a distinct domain concern used by both profile and listing flows.

### D4: Edit profile geography as simple pickers (not autocomplete UI)

For the edit-profile page, add two `Picker` controls: voivodeship (populated from existing `GET /api/v1/geography/voivodeships`) and city (populated from `GET /api/v1/geography/cities?voivodeship_id=X`). The autocomplete endpoint is wired for future search-as-you-type, but the initial implementation uses simple pickers for consistency with the create-listing flow.

**Alternative considered:** Full autocomplete SearchBar — rejected as over-engineering for an edit-profile form; the picker approach is consistent with the existing create-listing page.

### D5: Add `Condition` as nullable string to `ListingDetail`

Append `string? Condition` to the `ListingDetail` positional record. This is purely additive and backward-compatible with JSON deserialization (missing field → null).

## Risks / Trade-offs

**[Sections URL fix may break cached data]** → The app does not cache sections responses locally (they're fetched fresh each time), so this is safe.

**[Adding record parameters changes positional constructor]** → All existing call sites use named parameters or deserialization. Unit test factories that use positional constructors will need the new parameters added. Mitigated by using default values.

**[Geography service depends on Elasticsearch availability]** → The autocomplete endpoint requires ES. If ES is down, the endpoint returns 500. The MAUI app should handle HTTP errors gracefully (existing `IRequestProvider` already does).

**[Edit-profile geography pickers add network calls]** → Two extra API calls when the edit-profile page loads. Mitigated by loading them lazily (voivodeships on page load, cities only after voivodeship selection).
