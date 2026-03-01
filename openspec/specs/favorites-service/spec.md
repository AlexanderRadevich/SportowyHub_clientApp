## ADDED Requirements

### Requirement: Favorites API service interface

The app SHALL provide an `IFavoritesService` registered as a singleton in DI with methods to interact with the backend favorites endpoints. All methods SHALL require a valid auth token obtained from `IAuthService.GetTokenAsync()`.

#### Scenario: Service is injectable
- **WHEN** `IFavoritesService` is resolved from DI
- **THEN** a singleton `FavoritesService` instance SHALL be returned

### Requirement: Load favorite IDs

`IFavoritesService` SHALL provide a `LoadFavoriteIdsAsync(CancellationToken)` method that calls `GET /api/private/favorites/ids` and caches the returned IDs in memory.

#### Scenario: Load IDs successfully
- **WHEN** `LoadFavoriteIdsAsync` is called with a valid token
- **THEN** the service SHALL call `GET /api/private/favorites/ids` with Bearer auth
- **THEN** the returned IDs SHALL be stored in an in-memory `HashSet<string>`

#### Scenario: Load IDs when not authenticated
- **WHEN** `LoadFavoriteIdsAsync` is called and no token is available
- **THEN** the cache SHALL be cleared and no API call SHALL be made

### Requirement: Check if listing is favorited

`IFavoritesService` SHALL provide an `IsFavorite(string listingId)` synchronous method that checks the in-memory cache.

#### Scenario: Listing is favorited
- **WHEN** `IsFavorite("abc")` is called and "abc" is in the cache
- **THEN** it SHALL return `true`

#### Scenario: Listing is not favorited
- **WHEN** `IsFavorite("xyz")` is called and "xyz" is not in the cache
- **THEN** it SHALL return `false`

### Requirement: Get paginated favorites list

`IFavoritesService` SHALL provide a `GetFavoritesAsync(int page, int perPage, CancellationToken)` method that calls `GET /api/private/favorites?page={page}&per_page={perPage}`.

#### Scenario: Load favorites page
- **WHEN** `GetFavoritesAsync(1, 20)` is called
- **THEN** the service SHALL return a `FavoritesListResponse` with `Items`, `Total`, `Page`, `PerPage`, and `Pages`

### Requirement: Add to favorites

`IFavoritesService` SHALL provide an `AddAsync(string listingId, CancellationToken)` method that calls `POST /api/private/favorites/{listingId}` and updates the in-memory cache.

#### Scenario: Add successfully
- **WHEN** `AddAsync("abc")` is called
- **THEN** the service SHALL call `POST /api/private/favorites/abc` with Bearer auth
- **THEN** "abc" SHALL be added to the in-memory IDs cache

#### Scenario: Add already favorited (409)
- **WHEN** `AddAsync("abc")` is called and the backend returns 409
- **THEN** the service SHALL treat it as success and ensure "abc" is in the cache

### Requirement: Remove from favorites

`IFavoritesService` SHALL provide a `RemoveAsync(string listingId, CancellationToken)` method that calls `DELETE /api/private/favorites/{listingId}` and updates the in-memory cache.

#### Scenario: Remove successfully
- **WHEN** `RemoveAsync("abc")` is called
- **THEN** the service SHALL call `DELETE /api/private/favorites/abc` with Bearer auth
- **THEN** "abc" SHALL be removed from the in-memory IDs cache

### Requirement: Clear cache on logout

`IFavoritesService` SHALL provide a `ClearCache()` method that empties the in-memory IDs cache. This SHALL be called when the user logs out.

#### Scenario: Clear cache
- **WHEN** `ClearCache()` is called
- **THEN** the in-memory IDs set SHALL be empty
- **THEN** `IsFavorite` SHALL return `false` for all IDs

### Requirement: API response models

The app SHALL define the following records for favorites API deserialization, registered in `SportowyHubJsonContext`:

- `FavoritesIdsResponse(List<string> Ids, int Count)`
- `FavoritesListResponse(List<FavoriteItem> Items, int Total, int Page, int PerPage, int Pages)`
- `FavoriteItem(string Id, string Title, string? Price, string? Currency, string? City, string Status, string? Slug, int SerialId)`
- `FavoriteActionResponse(string Status, int FavoritesCount)`

#### Scenario: Deserialize favorites list response
- **WHEN** the API returns a JSON favorites list
- **THEN** it SHALL deserialize to `FavoritesListResponse` using source-generated JSON context with snake_case naming
