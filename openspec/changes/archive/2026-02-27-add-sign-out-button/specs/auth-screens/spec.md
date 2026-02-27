## ADDED Requirements

### Requirement: LogoutAsync in auth service
`IAuthService` SHALL expose a `LogoutAsync()` method returning `Task`. `AuthService` SHALL implement it as follows: read the refresh token from SecureStorage; if a refresh token exists, attempt `POST /api/v1/logout` with the refresh token as Bearer authorization; always call `ClearAuthAsync()` regardless of the API call result. The method SHALL NOT throw exceptions — all errors from the server call SHALL be silently caught.

#### Scenario: LogoutAsync clears local tokens
- **WHEN** `LogoutAsync()` is called
- **THEN** all auth tokens (access token, refresh token, token expiry, user info) SHALL be removed from SecureStorage

#### Scenario: LogoutAsync calls backend endpoint
- **WHEN** `LogoutAsync()` is called and a refresh token exists in SecureStorage
- **THEN** the service SHALL attempt `POST /api/v1/logout` with the refresh token as Bearer auth header

#### Scenario: LogoutAsync succeeds even when server is unreachable
- **WHEN** `LogoutAsync()` is called and the `POST /api/v1/logout` request fails (network error, server error, etc.)
- **THEN** local tokens SHALL still be cleared and no exception SHALL be thrown

#### Scenario: LogoutAsync with no refresh token skips server call
- **WHEN** `LogoutAsync()` is called and no refresh token exists in SecureStorage
- **THEN** the service SHALL skip the API call and only clear local tokens
