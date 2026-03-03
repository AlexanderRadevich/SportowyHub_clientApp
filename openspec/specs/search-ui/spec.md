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
The SearchViewModel SHALL expose an `ObservableCollection<SearchResultItem>` for search results and bind it to a `CollectionView` on the search page.

#### Scenario: Display search results
- **WHEN** the search API returns results
- **THEN** each result SHALL display: title, price with currency, and city in a card layout
- **THEN** the total result count SHALL be accessible via a `TotalResults` property

#### Scenario: Empty search results
- **WHEN** the search API returns zero results
- **THEN** a "no results found" message SHALL be displayed

### Requirement: Search results incremental loading
The SearchViewModel SHALL support incremental loading for search results using the same `RemainingItemsThreshold` pattern as the Home feed.

#### Scenario: Load more search results
- **WHEN** the user scrolls near the bottom of search results
- **THEN** the next page SHALL be fetched (offset += limit) and appended

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
