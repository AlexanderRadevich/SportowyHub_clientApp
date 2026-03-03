## ADDED Requirements

### Requirement: IMediaService interface
The app SHALL define an `IMediaService` interface registered as singleton in DI with methods:
- `UploadAsync(string listingId, Stream fileStream, string fileName, int? sortOrder, CancellationToken ct)` returning `Task<MediaItem>`
- `DeleteAsync(int mediaId, CancellationToken ct)` returning `Task`

#### Scenario: Service is injectable
- **WHEN** `IMediaService` is resolved from DI
- **THEN** a singleton `MediaService` instance SHALL be returned

### Requirement: Upload media
`UploadAsync` SHALL POST multipart/form-data to `/api/private/media/` with Bearer auth. The form data SHALL include `listing_id`, `file`, and optionally `sort_order`. Supported types: JPEG, PNG, HEIC, HEIF. Max size: 12MB.

#### Scenario: Upload image for listing
- **WHEN** `UploadAsync("listing-uuid", stream, "photo.jpg", sortOrder: 0)` is called
- **THEN** it SHALL POST multipart/form-data to `/api/private/media/` with fields `listing_id=listing-uuid`, `file=photo.jpg`, `sort_order=0`

#### Scenario: Upload without sort order
- **WHEN** `UploadAsync` is called with sortOrder=null
- **THEN** `sort_order` SHALL be omitted from the form data

### Requirement: Delete media
`DeleteAsync` SHALL send `DELETE /api/private/media/{id}` with Bearer auth. The response SHALL be 204 No Content.

#### Scenario: Delete media successfully
- **WHEN** `DeleteAsync(42)` is called
- **THEN** it SHALL DELETE `/api/private/media/42` with Bearer auth

### Requirement: MediaItem model
The app SHALL define a `MediaItem` record with: `Id` (int), `ListingId` (string), `Type` (string), `MimeType` (string), `Size` (int), `Width` (int), `Height` (int), `SortOrder` (int), `Urls` (MediaUrls), `CreatedAt` (string). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize upload response
- **WHEN** the API returns a media item JSON with nested `urls` object
- **THEN** it SHALL deserialize to `MediaItem` with `Urls.Original` and `Urls.Thumbnail`

### Requirement: MediaUrls model
The app SHALL define a `MediaUrls` record with `Original` (string) and `Thumbnail` (string). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize media URLs
- **WHEN** the API returns `{"original":"https://...","thumbnail":"https://..."}`
- **THEN** it SHALL deserialize to `MediaUrls` with both properties populated
