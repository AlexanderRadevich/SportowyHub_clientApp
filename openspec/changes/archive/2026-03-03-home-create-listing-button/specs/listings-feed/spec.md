# Listings Feed

## MODIFIED Requirements

### Requirement: HomeViewModel with listings feed
The system SHALL create a `HomeViewModel` with an `ObservableCollection<ListingSummary>` bound to the Home page. It SHALL inject `IListingsService`, `INavigationService`, `IToastService`, and `IAuthService` via primary constructor.

#### Scenario: Initial load on appearing
- **WHEN** the Home page appears
- **THEN** the `LoadListingsCommand` SHALL execute, fetching the first page of listings (limit=20, offset=0) and populating the collection

### Requirement: Home page CollectionView layout
The Home page SHALL use a `Grid` layout with the `RefreshView`/`CollectionView` for the listings feed and a FAB overlay. The outer `Grid` on the page SHALL use two rows: the search bar (Auto) and a content area (*) containing the feed grid and the FAB.

#### Scenario: Display listing card
- **WHEN** a `ListingSummary` is in the collection
- **THEN** the card SHALL show the title, formatted price (or empty if null), and city

#### Scenario: Empty state
- **WHEN** the listings collection is empty and not loading
- **THEN** an empty state message SHALL be displayed
