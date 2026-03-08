## ADDED Requirements

### Requirement: Search filter popup
The Search page SHALL provide a `Popup` (CommunityToolkit.Maui) displayed as a bottom sheet containing all filter controls. The popup SHALL be opened by tapping a filter icon button in the search bar row. The popup SHALL contain a scrollable layout with: sport picker, category picker, location autocomplete, price range inputs, condition chips, and sort picker. The popup SHALL have "Apply" and "Reset" action buttons at the bottom.

#### Scenario: Open filter popup
- **WHEN** the user taps the filter icon button in the search bar
- **THEN** a `Popup` SHALL be displayed with all filter controls
- **THEN** if filters were previously set, the controls SHALL reflect the current filter state

#### Scenario: Close filter popup without applying
- **WHEN** the user taps outside the popup or swipes it away
- **THEN** the popup SHALL close without changing the active filters
- **THEN** the search results SHALL remain unchanged

### Requirement: Sport (section) filter
The filter popup SHALL display a `Picker` bound to available sports loaded from `ISectionsService.GetSectionsAsync`. The locale SHALL be determined by `ILocaleService`. The picker SHALL show section names and store the selected `Section` object.

#### Scenario: Load sports on popup open
- **WHEN** the filter popup is opened
- **THEN** `ISectionsService.GetSectionsAsync` SHALL be called with the current app locale
- **THEN** the sport picker SHALL be populated with the returned sections

#### Scenario: Select a sport
- **WHEN** the user selects a sport from the picker
- **THEN** the selected sport's `Slug` SHALL be stored for the `sport` search parameter
- **THEN** the category picker SHALL become enabled and load categories for the selected sport

#### Scenario: Clear sport selection
- **WHEN** the user clears the sport picker (selects the empty/placeholder option)
- **THEN** the `sport` filter SHALL be cleared
- **THEN** the category picker SHALL be disabled and cleared

### Requirement: Category filter (cascading)
The filter popup SHALL display a `Picker` for categories that is disabled until a sport is selected. When a sport is selected, categories SHALL be loaded via `ISectionsService.GetCategoriesAsync(sectionId, locale)`. The picker SHALL show category names and store the selected `Category.Id`.

#### Scenario: Load categories for selected sport
- **WHEN** the user selects sport with ID 3
- **THEN** `ISectionsService.GetCategoriesAsync(3, locale)` SHALL be called
- **THEN** the category picker SHALL be populated with the returned categories

#### Scenario: Category picker disabled without sport
- **WHEN** no sport is selected
- **THEN** the category picker SHALL be disabled with a placeholder indicating "Select sport first"

#### Scenario: Sport change resets category
- **WHEN** the user changes the selected sport
- **THEN** the previously selected category SHALL be cleared
- **THEN** new categories SHALL be loaded for the new sport

### Requirement: Location filter via autocomplete
The filter popup SHALL display an `Entry` for location search that calls `IGeographyService.AutocompleteAsync` with debounce. Results SHALL appear in a list below the entry. Selecting a result SHALL store `voivodeshipId` and optionally `cityId`, and display the selected location as a label. The autocomplete locale SHALL be determined by `ILocaleService`.

#### Scenario: Type location query
- **WHEN** the user types at least 2 characters in the location entry
- **THEN** after a 300ms debounce, `IGeographyService.AutocompleteAsync` SHALL be called
- **THEN** results SHALL be displayed in a list showing voivodeships and cities

#### Scenario: Select a city result
- **WHEN** the user taps a city autocomplete result with `voivodeshipId=7` and `cityId=42`
- **THEN** `voivodeshipId` SHALL be stored as 7 and `cityId` as 42
- **THEN** the autocomplete list SHALL be hidden
- **THEN** the selected location label SHALL display the city's label text

#### Scenario: Select a voivodeship result
- **WHEN** the user taps a voivodeship autocomplete result with `voivodeshipId=7`
- **THEN** `voivodeshipId` SHALL be stored as 7 and `cityId` SHALL be null
- **THEN** the selected location label SHALL display the voivodeship name

#### Scenario: Clear location
- **WHEN** the user taps the clear button next to the selected location label
- **THEN** `voivodeshipId` and `cityId` SHALL be reset to null
- **THEN** the location entry SHALL be cleared and ready for new input

#### Scenario: Separator items are not selectable
- **WHEN** autocomplete results contain a `type=separator` item
- **THEN** it SHALL be rendered as a visual divider and SHALL NOT be tappable

### Requirement: Price range filter
The filter popup SHALL display two `Entry` fields for minimum and maximum price. Both SHALL accept numeric input only and bind to `float?` filter properties.

#### Scenario: Set price range
- **WHEN** the user enters 100 in "Price from" and 500 in "Price to"
- **THEN** `priceMin` SHALL be stored as 100 and `priceMax` as 500

#### Scenario: Partial price range
- **WHEN** the user enters only "Price from" as 50 and leaves "Price to" empty
- **THEN** `priceMin` SHALL be 50 and `priceMax` SHALL be null

### Requirement: Condition filter
The filter popup SHALL display two selectable chips for condition: "New" and "Used". Only one chip MAY be selected at a time, or neither (no filter). The chip labels SHALL be localized.

#### Scenario: Select condition
- **WHEN** the user taps the "New" chip
- **THEN** the `condition` filter SHALL be set to `"new"`
- **THEN** the "New" chip SHALL appear selected and "Used" SHALL appear unselected

#### Scenario: Deselect condition
- **WHEN** the user taps the already-selected "New" chip
- **THEN** the `condition` filter SHALL be cleared to null
- **THEN** both chips SHALL appear unselected

### Requirement: Sort order filter
The filter popup SHALL display a `Picker` with sort options: "Newest" (default), "Price: Low to High", "Price: High to Low". The picker labels SHALL be localized. The values SHALL map to `sort` parameter values: `"newest"`, `"price_asc"`, `"price_desc"`.

#### Scenario: Select sort order
- **WHEN** the user selects "Price: Low to High"
- **THEN** the `sort` filter SHALL be set to `"price_asc"`

#### Scenario: Default sort
- **WHEN** no sort option is selected
- **THEN** the `sort` parameter SHALL be null (backend defaults to newest)

### Requirement: Apply filters
When the user taps the "Apply" button, the popup SHALL close and return the filter state. The `SearchViewModel` SHALL re-execute the search with all active filter parameters passed to `IListingsService.SearchAsync`.

#### Scenario: Apply filters triggers search
- **WHEN** the user sets sport="tennis", cityId=42, priceMin=100 and taps "Apply"
- **THEN** the popup SHALL close
- **THEN** `SearchAsync` SHALL be called with `sport="tennis"`, `cityId=42`, `priceMin=100` and the current search text (if any)
- **THEN** search results SHALL be cleared and reloaded with the new filters

#### Scenario: Apply filters with empty search text
- **WHEN** the user sets filters but has no search text and taps "Apply"
- **THEN** `SearchAsync` SHALL be called with filters and `query=null`
- **THEN** filtered results SHALL be displayed

### Requirement: Reset filters
When the user taps the "Reset" button, all filter controls SHALL be cleared to their default (empty) state. The reset SHALL NOT close the popup — the user must tap "Apply" to execute the cleared search.

#### Scenario: Reset all filters
- **WHEN** the user taps "Reset"
- **THEN** sport, category, location, price range, condition, and sort SHALL all be cleared
- **THEN** the popup SHALL remain open with all controls in their default state

### Requirement: SearchFilterState class
The app SHALL define a `SearchFilterState` partial class extending `ObservableObject` with properties: `SelectedSection` (Section?), `SelectedCategoryId` (int?), `SelectedVoivodeshipId` (int?), `SelectedCityId` (int?), `SelectedLocationLabel` (string?), `PriceMin` (float?), `PriceMax` (float?), `Condition` (string?), `Sort` (string?). It SHALL expose an `ActiveFilterCount` computed property returning the number of non-null filters. It SHALL expose a `Reset()` method that clears all properties.

#### Scenario: Active filter count with 3 filters
- **WHEN** sport, cityId, and priceMin are set
- **THEN** `ActiveFilterCount` SHALL return 3

#### Scenario: Reset clears all
- **WHEN** `Reset()` is called
- **THEN** all filter properties SHALL be null and `ActiveFilterCount` SHALL return 0

### Requirement: Filter button active indicator
The filter icon button in the search bar SHALL display a badge showing the `ActiveFilterCount` when filters are active (count > 0). When no filters are active, the badge SHALL be hidden.

#### Scenario: No active filters
- **WHEN** `ActiveFilterCount` is 0
- **THEN** the filter button SHALL show only the filter icon without a badge

#### Scenario: Active filters
- **WHEN** `ActiveFilterCount` is 3
- **THEN** the filter button SHALL display a badge with "3" overlaid on the filter icon

### Requirement: Filters persist during search session
Filter state SHALL be retained in `SearchViewModel` for the duration of the page's lifetime. Navigating away and back (if the ViewModel is re-created) SHALL reset filters.

#### Scenario: Filters retained across searches
- **WHEN** the user applies filters and then changes the search text
- **THEN** the new search SHALL include the previously applied filters
- **THEN** the filter badge count SHALL remain the same
