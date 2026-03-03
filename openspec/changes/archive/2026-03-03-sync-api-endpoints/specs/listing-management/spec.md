## ADDED Requirements

### Requirement: IListingManagementService interface
The app SHALL define an `IListingManagementService` interface registered as singleton in DI with methods for authenticated listing CRUD operations:
- `GetMyListingsAsync(string? status, CancellationToken ct)` returning `Task<ListingsResponse>`
- `CreateListingAsync(CreateListingRequest request, CancellationToken ct)` returning `Task<ListingDetail>`
- `UpdateListingAsync(string id, UpdateListingRequest request, CancellationToken ct)` returning `Task<UpdateListingResponse>`
- `DeleteListingAsync(string id, CancellationToken ct)` returning `Task`
- `UpdateStatusAsync(string id, string status, CancellationToken ct)` returning `Task<UpdateStatusResponse>`
- `ResubmitForReviewAsync(string id, CancellationToken ct)` returning `Task<ResubmitResponse>`

#### Scenario: Service is injectable
- **WHEN** `IListingManagementService` is resolved from DI
- **THEN** a singleton `ListingManagementService` instance SHALL be returned

### Requirement: Get my listings
`GetMyListingsAsync` SHALL call `GET /api/private/listings/my` with optional `status` query parameter and Bearer auth.

#### Scenario: Fetch all my listings
- **WHEN** `GetMyListingsAsync(status: null)` is called
- **THEN** it SHALL call `GET /api/private/listings/my` with Bearer auth

#### Scenario: Fetch published listings only
- **WHEN** `GetMyListingsAsync(status: "published")` is called
- **THEN** it SHALL call `GET /api/private/listings/my?status=published`

### Requirement: Create listing
`CreateListingAsync` SHALL POST to `/api/private/listings/` with Bearer auth and the request body.

#### Scenario: Create listing successfully
- **WHEN** `CreateListingAsync` is called with valid data
- **THEN** it SHALL POST to `/api/private/listings/` and return the created `ListingDetail` with status "draft"

### Requirement: Update listing
`UpdateListingAsync` SHALL PUT to `/api/private/listings/{id}` with Bearer auth and the request body.

#### Scenario: Update listing fields
- **WHEN** `UpdateListingAsync("uuid", request)` is called
- **THEN** it SHALL PUT to `/api/private/listings/uuid` with Bearer auth

### Requirement: Delete listing
`DeleteListingAsync` SHALL send `DELETE /api/private/listings/{id}` with Bearer auth.

#### Scenario: Delete own listing
- **WHEN** `DeleteListingAsync("uuid")` is called
- **THEN** it SHALL DELETE `/api/private/listings/uuid` with Bearer auth

### Requirement: Update listing status
`UpdateStatusAsync` SHALL PATCH to `/api/private/listings/{id}/status` with `{"status":"..."}` body and Bearer auth.

#### Scenario: Publish listing
- **WHEN** `UpdateStatusAsync("uuid", "published")` is called
- **THEN** it SHALL PATCH to `/api/private/listings/uuid/status` with `{"status":"published"}`

### Requirement: Resubmit for review
`ResubmitForReviewAsync` SHALL POST to `/api/private/listings/{id}/resubmit-for-review` with Bearer auth.

#### Scenario: Resubmit rejected listing
- **WHEN** `ResubmitForReviewAsync("uuid")` is called
- **THEN** it SHALL POST to `/api/private/listings/uuid/resubmit-for-review` with Bearer auth

### Requirement: CreateListingRequest model
The app SHALL define a `CreateListingRequest` record with: `CategoryId` (int), `Title` (string), `Description` (string), `Price` (decimal?), `Currency` (string?), `CityId` (int), `VoivodeshipId` (int), `LocationLatitude` (double?), `LocationLongitude` (double?), `Attributes` (Dictionary<string, string>?), `ContentLocale` (string?). Registered in `SportowyHubJsonContext`.

#### Scenario: Serialize create request
- **WHEN** a `CreateListingRequest` is serialized
- **THEN** all fields SHALL use snake_case naming (e.g., `category_id`, `city_id`, `voivodeship_id`)

### Requirement: UpdateListingRequest model
The app SHALL define an `UpdateListingRequest` record with the same fields as `CreateListingRequest`, all nullable. Registered in `SportowyHubJsonContext`.

#### Scenario: Serialize partial update
- **WHEN** an `UpdateListingRequest` with only Title="New Title" is serialized
- **THEN** only non-null fields SHALL be included in the JSON

### Requirement: UpdateListingResponse model
The app SHALL define an `UpdateListingResponse` record with `Message` (string). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize update response
- **WHEN** the API returns `{"message":"Listing updated successfully"}`
- **THEN** it SHALL deserialize to `UpdateListingResponse`

### Requirement: UpdateStatusResponse model
The app SHALL define an `UpdateStatusResponse` record with `Message` (string) and `Status` (string). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize status response
- **WHEN** the API returns `{"message":"Status updated successfully","status":"published"}`
- **THEN** it SHALL deserialize to `UpdateStatusResponse`

### Requirement: ResubmitResponse model
The app SHALL define a `ResubmitResponse` record with `Message` (string), `ListingId` (string), and `Status` (string). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize resubmit response
- **WHEN** the API returns `{"message":"Listing resubmitted for review","listing_id":"uuid","status":"pending_review"}`
- **THEN** it SHALL deserialize to `ResubmitResponse`
