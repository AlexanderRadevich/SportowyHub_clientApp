## ADDED Requirements

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

## MODIFIED Requirements

### Requirement: MediaUrls record fields
The `MediaUrls` record SHALL define properties matching the actual backend media URL derivatives: `Thumb160` (string?), `Thumb320` (string?), `Card640` (string?), `Gallery1024` (string?), `Gallery1920` (string?), `Og1200x630` (string?). All fields SHALL be nullable because URLs are null when the listing is not in published/archived status. The old `Original` and `Thumbnail` properties SHALL be removed.

#### Scenario: MediaUrls deserializes from upload response
- **WHEN** the API returns `{"thumb_160":"https://cdn.../...","thumb_320":"https://cdn.../...","card_640":"https://cdn.../...",...}`
- **THEN** `MediaUrls.Thumb160`, `Thumb320`, `Card640` etc. SHALL contain the corresponding URLs

#### Scenario: MediaUrls deserializes with null URLs for draft listing
- **WHEN** the API returns media URLs for a draft listing where all derivative URLs are null
- **THEN** all `MediaUrls` properties SHALL be null
