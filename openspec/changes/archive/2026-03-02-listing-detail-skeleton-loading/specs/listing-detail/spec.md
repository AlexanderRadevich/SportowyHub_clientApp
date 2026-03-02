## MODIFIED Requirements

### Requirement: Listing detail page layout
The listing detail page SHALL display the listing title, price with currency, location (city + region), description, and published date. It SHALL also display a favorite toggle heart button in the toolbar area allowing the user to add or remove the listing from favorites. The page SHALL accept preview data (title, price, currency, city) via navigation parameters and display these fields immediately on arrival, before the full API response is available.

#### Scenario: Display listing details
- **WHEN** a listing detail is loaded successfully
- **THEN** the page SHALL display title, formatted price, location, description, published date, and a favorite heart button

#### Scenario: Loading state
- **WHEN** the listing detail API call is in progress
- **THEN** the title, price, and city from navigation parameters SHALL be displayed immediately in their final positions
- **AND** skeleton placeholders SHALL be shown in place of description and published date
- **AND** no full-screen `ActivityIndicator` SHALL be displayed

#### Scenario: Content transition after load
- **WHEN** the API response arrives successfully
- **THEN** the skeleton placeholders SHALL be replaced with the actual description and published date
- **AND** the title, price, and location SHALL update to reflect the full API data (including region in location)

#### Scenario: Error state
- **WHEN** the listing fails to load
- **THEN** a generic error message SHALL be displayed

## ADDED Requirements

### Requirement: Navigation parameters for preview data
Source screens (Home feed, Search results, Favorites) SHALL pass `title`, `price`, `currency`, and `city` as navigation query parameters alongside `id` when navigating to the listing detail page. The `SearchViewModel` SHALL convert `SearchResultItem.Price` from `float?` to string before passing.

#### Scenario: Navigate from Home feed
- **WHEN** the user taps a listing in the Home feed
- **THEN** the app SHALL navigate to listing detail with `id`, `title`, `price`, `currency`, and `city` from the `ListingSummary`

#### Scenario: Navigate from Search results
- **WHEN** the user taps a listing in Search results
- **THEN** the app SHALL navigate to listing detail with `id`, `title`, `price` (converted from float to string), `currency`, and `city` from the `SearchResultItem`

#### Scenario: Navigate from Favorites
- **WHEN** the user taps a listing in Favorites
- **THEN** the app SHALL navigate to listing detail with `id`, `title`, `price`, `currency`, and `city` from the `FavoriteItem`

#### Scenario: Missing preview parameters
- **WHEN** the listing detail page is opened with only the `id` parameter (no preview data)
- **THEN** the page SHALL fall back to showing skeleton placeholders for all fields until the API responds
