## MODIFIED Requirements

### Requirement: HomeViewModel with listings feed
The system SHALL create a `HomeViewModel` with an `ObservableCollection<ListingSummary>` bound to the Home page. It SHALL inject `IListingsService`, `INavigationService`, `IToastService`, `IAuthService`, and `IFavoritesService` via primary constructor. The ViewModel SHALL also expose a `HotPicks` collection (first 6 items) and a `SelectedCondition` observable property for condition filtering.

#### Scenario: Initial load on appearing
- **WHEN** the Home page appears
- **THEN** the `LoadListingsCommand` SHALL execute, fetching the first page of listings (limit=20, offset=0) and populating both the main collection and the `HotPicks` collection (first 6 items)

### Requirement: Home page CollectionView layout
The Home page SHALL use a single `CollectionView` with `GridItemsLayout` (2 columns) for the All Products section. The `CollectionView.Header` SHALL contain the header bar, search bar, category chips, and Hot Picks carousel. The FAB SHALL overlay the page.

#### Scenario: Display listing card
- **WHEN** a `ListingSummary` is in the collection
- **THEN** the card SHALL use the `ListingCardView` control showing placeholder image, condition badge, favorite heart, title, and formatted price

#### Scenario: Empty state
- **WHEN** the listings collection is empty and not loading
- **THEN** an empty state message SHALL be displayed

### Requirement: Pull-to-refresh
The `HomeViewModel` SHALL expose an `IsRefreshing` property and a `RefreshCommand`. The `RefreshView` wrapping the `CollectionView` SHALL bind to these.

#### Scenario: Pull to refresh
- **WHEN** the user pulls down to refresh
- **THEN** the collection SHALL be cleared, offset reset to 0, and the first page re-fetched using the current condition filter
- **THEN** `HotPicks` SHALL be updated with the first 6 items of the new results
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

## ADDED Requirements

### Requirement: Hot Picks collection
The `HomeViewModel` SHALL expose a `HotPicks` observable collection containing up to 6 listings. This collection SHALL be populated from the first 6 items of the main listings load.

#### Scenario: Populate Hot Picks
- **WHEN** listings are loaded (initial or refresh)
- **THEN** `HotPicks` SHALL contain the first `Math.Min(6, listings.Count)` items

#### Scenario: Condition filter changes Hot Picks
- **WHEN** the user selects a different condition filter
- **THEN** `HotPicks` SHALL update to reflect the first 6 items of the filtered results

### Requirement: Condition filter state
The `HomeViewModel` SHALL expose a `SelectedCondition` property (string, nullable). Values: null (ALL), "new", "used". Changing this property SHALL reload listings using `IListingsService.SearchAsync` with the `condition` parameter.

#### Scenario: Select NEW condition
- **WHEN** the user taps the NEW chip
- **THEN** `SelectedCondition` SHALL be set to "new"
- **THEN** listings SHALL reload using `SearchAsync(condition: "new")`

#### Scenario: Select ALL condition
- **WHEN** the user taps the ALL chip
- **THEN** `SelectedCondition` SHALL be set to null
- **THEN** listings SHALL reload using `GetListingsAsync` (no condition filter)

#### Scenario: Select USED condition
- **WHEN** the user taps the USED chip
- **THEN** `SelectedCondition` SHALL be set to "used"
- **THEN** listings SHALL reload using `SearchAsync(condition: "used")`

### Requirement: Total results count
The `HomeViewModel` SHALL expose a `TotalResults` observable property reflecting the total count from the API response.

#### Scenario: Display total
- **WHEN** listings are loaded
- **THEN** `TotalResults` SHALL be set from the API response total count
