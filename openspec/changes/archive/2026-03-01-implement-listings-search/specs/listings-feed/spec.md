## ADDED Requirements

### Requirement: HomeViewModel with listings feed
The system SHALL create a `HomeViewModel` with an `ObservableCollection<ListingSummary>` bound to the Home page. It SHALL inject `IListingsService` and `INavigationService` via primary constructor.

#### Scenario: Initial load on appearing
- **WHEN** the Home page appears
- **THEN** the `LoadListingsCommand` SHALL execute, fetching the first page of listings (limit=20, offset=0) and populating the collection

### Requirement: Pull-to-refresh
The `HomeViewModel` SHALL expose an `IsRefreshing` property and a `RefreshCommand`. The `RefreshView` wrapping the listings `CollectionView` SHALL bind to these.

#### Scenario: Pull to refresh
- **WHEN** the user pulls down to refresh
- **THEN** the collection SHALL be cleared, offset reset to 0, and the first page re-fetched
- **THEN** `IsRefreshing` SHALL be set to false after the load completes (in a finally block)

### Requirement: Incremental loading (infinite scroll)
The `HomeViewModel` SHALL expose a `LoadMoreListingsCommand` bound to `CollectionView.RemainingItemsThresholdReachedCommand` with threshold of 5. It SHALL track a `_hasMoreItems` flag.

#### Scenario: Load next page
- **WHEN** the user scrolls near the bottom (threshold reached) and `_hasMoreItems` is true
- **THEN** the next page SHALL be fetched (offset += limit) and appended to the existing collection

#### Scenario: End of results
- **WHEN** the API returns fewer items than the requested limit
- **THEN** `_hasMoreItems` SHALL be set to false and no further requests SHALL be made

#### Scenario: Prevent duplicate loading
- **WHEN** `LoadMoreListingsCommand` is already executing
- **THEN** a second invocation SHALL not start (guard via `IsLoading` check)

### Requirement: Home page CollectionView layout
The Home page SHALL replace the placeholder "Coming Soon" content with a `RefreshView` containing a `CollectionView`. Each listing item SHALL display title, price with currency, and city in a card-style layout.

#### Scenario: Display listing card
- **WHEN** a `ListingSummary` is in the collection
- **THEN** the card SHALL show the title, formatted price (or empty if null), and city

#### Scenario: Empty state
- **WHEN** the listings collection is empty and not loading
- **THEN** an empty state message SHALL be displayed

### Requirement: Navigate to listing detail from feed
Tapping a listing card SHALL navigate to the listing detail page with the listing ID.

#### Scenario: Tap listing card
- **WHEN** the user taps a listing card
- **THEN** the app SHALL navigate to `listing-detail?id={listing.Id}` via `INavigationService`

### Requirement: Error handling
The `HomeViewModel` SHALL catch exceptions during loading and display an error via `IToastService`.

#### Scenario: Network error on load
- **WHEN** the listings API call throws an exception
- **THEN** a toast error message SHALL be shown and `IsLoading` SHALL be set to false

### Requirement: Search bar tap navigates via ViewModel
The Home page search bar tap SHALL be handled by the `HomeViewModel` via a `GoToSearchCommand` using `INavigationService`, not code-behind with `Shell.Current`.

#### Scenario: Tap search bar
- **WHEN** the user taps the search bar on the Home page
- **THEN** the app SHALL switch to the Search tab
