## Purpose

Defines the service interface and models for DSAR (Data Subject Access Request) compliance, allowing users to request data exports and account deletions via the private API.

## Requirements

### Requirement: IDsarService interface
The app SHALL define an `IDsarService` interface registered as singleton in DI with methods:
- `GetRequestsAsync(int limit, int offset, CancellationToken ct)` returning `Task<DsarListResponse>`
- `RequestDataExportAsync(CancellationToken ct)` returning `Task<DsarRequestResponse>`
- `RequestAccountDeletionAsync(CancellationToken ct)` returning `Task<DsarRequestResponse>`

#### Scenario: Service is injectable
- **WHEN** `IDsarService` is resolved from DI
- **THEN** a singleton `DsarService` instance SHALL be returned

### Requirement: List DSAR requests
`GetRequestsAsync` SHALL call `GET /api/private/dsar?limit={limit}&offset={offset}` with Bearer auth.

#### Scenario: Fetch DSAR requests
- **WHEN** `GetRequestsAsync(20, 0)` is called
- **THEN** it SHALL return a `DsarListResponse` with the user's DSAR request items

### Requirement: Request data export
`RequestDataExportAsync` SHALL POST to `/api/private/dsar/export` with Bearer auth and an empty body.

#### Scenario: Request export successfully
- **WHEN** `RequestDataExportAsync()` is called
- **THEN** it SHALL POST to `/api/private/dsar/export` and return `DsarRequestResponse` with status "pending"

### Requirement: Request account deletion
`RequestAccountDeletionAsync` SHALL POST to `/api/private/dsar/delete` with Bearer auth and an empty body.

#### Scenario: Request deletion successfully
- **WHEN** `RequestAccountDeletionAsync()` is called
- **THEN** it SHALL POST to `/api/private/dsar/delete` and return `DsarRequestResponse` with status "pending"

### Requirement: DsarRequestItem model
The app SHALL define a `DsarRequestItem` record with: `Id` (int), `Type` (string), `Status` (string), `RequestedAt` (string), `Deadline` (string), `CompletedAt` (string?), `IsOverdue` (bool). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize DSAR item
- **WHEN** the API returns a DSAR request item JSON
- **THEN** it SHALL deserialize to `DsarRequestItem` with all fields mapped

### Requirement: DsarListResponse model
The app SHALL define a `DsarListResponse` record with: `Items` (List&lt;DsarRequestItem&gt;). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize DSAR list
- **WHEN** the API returns a DSAR list JSON
- **THEN** it SHALL deserialize to `DsarListResponse`

### Requirement: DsarRequestResponse model
The app SHALL define a `DsarRequestResponse` record with: `RequestId` (int), `Type` (string), `Status` (string), `Deadline` (string), `ResultReference` (string?), `Message` (string?). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize export request response
- **WHEN** the API returns `{"request_id":1,"type":"export","status":"pending","deadline":"2026-03-31T10:00:00+00:00","result_reference":null}`
- **THEN** it SHALL deserialize to `DsarRequestResponse` with ResultReference=null
