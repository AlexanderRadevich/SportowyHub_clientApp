## Purpose

Defines the service interface and models for reporting listings to the moderation system with a reason code and optional comment.

## Requirements

### Requirement: IModerationService interface
The app SHALL define an `IModerationService` interface registered as singleton in DI with method:
- `ReportListingAsync(string listingId, string reasonCode, string? comment, CancellationToken ct)` returning `Task<ReportResponse>`

#### Scenario: Service is injectable
- **WHEN** `IModerationService` is resolved from DI
- **THEN** a singleton `ModerationService` instance SHALL be returned

### Requirement: Report listing
`ReportListingAsync` SHALL POST to `/api/v1/moderation/report` with `{"listing_id":"...","reason_code":"...","comment":"..."}`. No auth required but authenticated users have higher trust.

#### Scenario: Report with comment
- **WHEN** `ReportListingAsync("uuid", "SPAM", "This is spam content")` is called
- **THEN** it SHALL POST the report with all three fields and return `ReportResponse`

#### Scenario: Report without comment
- **WHEN** `ReportListingAsync("uuid", "SPAM", comment: null)` is called
- **THEN** the `comment` field SHALL be null or omitted in the JSON

#### Scenario: Listing not found
- **WHEN** the backend returns 404
- **THEN** the exception SHALL propagate to the caller

### Requirement: ReportListingRequest model
The app SHALL define a `ReportListingRequest` record with: `ListingId` (string), `ReasonCode` (string), `Comment` (string?). Registered in `SportowyHubJsonContext`.

#### Scenario: Serialize report request
- **WHEN** serialized with ListingId="uuid", ReasonCode="SPAM", Comment="spam"
- **THEN** the JSON SHALL include `"listing_id":"uuid"`, `"reason_code":"SPAM"`, `"comment":"spam"`

### Requirement: ReportResponse model
The app SHALL define a `ReportResponse` record with: `Id` (int), `ListingId` (string), `Type` (string), `Status` (string), `ReasonCode` (string), `CreatedAt` (string). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize report response
- **WHEN** the API returns a report JSON with all fields
- **THEN** it SHALL deserialize to `ReportResponse` with Status="open"
