# Search UI

## Purpose

Defines the Search page and search bar behavior, including the search bar on the Home screen, the editable search page, suggestions, search results, incremental loading, and recent searches.

## Requirements

### Requirement: Search bar on Home screen
The Home screen SHALL display a search bar at the top of the page. The search bar SHALL be a styled container (`Border` with `RoundRectangle`) containing a magnifying glass icon on the left and localized placeholder text (`SearchPlaceholder`). It SHALL NOT be an editable input on the Home screen — tapping it navigates to the Search page. The magnifying glass icon SHALL use a theme-aware tint color: `Secondary` (#39485F) in light theme and `White` (#FFFFFF) in dark theme.

#### Scenario: Search bar is visible on Home screen
- **WHEN** the Home screen is displayed
- **THEN** a search bar SHALL be visible at the top with a magnifying glass icon and localized placeholder text

#### Scenario: Search bar light theme styling
- **WHEN** the Home screen is in light theme
- **THEN** the search bar background SHALL be `#F1F3F6`, the icon tint color SHALL be `#39485F`, and the text color SHALL be `#39485F`

#### Scenario: Search bar dark theme styling
- **WHEN** the Home screen is in dark theme
- **THEN** the search bar background SHALL be `#1E1E1E` with a subtle border, the icon tint color SHALL be `#FFFFFF`, and the text color SHALL be `#FFFFFF`

#### Scenario: Tapping the search bar navigates to Search page
- **WHEN** the user taps the search bar on the Home screen
- **THEN** the app SHALL navigate to the dedicated Search page

### Requirement: Search page with autofocus
The Search page SHALL have a full editable search input that auto-focuses on appearance. It SHALL include a localized placeholder, clear button (visible when text entered), back button, and AutomationId="SearchEntry". Both the back and clear icons SHALL use a theme-aware tint color: `Secondary` (#39485F) in light theme and `White` (#FFFFFF) in dark theme. **When the user types text, the page SHALL switch from suggestions view to search results view. When text is cleared, it SHALL return to suggestions view.**

#### Scenario: Search Entry is locatable by AutomationId
- **WHEN** the Search page is displayed
- **THEN** the Search Entry SHALL have `AutomationId="SearchEntry"`

#### Scenario: Search page icons adapt to dark theme
- **WHEN** the Search page is in dark theme
- **THEN** the back and clear icons SHALL be tinted `#FFFFFF` (White) and be clearly visible against the dark search bar background

#### Scenario: Search page icons adapt to light theme
- **WHEN** the Search page is in light theme
- **THEN** the back and clear icons SHALL be tinted `#39485F` (Secondary)

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

### Requirement: Search results display
The SearchViewModel SHALL expose an `ObservableCollection<SearchResultItem>` for search results. The search page SHALL convert each `SearchResultItem` to a `ListingSummary` via a `ToListingSummary()` extension method and display results using the reusable `ListingCardView` control in a 2-column wrapped grid layout (`FlexLayout` with `Wrap="Wrap"`, `Direction="Row"`, `JustifyContent="SpaceBetween"`) inside a `ScrollView`, matching the Home page "All Products" section layout.

Each `ListingCardView` SHALL receive:
- `Listing`: The mapped `ListingSummary` instance
- `TapCommand`: Bound to the SearchViewModel's `GoToListingDetailCommand`
- `HasCondition`: True when the `SearchResultItem.Attributes` list contains a key `"condition"`
- `ConditionText`: The localized condition label ("NEW" or "USED") extracted from attributes
- `ConditionBadgeColor`: Dark/black for "new", orange/amber for "used"

The `ToggleFavoriteCommand` SHALL NOT be bound (favorite toggling is not supported in search results).

#### Scenario: Display search results as card grid
- **WHEN** the search API returns results
- **THEN** each result SHALL be displayed using a `ListingCardView` in a 2-column wrapped grid
- **THEN** the cards SHALL show image placeholder, title, price with currency, and view count — identical to Home page cards

#### Scenario: Condition badge from search attributes
- **WHEN** a search result has an attribute with key `"condition"` and value `"new"`
- **THEN** the card SHALL display a "NEW" badge with dark background

#### Scenario: Condition badge for used items
- **WHEN** a search result has an attribute with key `"condition"` and value `"used"`
- **THEN** the card SHALL display a "USED" badge with orange background

#### Scenario: No condition attribute
- **WHEN** a search result has no `"condition"` attribute
- **THEN** no condition badge SHALL be displayed on the card

#### Scenario: Empty search results
- **WHEN** the search API returns zero results
- **THEN** a "no results found" message SHALL be displayed

#### Scenario: Card tap navigates to detail
- **WHEN** the user taps a search result card
- **THEN** the app SHALL navigate to `listing-detail?id={item.Id}` with preview parameters (title, price, currency, city)

### Requirement: Search results incremental loading
The search page SHALL support incremental loading using scroll-position detection on the wrapping `ScrollView`. When the user scrolls within 200dp of the bottom, the next page SHALL be fetched and appended.

#### Scenario: Load more search results on scroll
- **WHEN** the user scrolls the search results `ScrollView` within 200dp of the bottom
- **THEN** the next page SHALL be fetched (offset += limit) and appended to the results collection

#### Scenario: End of search results
- **WHEN** the offset + items count >= total
- **THEN** no further requests SHALL be made

### Requirement: Navigate to listing detail from search results
Tapping a search result SHALL navigate to the listing detail page.

#### Scenario: Tap search result
- **WHEN** the user taps a search result item
- **THEN** the app SHALL navigate to `listing-detail?id={item.Id}` via `INavigationService`

### Requirement: Search error handling
The SearchViewModel SHALL handle API errors gracefully.

#### Scenario: Search API error
- **WHEN** the search API call fails (network error, 503)
- **THEN** a toast error SHALL be shown via `IToastService`
- **THEN** `IsLoading` SHALL be set to false

### Requirement: Search loading indicator
The SearchViewModel SHALL expose an `IsSearching` property to show a loading indicator while a search is in progress.

#### Scenario: Loading state during search
- **WHEN** a search API call is in progress
- **THEN** `IsSearching` SHALL be true and an `ActivityIndicator` SHALL be visible below the search bar

### Requirement: Recent searches dynamic binding
The `SearchViewModel` SHALL expose `RecentSearches` as an `ObservableCollection<string>` populated from `IRecentSearchesService.GetAll()` on page appearance, replacing the hardcoded placeholder data.

#### Scenario: Load recent searches on page appearance
- **WHEN** the search page appears
- **THEN** the `RecentSearches` collection SHALL be refreshed from `IRecentSearchesService`

#### Scenario: Update after search execution
- **WHEN** a search completes successfully
- **THEN** `RecentSearches` SHALL be refreshed to reflect the newly added query

### Requirement: Clear recent searches from UI
The search page SHALL display a localized "Clear" action (`SearchClearRecent`) next to the "Recent Searches" section header. Tapping it SHALL clear all recent searches and hide the section.

#### Scenario: Tap clear action
- **WHEN** the user taps the "Clear" action
- **THEN** `IRecentSearchesService.Clear()` SHALL be called
- **THEN** the `RecentSearches` collection SHALL become empty
- **THEN** the recent searches section SHALL be hidden

### Requirement: Hide empty recent searches section
The recent searches section SHALL be hidden entirely when no recent searches exist.

#### Scenario: No recent searches
- **WHEN** the `RecentSearches` collection is empty
- **THEN** the entire recent searches section (header + list) SHALL not be visible

#### Scenario: Recent searches exist
- **WHEN** the `RecentSearches` collection has one or more items
- **THEN** the recent searches section SHALL be visible

### Requirement: SearchResultItem to ListingSummary mapping
The system SHALL provide a `ToListingSummary()` extension method on `SearchResultItem` that maps fields as follows:
- `Id` → `Id`
- `Title` → `Title`
- `Price` (float?) → `Price` (decimal?) via decimal conversion
- `Currency` → `Currency`
- `City` → `City`
- `CategoryId` (string) → `CategoryId` (int) via `int.Parse`
- `ContentLocale` → `null`
- `PublishedAt` → `PublishedAt`
- `ViewCount` → `ViewCount`

The `Slug` field SHALL map to `Slug` on `ListingSummary`.

#### Scenario: Map search result to listing summary
- **WHEN** a `SearchResultItem` with Id="abc", Title="Ball", Price=29.99, Currency="PLN", City="Warsaw", CategoryId="5", ViewCount=150 is mapped
- **THEN** the resulting `ListingSummary` SHALL have matching field values with Price as `29.99m` (decimal) and CategoryId as `5` (int)

#### Scenario: Map with null optional fields
- **WHEN** a `SearchResultItem` has null Price, Currency, and City
- **THEN** the resulting `ListingSummary` SHALL have null Price, Currency, and City

### Requirement: Condition extraction from search attributes
The system SHALL extract condition information from `SearchResultItem.Attributes` for display on the card. The extraction logic SHALL:
- Find an attribute with `Key == "condition"`
- Map value `"new"` to localized "NEW" text with dark badge color
- Map value `"used"` to localized "USED" text with orange badge color
- Return no condition when the attribute is absent

#### Scenario: Extract new condition
- **WHEN** `SearchResultItem.Attributes` contains `{ Key: "condition", Value: "new" }`
- **THEN** `HasCondition` SHALL be true, `ConditionText` SHALL be the localized "NEW" label, and `ConditionBadgeColor` SHALL be dark/black

#### Scenario: Extract used condition
- **WHEN** `SearchResultItem.Attributes` contains `{ Key: "condition", Value: "used" }`
- **THEN** `HasCondition` SHALL be true, `ConditionText` SHALL be the localized "USED" label, and `ConditionBadgeColor` SHALL be orange/amber

#### Scenario: No condition attribute present
- **WHEN** `SearchResultItem.Attributes` is null or contains no `"condition"` key
- **THEN** `HasCondition` SHALL be false
