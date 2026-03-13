## MODIFIED Requirements

### Requirement: Search page with autofocus
The Search page SHALL have a full editable search input that auto-focuses on appearance. It SHALL include a localized placeholder, clear button (visible when text entered), back button, filter button, and AutomationId="SearchEntry". Both the back and clear icons SHALL use a theme-aware tint color: `Secondary` (#39485F) in light theme and `White` (#FFFFFF) in dark theme. **When the user types text, the page SHALL switch from suggestions view to search results view. When text is cleared, it SHALL return to suggestions view.** The search bar row SHALL include a filter icon button (`icon_filter.svg`) to the right of the clear button. The filter icon SHALL use the same theme-aware tint color as other icons. When filters are active, a badge SHALL overlay the filter icon showing the active filter count.

#### Scenario: Search Entry is locatable by AutomationId
- **WHEN** the Search page is displayed
- **THEN** the Search Entry SHALL have `AutomationId="SearchEntry"`

#### Scenario: Search page icons adapt to dark theme
- **WHEN** the Search page is in dark theme
- **THEN** the back, clear, and filter icons SHALL be tinted `#FFFFFF` (White) and be clearly visible against the dark search bar background

#### Scenario: Search page icons adapt to light theme
- **WHEN** the Search page is in light theme
- **THEN** the back, clear, and filter icons SHALL be tinted `#39485F` (Secondary)

#### Scenario: Empty search shows suggestions
- **WHEN** the search text is empty
- **THEN** the Recent Searches section SHALL be visible only if recent searches exist (count > 0)
- **THEN** the Popular Searches section SHALL be visible
- **THEN** the search results CollectionView SHALL be hidden

#### Scenario: Text input triggers search
- **WHEN** the user types text into the search entry
- **THEN** the suggestions SHALL be hidden
- **THEN** the search results CollectionView SHALL be visible
- **THEN** after a 400ms debounce, the app SHALL call `IListingsService.SearchAsync` with `cityId` and `voivodeshipId` integer parameters instead of a `city` string parameter
- **THEN** the search SHALL include any active filter parameters from `SearchFilterState`

#### Scenario: Search with city filter uses city ID
- **WHEN** the user selects a city filter and types a search query
- **THEN** the `SearchAsync` call SHALL pass the selected city's integer ID as `cityId` parameter and the voivodeship's integer ID as `voivodeshipId`, not a city name string

#### Scenario: Search without city filter omits location parameters
- **WHEN** the user searches without selecting a city filter
- **THEN** the `SearchAsync` call SHALL pass `cityId: null` and `voivodeshipId: null`

#### Scenario: Debounce cancels previous request
- **WHEN** the user types additional characters before the 400ms debounce completes
- **THEN** the previous pending search request SHALL be cancelled via CancellationToken
- **THEN** a new 400ms debounce SHALL start

#### Scenario: Clear text returns to suggestions
- **WHEN** the user clears the search text (via clear button or manual deletion)
- **THEN** the search results SHALL be hidden and suggestions SHALL reappear

#### Scenario: Filter button opens filter popup
- **WHEN** the user taps the filter icon button
- **THEN** the filter popup SHALL be displayed

#### Scenario: Filter button shows badge when filters active
- **WHEN** `SearchFilterState.ActiveFilterCount` is greater than 0
- **THEN** a badge with the count SHALL be visible on the filter button

#### Scenario: Search includes active filters
- **WHEN** filters are active (e.g., sport="tennis", priceMin=50) and the user types a query
- **THEN** `SearchAsync` SHALL be called with both the query text AND all active filter parameters

#### Scenario: Filter-only search without text
- **WHEN** filters are applied but search text is empty
- **THEN** `SearchAsync` SHALL be called with `query=null` and the active filter parameters
- **THEN** results SHALL be displayed in the search results CollectionView

## ADDED Requirements

### Requirement: SearchViewModel accepts sport query parameter
`SearchViewModel` SHALL implement `IQueryAttributable` to receive a `sport` query parameter. When received, the sport filter SHALL be pre-populated in `SearchFilterState` and a search SHALL auto-execute.

#### Scenario: Receive sport parameter from Home navigation
- **WHEN** `SearchViewModel` receives query attribute `sport="tennis"`
- **THEN** `SearchFilterState.SelectedSection` SHALL be set to the matching section (loaded from `ISectionsService`)
- **THEN** `SearchAsync` SHALL be called with `sport="tennis"`
- **THEN** the filter badge SHALL show "1"

### Requirement: SearchViewModel injects filter dependencies
`SearchViewModel` SHALL inject `ISectionsService`, `IGeographyService`, and `ILocaleService` in addition to its existing dependencies. These SHALL be used by the filter popup for loading sports, categories, and location autocomplete data.

#### Scenario: All dependencies injectable
- **WHEN** `SearchViewModel` is resolved from DI
- **THEN** it SHALL have access to `ISectionsService`, `IGeographyService`, `ILocaleService`, `IListingsService`, `INavigationService`, `IToastService`, and `IRecentSearchesService`
