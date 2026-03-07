## MODIFIED Requirements

### Requirement: ListingDetail record for detail endpoint
The system SHALL define a `ListingDetail` record with all fields from `/api/v1/listings/{id}`: `Id` (string), `Slug` (string?), `Title` (string), `Description` (string?), `Price` (decimal?, with `[JsonConverter(typeof(FlexibleDecimalConverter))]`), `Currency` (string?), `City` (string?), `Region` (string?), `Status` (string), `CategoryId` (int), `ContentLocale` (string?), `Condition` (string?), `CreatedAt` (string), `PublishedAt` (string?), `LastModeratorComment` (string?).

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
