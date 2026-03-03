## Purpose

Defines the server-side logout behavior, ensuring refresh tokens are revoked on the backend before local auth data is cleared.

## Requirements

### Requirement: Server-side logout
`IAuthService.LogoutAsync()` SHALL call `POST /api/v1/logout` with the current Bearer token to revoke server-side refresh tokens before clearing local auth data.

#### Scenario: Successful logout
- **WHEN** `LogoutAsync()` is called with a valid token
- **THEN** the service SHALL POST to `/api/v1/logout`, then call `ClearAuthAsync()` and `IFavoritesService.ClearCache()`

#### Scenario: Logout with network failure
- **WHEN** `LogoutAsync()` is called and the POST fails
- **THEN** the service SHALL still call `ClearAuthAsync()` and `IFavoritesService.ClearCache()` to ensure local cleanup

#### Scenario: Logout without token
- **WHEN** `LogoutAsync()` is called and no token is stored
- **THEN** the service SHALL skip the POST and only call `ClearAuthAsync()` and `IFavoritesService.ClearCache()`
