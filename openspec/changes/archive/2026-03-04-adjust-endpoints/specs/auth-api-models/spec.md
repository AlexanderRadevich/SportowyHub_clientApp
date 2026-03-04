## MODIFIED Requirements

### Requirement: FavoriteActionResponse includes favorites count
The `FavoriteActionResponse` record SHALL include a `FavoritesCount` field of type `int?` that reflects the total number of favorites for the user after the add operation, as returned by the backend `POST /api/private/favorites/{listingId}` response field `favorites_count`.

#### Scenario: Add favorite returns favorites count
- **GIVEN** the user is authenticated and the listing is published
- **WHEN** `FavoritesService.AddAsync` receives a successful `201` response from the backend
- **THEN** the deserialized `FavoriteActionResponse` SHALL have `FavoritesCount` populated with the integer value of `favorites_count` from the JSON response

#### Scenario: FavoritesCount is null-safe when field is absent
- **GIVEN** a backend response that omits `favorites_count`
- **WHEN** `System.Text.Json` deserializes the response using `SportowyHubJsonContext`
- **THEN** `FavoriteActionResponse.FavoritesCount` SHALL be `null` and no deserialization exception SHALL occur

### Requirement: FavoriteItem includes serial ID
The `FavoriteItem` record SHALL include a `SerialId` field of type `int` mapping to the `serial_id` field returned by `GET /api/private/favorites`. This field SHALL be available for use in display and deep-link navigation.

#### Scenario: Favorites list response contains serial ID per item
- **GIVEN** the user is authenticated and has at least one favorite listing
- **WHEN** `FavoritesService.GetFavoritesAsync` receives a `200` response
- **THEN** each item in the deserialized `FavoritesListResponse.Items` list SHALL have `SerialId` populated with a positive integer

#### Scenario: FavoriteItem is registered in SportowyHubJsonContext
- **GIVEN** `FavoriteItem` has a `SerialId` property of type `int`
- **WHEN** the source-generated `SportowyHubJsonContext` serializes or deserializes `FavoriteItem`
- **THEN** `serial_id` is correctly mapped to `SerialId` via the `SnakeCaseLower` naming policy

## ADDED Requirements

### Requirement: Endpoint URI strings contain no trailing slashes on mutation endpoints
Every URI string passed to `IRequestProvider` methods (`PostAsync`, `PutAsync`, `PatchAsync`, `DeleteAsync`) throughout all service classes SHALL NOT end with a trailing slash (`/`). This applies to `ListingManagementService`, `MessagingService`, and `MediaService` which currently have trailing slashes on their create endpoints.

#### Scenario: CreateListingAsync sends POST without trailing slash
- **GIVEN** `ListingManagementService.CreateListingAsync` is called with a valid `CreateListingRequest`
- **WHEN** the URI is constructed
- **THEN** the URI SHALL be `/api/private/listings` with no trailing slash

#### Scenario: CreateConversationAsync sends POST without trailing slash
- **GIVEN** `MessagingService.CreateConversationAsync` is called with a listing ID
- **WHEN** the URI is constructed
- **THEN** the URI SHALL be `/api/private/conversations` with no trailing slash

#### Scenario: MediaService.UploadAsync sends POST without trailing slash
- **GIVEN** `MediaService.UploadAsync` is called with a listing ID and a file stream
- **WHEN** the multipart URI is constructed
- **THEN** the URI SHALL be `/api/private/media` with no trailing slash
