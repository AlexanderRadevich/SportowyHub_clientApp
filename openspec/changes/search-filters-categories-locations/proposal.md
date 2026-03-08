## Why

The search page currently supports only free-text queries. The backend already exposes filtering by sport, category, voivodeship, city, price range, and sort order via `GET /api/v1/search`, and the client's `IListingsService.SearchAsync` already accepts all these parameters — but the UI never passes them. Users cannot browse listings by category or narrow results by location, making discovery inefficient.

## What Changes

- Add a filter bar/sheet to the Search page that exposes: sport (section) picker, category picker (dependent on sport), location picker (voivodeship + city via autocomplete), price range inputs, sort order selector, and condition (new/used) toggle
- Wire selected filter values through to `IListingsService.SearchAsync` parameters that are already implemented but unused
- Add filter state management in `SearchViewModel` with active filter count badge
- Add a "Browse by category" section on the Home page showing sport sections as tappable cards that navigate to search pre-filtered by sport

## Capabilities

### New Capabilities
- `search-filters`: Filter UI panel for the Search page — sport, category, location, price range, sort, condition filters with apply/reset actions
- `home-category-browse`: Category browsing section on the Home page showing sport sections as navigation entry points to filtered search

### Modified Capabilities
- `search-ui`: Search page gains filter bar trigger, active filter indicator, and passes filter parameters to SearchAsync
- `sections-dictionary`: ISectionsService is now consumed by search filtering in addition to listing creation (no API change, usage expansion)
- `geography-autocomplete`: IGeographyService is now consumed by search location filter in addition to profile editing (no API change, usage expansion)

## Impact

- **ViewModels**: `SearchViewModel` (add filter state + wiring), `HomeViewModel` (add sections loading + browse navigation)
- **Views**: `SearchPage.xaml` (filter bar + sheet), `HomePage.xaml` (category browse section)
- **Services**: No new services needed — `ISectionsService`, `IGeographyService`, `IListingsService` already have the required methods
- **Models**: No new API models needed — `Section`, `Category`, `GeographyAutocompleteItem`, `SearchResponse` already exist
- **Localization**: New resource strings for filter labels, placeholders, and actions across 4 locales (pl, en, uk, ru)
