## MODIFIED Requirements

### Requirement: Search page with autofocus
The Search page SHALL have a full editable search input that auto-focuses on appearance. It SHALL include a localized placeholder, clear button (visible when text entered), back button, and AutomationId="SearchEntry". **When the user types text, the page SHALL switch from suggestions view to search results view. When text is cleared, it SHALL return to suggestions view.**

#### Scenario: Empty search shows suggestions
- **WHEN** the search text is empty
- **THEN** the Recent Searches and Popular Searches sections SHALL be visible
- **THEN** the search results CollectionView SHALL be hidden

#### Scenario: Text input triggers search
- **WHEN** the user types text into the search entry
- **THEN** the suggestions SHALL be hidden
- **THEN** the search results CollectionView SHALL be visible
- **THEN** after a 400ms debounce, the app SHALL call `/api/v1/search?q={text}&limit=30&offset=0` via `IListingsService.SearchAsync`

#### Scenario: Debounce cancels previous request
- **WHEN** the user types additional characters before the 400ms debounce completes
- **THEN** the previous pending search request SHALL be cancelled via CancellationToken
- **THEN** a new 400ms debounce SHALL start

#### Scenario: Clear text returns to suggestions
- **WHEN** the user clears the search text (via clear button or manual deletion)
- **THEN** the search results SHALL be hidden and suggestions SHALL reappear

## ADDED Requirements

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
