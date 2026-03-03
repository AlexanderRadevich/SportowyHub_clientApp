## MODIFIED Requirements

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
