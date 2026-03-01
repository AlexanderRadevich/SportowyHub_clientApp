## ADDED Requirements

### Requirement: ListingSummary record for browse endpoint
The system SHALL define a `ListingSummary` record with properties matching the `/api/v1/listings` response item: `Id` (string), `Slug` (string?), `Title` (string), `Price` (string?), `Currency` (string?), `City` (string?), `CategoryId` (int), `ContentLocale` (string?), `PublishedAt` (string?).

#### Scenario: Deserialize listing summary from API
- **WHEN** the app receives a JSON object from `/api/v1/listings` with snake_case fields
- **THEN** it SHALL deserialize into a `ListingSummary` record with all fields mapped correctly

### Requirement: ListingsResponse wrapper for browse endpoint
The system SHALL define a `ListingsResponse` record with `Listings` (List&lt;ListingSummary&gt;) and `Total` (int) matching the `/api/v1/listings` envelope.

#### Scenario: Deserialize listings page
- **WHEN** the app receives `{ "listings": [...], "total": 5 }`
- **THEN** it SHALL deserialize into a `ListingsResponse` with the listings list and total count

### Requirement: ListingDetail record for detail endpoint
The system SHALL define a `ListingDetail` record with all fields from `/api/v1/listings/{id}`: `Id` (string), `Slug` (string?), `Title` (string), `Description` (string?), `Price` (string?), `Currency` (string?), `City` (string?), `Region` (string?), `Status` (string), `CategoryId` (int), `ContentLocale` (string?), `CreatedAt` (string), `PublishedAt` (string?), `LastModeratorComment` (string?).

#### Scenario: Deserialize listing detail
- **WHEN** the app receives a listing detail JSON from `/api/v1/listings/{id}`
- **THEN** it SHALL deserialize into a `ListingDetail` record with all fields mapped

### Requirement: SearchResultItem record for search endpoint
The system SHALL define a `SearchResultItem` record with properties matching the `/api/v1/search` response item: `Id` (string), `Slug` (string), `SerialId` (int), `Title` (string), `Description` (string), `CategoryId` (string), `CategoryPath` (string), `Sport` (string), `Price` (float?), `Currency` (string?), `City` (string?), `Region` (string?), `Status` (string), `OwnerTrustLevel` (string), `CreatedAt` (string), `PublishedAt` (string?), `Location` (GeoLocation?), `Attributes` (List&lt;SearchAttribute&gt;?).

#### Scenario: Deserialize search result with location and attributes
- **WHEN** the app receives a search result JSON containing `location` and `attributes` fields
- **THEN** it SHALL deserialize into a `SearchResultItem` with `Location` as a `GeoLocation` object and `Attributes` as a list of `SearchAttribute`

#### Scenario: Deserialize search result without optional fields
- **WHEN** the app receives a search result JSON without `location` or `attributes`
- **THEN** `Location` SHALL be null and `Attributes` SHALL be null

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

### Requirement: Register all models in SportowyHubJsonContext
All new records SHALL be registered in `SportowyHubJsonContext` with `[JsonSerializable]` attributes to enable source-generated serialization with the existing `SnakeCaseLower` naming policy.

#### Scenario: Source-generated serialization
- **WHEN** the app serializes or deserializes any new listing/search model
- **THEN** it SHALL use the source-generated `SportowyHubJsonContext` without reflection
