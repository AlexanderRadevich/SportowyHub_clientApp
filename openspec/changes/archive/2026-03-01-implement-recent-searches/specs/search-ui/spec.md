## MODIFIED Requirements

### Requirement: Search page with autofocus
The Search page SHALL have a full editable search input that auto-focuses on appearance. It SHALL include a localized placeholder, clear button (visible when text entered), back button, and AutomationId="SearchEntry". **When the user types text, the page SHALL switch from suggestions view to search results view. When text is cleared, it SHALL return to suggestions view.**

#### Scenario: Empty search shows suggestions
- **WHEN** the search text is empty
- **THEN** the Recent Searches section SHALL be visible only if recent searches exist (count > 0)
- **THEN** the Popular Searches section SHALL be visible
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
