## MODIFIED Requirements

### Requirement: Remove from favorites
`IFavoritesService` SHALL provide a `RemoveAsync(string listingId, CancellationToken)` method that calls `DELETE /api/private/favorites/{listingId}` and updates the in-memory cache. The method SHALL handle both `200 OK` with a JSON body (`{"status":"removed","favorites_count":N}`) and `204 No Content` responses as success.

#### Scenario: Remove successfully with 200 response
- **WHEN** `RemoveAsync("abc")` is called and the backend returns 200 with `{"status":"removed","favorites_count":2}`
- **THEN** "abc" SHALL be removed from the in-memory IDs cache

#### Scenario: Remove successfully with 204 response
- **WHEN** `RemoveAsync("abc")` is called and the backend returns 204 No Content
- **THEN** "abc" SHALL be removed from the in-memory IDs cache

#### Scenario: Remove updates cache regardless of response body
- **WHEN** `RemoveAsync("abc")` is called and the DELETE succeeds (any 2xx)
- **THEN** the service SHALL remove "abc" from the local `_favoriteIds` cache without requiring a specific response body
