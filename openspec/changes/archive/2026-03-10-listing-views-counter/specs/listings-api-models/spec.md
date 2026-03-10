## MODIFIED Requirements

### Requirement: ListingSummary record for browse endpoint
The system SHALL define a `ListingSummary` record with properties matching the `/api/v1/listings` response item: `Id` (string), `Slug` (string?), `Title` (string), `Price` (decimal?, with `[JsonConverter(typeof(FlexibleDecimalConverter))]`), `Currency` (string?), `City` (string?), `CategoryId` (int), `ContentLocale` (string?), `PublishedAt` (string?), `ViewCount` (int, default 0).

#### Scenario: Deserialize listing summary from API
- **WHEN** the app receives a JSON object from `/api/v1/listings` with snake_case fields
- **THEN** it SHALL deserialize into a `ListingSummary` record with all fields mapped correctly

#### Scenario: Deserialize listing summary with numeric price
- **WHEN** the JSON response from `GET /api/v1/listings` contains `"price": 150.0`
- **THEN** `ListingSummary.Price` SHALL be `150.0m`

#### Scenario: Deserialize listing summary with null price
- **WHEN** the JSON response contains `"price": null`
- **THEN** `ListingSummary.Price` SHALL be `null`

#### Scenario: Deserialize listing summary with view_count
- **WHEN** the JSON response contains `"view_count": 1234`
- **THEN** `ListingSummary.ViewCount` SHALL be `1234`

#### Scenario: Deserialize listing summary without view_count field
- **WHEN** the JSON response does not contain a `view_count` field
- **THEN** `ListingSummary.ViewCount` SHALL default to `0`

### Requirement: ListingDetail record for detail endpoint
The system SHALL define a `ListingDetail` record with all fields from `/api/v1/listings/{id}`: `Id` (string), `Slug` (string?), `Title` (string), `Description` (string?), `Price` (decimal?, with `[JsonConverter(typeof(FlexibleDecimalConverter))]`), `Currency` (string?), `City` (string?), `Region` (string?), `Status` (string), `CategoryId` (int), `ContentLocale` (string?), `Condition` (string?), `CreatedAt` (string), `PublishedAt` (string?), `LastModeratorComment` (string?), `ViewCount` (int, default 0).

#### Scenario: Deserialize listing detail
- **WHEN** the app receives a listing detail JSON from `/api/v1/listings/{id}`
- **THEN** it SHALL deserialize into a `ListingDetail` record with all fields mapped

#### Scenario: Deserialize listing detail with string price
- **WHEN** the JSON response from `GET /api/v1/listings/{id}` contains `"price": "150.00"`
- **THEN** `ListingDetail.Price` SHALL be `150.00m`

#### Scenario: Deserialize listing detail with null price
- **WHEN** the JSON response contains `"price": null`
- **THEN** `ListingDetail.Price` SHALL be `null`

#### Scenario: Deserialize listing detail with condition
- **WHEN** the JSON response contains `"condition": "new"`
- **THEN** `ListingDetail.Condition` SHALL be `"new"`

#### Scenario: Deserialize listing detail without condition
- **WHEN** the JSON response does not contain a `condition` field
- **THEN** `ListingDetail.Condition` SHALL be `null`

#### Scenario: Deserialize listing detail with view_count
- **WHEN** the JSON response contains `"view_count": 567`
- **THEN** `ListingDetail.ViewCount` SHALL be `567`

#### Scenario: Deserialize listing detail without view_count field
- **WHEN** the JSON response does not contain a `view_count` field
- **THEN** `ListingDetail.ViewCount` SHALL default to `0`

### Requirement: SearchResultItem record for search endpoint
The system SHALL define a `SearchResultItem` record with properties matching the `/api/v1/search` response item: `Id` (string), `Slug` (string), `SerialId` (int), `Title` (string), `Description` (string), `CategoryId` (string), `CategoryPath` (string), `Sport` (string), `Price` (float?), `Currency` (string?), `City` (string?), `Region` (string?), `Status` (string), `OwnerTrustLevel` (string), `CreatedAt` (string), `PublishedAt` (string?), `Location` (GeoLocation?), `Attributes` (List&lt;SearchAttribute&gt;?), `ContentLocale` (string?), `ViewCount` (int, default 0).

#### Scenario: Deserialize search result with location and attributes
- **WHEN** the app receives a search result JSON containing `location` and `attributes` fields
- **THEN** it SHALL deserialize into a `SearchResultItem` with `Location` as a `GeoLocation` object and `Attributes` as a list of `SearchAttribute`

#### Scenario: Deserialize search result without optional fields
- **WHEN** the app receives a search result JSON without `location` or `attributes`
- **THEN** `Location` SHALL be null and `Attributes` SHALL be null

#### Scenario: Deserialize search result with content_locale
- **WHEN** the app receives a search result JSON with `content_locale` field
- **THEN** `ContentLocale` SHALL contain the locale value

#### Scenario: Deserialize search result with view_count
- **WHEN** the search result JSON contains `"view_count": 890`
- **THEN** `SearchResultItem.ViewCount` SHALL be `890`

#### Scenario: Deserialize search result without view_count field
- **WHEN** the search result JSON does not contain a `view_count` field
- **THEN** `SearchResultItem.ViewCount` SHALL default to `0`

### Requirement: MyListingSummary model
The app SHALL define a `MyListingSummary` record with properties: `Id` (string), `Slug` (string?), `Title` (string), `Status` (string), `Price` (decimal?, with FlexibleDecimalConverter), `Currency` (string?), `ContentLocale` (string?), `CreatedAt` (string), `PublishedAt` (string?), `ViewCount` (int, default 0). The type SHALL be registered in `SportowyHubJsonContext`.

#### Scenario: MyListingSummary deserializes from my-listings endpoint
- **WHEN** the API returns `{"id":"uuid","title":"Bike","status":"draft","price":"150.00","created_at":"2025-01-15T12:00:00+00:00"}`
- **THEN** `MyListingSummary` SHALL deserialize with Status="draft", Price=150.00m, CreatedAt="2025-01-15T12:00:00+00:00"

#### Scenario: MyListingSummary deserializes view_count
- **WHEN** the API returns `{"id":"uuid","title":"Bike","status":"published","view_count":42}`
- **THEN** `MyListingSummary.ViewCount` SHALL be `42`
