## MODIFIED Requirements

### Requirement: ListingSummary record fields
The `ListingSummary` record SHALL define `Price` as `decimal?` with `[JsonConverter(typeof(FlexibleDecimalConverter))]` attribute.

#### Scenario: Deserialize listing summary with numeric price
- **WHEN** the JSON response from `GET /api/v1/listings` contains `"price": 150.0`
- **THEN** `ListingSummary.Price` SHALL be `150.0m`

#### Scenario: Deserialize listing summary with null price
- **WHEN** the JSON response contains `"price": null`
- **THEN** `ListingSummary.Price` SHALL be `null`

### Requirement: ListingDetail record fields
The `ListingDetail` record SHALL define `Price` as `decimal?` with `[JsonConverter(typeof(FlexibleDecimalConverter))]` attribute.

#### Scenario: Deserialize listing detail with string price
- **WHEN** the JSON response from `GET /api/v1/listings/{id}` contains `"price": "150.00"`
- **THEN** `ListingDetail.Price` SHALL be `150.00m`

#### Scenario: Deserialize listing detail with null price
- **WHEN** the JSON response contains `"price": null`
- **THEN** `ListingDetail.Price` SHALL be `null`

### Requirement: FavoriteItem record fields
The `FavoriteItem` record SHALL define `Price` as `decimal?` with `[JsonConverter(typeof(FlexibleDecimalConverter))]` attribute.

#### Scenario: Deserialize favorite item with string price
- **WHEN** the JSON response from `GET /api/private/favorites` contains `"price": "25.50"`
- **THEN** `FavoriteItem.Price` SHALL be `25.50m`

### Requirement: Navigation query strings format Price as invariant string
ViewModels that pass `Price` in Shell navigation query strings SHALL format it using `CultureInfo.InvariantCulture` to produce a culture-independent decimal representation.

#### Scenario: HomeViewModel formats price for navigation
- **WHEN** navigating to listing detail from the home feed
- **THEN** the price query parameter SHALL be formatted as `Price?.ToString(CultureInfo.InvariantCulture) ?? string.Empty`

#### Scenario: FavoritesViewModel formats price for navigation
- **WHEN** navigating to listing detail from the favorites page
- **THEN** the price query parameter SHALL be formatted as `Price?.ToString(CultureInfo.InvariantCulture) ?? string.Empty`
