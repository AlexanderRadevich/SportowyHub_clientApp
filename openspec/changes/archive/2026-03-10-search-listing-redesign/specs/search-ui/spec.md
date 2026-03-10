## MODIFIED Requirements

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

## ADDED Requirements

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
