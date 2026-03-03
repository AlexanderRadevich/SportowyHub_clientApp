# Listings API Models

## Purpose

Defines the C# record types used to deserialize API responses for listings, search, and related operations.

## Requirements

### Requirement: ListingSummary record for browse endpoint
The system SHALL define a `ListingSummary` record with properties matching the `/api/v1/listings` response item: `Id` (string), `Slug` (string?), `Title` (string), `Price` (decimal?, with `[JsonConverter(typeof(FlexibleDecimalConverter))]`), `Currency` (string?), `City` (string?), `CategoryId` (int), `ContentLocale` (string?), `PublishedAt` (string?).

#### Scenario: Deserialize listing summary from API
- **WHEN** the app receives a JSON object from `/api/v1/listings` with snake_case fields
- **THEN** it SHALL deserialize into a `ListingSummary` record with all fields mapped correctly

#### Scenario: Deserialize listing summary with numeric price
- **WHEN** the JSON response from `GET /api/v1/listings` contains `"price": 150.0`
- **THEN** `ListingSummary.Price` SHALL be `150.0m`

#### Scenario: Deserialize listing summary with null price
- **WHEN** the JSON response contains `"price": null`
- **THEN** `ListingSummary.Price` SHALL be `null`

### Requirement: ListingsResponse wrapper for browse endpoint
The system SHALL define a `ListingsResponse` record with `Listings` (List&lt;ListingSummary&gt;) and `Total` (int) matching the `/api/v1/listings` envelope.

#### Scenario: Deserialize listings page
- **WHEN** the app receives `{ "listings": [...], "total": 5 }`
- **THEN** it SHALL deserialize into a `ListingsResponse` with the listings list and total count

### Requirement: ListingDetail record for detail endpoint
The system SHALL define a `ListingDetail` record with all fields from `/api/v1/listings/{id}`: `Id` (string), `Slug` (string?), `Title` (string), `Description` (string?), `Price` (decimal?, with `[JsonConverter(typeof(FlexibleDecimalConverter))]`), `Currency` (string?), `City` (string?), `Region` (string?), `Status` (string), `CategoryId` (int), `ContentLocale` (string?), `CreatedAt` (string), `PublishedAt` (string?), `LastModeratorComment` (string?).

#### Scenario: Deserialize listing detail
- **WHEN** the app receives a listing detail JSON from `/api/v1/listings/{id}`
- **THEN** it SHALL deserialize into a `ListingDetail` record with all fields mapped

#### Scenario: Deserialize listing detail with string price
- **WHEN** the JSON response from `GET /api/v1/listings/{id}` contains `"price": "150.00"`
- **THEN** `ListingDetail.Price` SHALL be `150.00m`

#### Scenario: Deserialize listing detail with null price
- **WHEN** the JSON response contains `"price": null`
- **THEN** `ListingDetail.Price` SHALL be `null`

### Requirement: SearchResultItem record for search endpoint
The system SHALL define a `SearchResultItem` record with properties matching the `/api/v1/search` response item: `Id` (string), `Slug` (string), `SerialId` (int), `Title` (string), `Description` (string), `CategoryId` (string), `CategoryPath` (string), `Sport` (string), `Price` (float?), `Currency` (string?), `City` (string?), `Region` (string?), `Status` (string), `OwnerTrustLevel` (string), `CreatedAt` (string), `PublishedAt` (string?), `Location` (GeoLocation?), `Attributes` (List&lt;SearchAttribute&gt;?), `ContentLocale` (string?).

#### Scenario: Deserialize search result with location and attributes
- **WHEN** the app receives a search result JSON containing `location` and `attributes` fields
- **THEN** it SHALL deserialize into a `SearchResultItem` with `Location` as a `GeoLocation` object and `Attributes` as a list of `SearchAttribute`

#### Scenario: Deserialize search result without optional fields
- **WHEN** the app receives a search result JSON without `location` or `attributes`
- **THEN** `Location` SHALL be null and `Attributes` SHALL be null

#### Scenario: Deserialize search result with content_locale
- **WHEN** the app receives a search result JSON with `content_locale` field
- **THEN** `ContentLocale` SHALL contain the locale value

### Requirement: GeoLocation record
The system SHALL define a `GeoLocation` record with `Lat` (double) and `Lon` (double).

#### Scenario: Deserialize geo coordinates
- **WHEN** the app receives `{ "lat": 52.2297, "lon": 21.0122 }`
- **THEN** it SHALL deserialize into a `GeoLocation` with the correct coordinates

### Requirement: SearchAttribute record
The system SHALL define a `SearchAttribute` record with `Key` (string) and `Value` (string).

#### Scenario: Deserialize attribute pair
- **WHEN** the app receives `{ "key": "condition", "value": "new" }`
- **THEN** it SHALL deserialize into a `SearchAttribute` with Key="condition" and Value="new"

### Requirement: SearchResponse wrapper for search endpoint
The system SHALL define a `SearchResponse` record with `Items` (List&lt;SearchResultItem&gt;), `Total` (int), `Limit` (int), `Offset` (int).

#### Scenario: Deserialize search response with pagination
- **WHEN** the app receives `{ "items": [...], "total": 156, "limit": 30, "offset": 0 }`
- **THEN** it SHALL deserialize into a `SearchResponse` with all pagination metadata

### Requirement: FavoriteItem record fields
The `FavoriteItem` record SHALL define `Price` as `decimal?` with `[JsonConverter(typeof(FlexibleDecimalConverter))]` attribute.

#### Scenario: Deserialize favorite item with string price
- **WHEN** the JSON response from `GET /api/private/favorites` contains `"price": "25.50"`
- **THEN** `FavoriteItem.Price` SHALL be `25.50m`

### Requirement: MyListingSummary model
The app SHALL define a `MyListingSummary` record with properties: `Id` (string), `Slug` (string?), `Title` (string), `Status` (string), `Price` (decimal?, with FlexibleDecimalConverter), `Currency` (string?), `ContentLocale` (string?), `CreatedAt` (string), `PublishedAt` (string?). The type SHALL be registered in `SportowyHubJsonContext`.

#### Scenario: MyListingSummary deserializes from my-listings endpoint
- **WHEN** the API returns `{"id":"uuid","title":"Bike","status":"draft","price":"150.00","created_at":"2025-01-15T12:00:00+00:00"}`
- **THEN** `MyListingSummary` SHALL deserialize with Status="draft", Price=150.00m, CreatedAt="2025-01-15T12:00:00+00:00"

### Requirement: MyListingsResponse model
The app SHALL define a `MyListingsResponse` record with properties: `Listings` (List&lt;MyListingSummary&gt;), `Total` (int). The type SHALL be registered in `SportowyHubJsonContext`.

#### Scenario: MyListingsResponse deserializes full response
- **WHEN** the API returns `{"listings":[...],"total":5}`
- **THEN** `MyListingsResponse.Listings` SHALL contain the list and `Total` SHALL be 5

### Requirement: GetMyListingsAsync returns MyListingsResponse
`IListingManagementService.GetMyListingsAsync` SHALL return `MyListingsResponse` instead of `ListingsResponse` to correctly deserialize the `status` and `created_at` fields.

#### Scenario: Service returns correct response type
- **WHEN** `GetMyListingsAsync(status: "draft")` is called
- **THEN** the response SHALL be deserialized as `MyListingsResponse` with `MyListingSummary` items

### Requirement: MediaUrls record fields
The `MediaUrls` record SHALL define properties matching the actual backend media URL derivatives: `Thumb160` (string?), `Thumb320` (string?), `Card640` (string?), `Gallery1024` (string?), `Gallery1920` (string?), `Og1200x630` (string?). All fields SHALL be nullable because URLs are null when the listing is not in published/archived status.

#### Scenario: MediaUrls deserializes from upload response
- **WHEN** the API returns `{"thumb_160":"https://cdn.../...","thumb_320":"https://cdn.../...","card_640":"https://cdn.../...",...}`
- **THEN** `MediaUrls.Thumb160`, `Thumb320`, `Card640` etc. SHALL contain the corresponding URLs

#### Scenario: MediaUrls deserializes with null URLs for draft listing
- **WHEN** the API returns media URLs for a draft listing where all derivative URLs are null
- **THEN** all `MediaUrls` properties SHALL be null

### Requirement: Navigation query strings format Price as invariant string
ViewModels that pass `Price` in Shell navigation query strings SHALL format it using `CultureInfo.InvariantCulture` to produce a culture-independent decimal representation.

#### Scenario: HomeViewModel formats price for navigation
- **WHEN** navigating to listing detail from the home feed
- **THEN** the price query parameter SHALL be formatted as `Price?.ToString(CultureInfo.InvariantCulture) ?? string.Empty`

#### Scenario: FavoritesViewModel formats price for navigation
- **WHEN** navigating to listing detail from the favorites page
- **THEN** the price query parameter SHALL be formatted as `Price?.ToString(CultureInfo.InvariantCulture) ?? string.Empty`

### Requirement: Register all models in SportowyHubJsonContext
All new records SHALL be registered in `SportowyHubJsonContext` with `[JsonSerializable]` attributes to enable source-generated serialization with the existing `SnakeCaseLower` naming policy.

#### Scenario: Source-generated serialization
- **WHEN** the app serializes or deserializes any new listing/search model
- **THEN** it SHALL use the source-generated `SportowyHubJsonContext` without reflection
