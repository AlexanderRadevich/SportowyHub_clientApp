## ADDED Requirements

### Requirement: ListingDetailPage and ViewModel
The system SHALL create a `ListingDetailPage.xaml` and `ListingDetailViewModel` under `Features/` or the existing structure. The ViewModel SHALL use a primary constructor injecting `IListingsService`, `INavigationService`, and `IToastService`.

#### Scenario: Receive listing ID via query parameter
- **WHEN** the page is navigated to with `listing-detail?id=abc-123`
- **THEN** the `ListingDetailViewModel` SHALL implement `IQueryAttributable`, extract the `id`, and invoke `LoadListingCommand`

### Requirement: Load and display listing detail
The ViewModel SHALL have a `LoadListingCommand` that calls `IListingsService.GetListingAsync(id, ct)` and populates observable properties.

#### Scenario: Successful load
- **WHEN** the API returns a `ListingDetail`
- **THEN** the page SHALL display: title, description, price with currency, city, region, status, and published date
- **THEN** `IsLoading` SHALL be false

#### Scenario: Loading state
- **WHEN** `LoadListingCommand` begins execution
- **THEN** `IsLoading` SHALL be true and an `ActivityIndicator` SHALL be visible

#### Scenario: Error state
- **WHEN** the API call fails (network error, 404)
- **THEN** a toast error SHALL be shown and `HasError` SHALL be true

### Requirement: Shell route registration
The route `listing-detail` SHALL be registered in `AppShell.xaml.cs` pointing to `ListingDetailPage`.

#### Scenario: Route is navigable
- **WHEN** any page navigates to `listing-detail?id=abc-123`
- **THEN** Shell SHALL resolve and display the `ListingDetailPage`

### Requirement: DI registration
`ListingDetailPage` and `ListingDetailViewModel` SHALL be registered as transient in `MauiProgram.cs`.

#### Scenario: Transient registration
- **WHEN** navigating to the listing detail page multiple times
- **THEN** each navigation SHALL create a new page and ViewModel instance

### Requirement: Back navigation
The listing detail page SHALL have a back button (Shell default) that navigates back to the previous page.

#### Scenario: Navigate back
- **WHEN** the user taps the back button
- **THEN** the app SHALL navigate back to the listings feed or search results
