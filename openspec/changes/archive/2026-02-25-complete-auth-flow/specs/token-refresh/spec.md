## ADDED Requirements

### Requirement: Refresh token storage
When `LoginAsync` receives a successful response containing a `refresh_token` field, the refresh token SHALL be stored in `SecureStorage` under key `auth_refresh_token`. The `expires_in` value SHALL be stored under key `auth_token_expiry` as the absolute expiry timestamp (current UTC + expires_in seconds).

#### Scenario: Refresh token stored on login
- **WHEN** `LoginAsync` succeeds and the response contains `refresh_token`
- **THEN** `SecureStorage["auth_refresh_token"]` SHALL contain the refresh token value

#### Scenario: Token expiry stored on login
- **WHEN** `LoginAsync` succeeds and the response contains `expires_in: 3600`
- **THEN** `SecureStorage["auth_token_expiry"]` SHALL contain a UTC timestamp approximately 3600 seconds from now

#### Scenario: No refresh token in response
- **WHEN** `LoginAsync` succeeds and the response does not contain `refresh_token`
- **THEN** `SecureStorage["auth_refresh_token"]` SHALL NOT be written

### Requirement: ClearAuthAsync removes refresh token data
`ClearAuthAsync` SHALL remove `auth_refresh_token` and `auth_token_expiry` from `SecureStorage` in addition to the existing `auth_token` and `auth_user` keys.

#### Scenario: Clear auth removes all token data
- **WHEN** `ClearAuthAsync` is called
- **THEN** `SecureStorage["auth_refresh_token"]` and `SecureStorage["auth_token_expiry"]` SHALL be removed

### Requirement: RefreshTokenAsync method
`IAuthService` SHALL expose `Task<AuthResult<LoginResponse>> RefreshTokenAsync()`. The implementation SHALL read `auth_refresh_token` from SecureStorage, POST to `/api/v1/refresh` with the refresh token as Bearer authorization and `X-Include-Refresh-Token: true` header. On success, it SHALL store the new access token (and new refresh token if present) and return `AuthResult.Success`. On failure, it SHALL clear all auth data and return `AuthResult.Failure`.

#### Scenario: Successful refresh stores new tokens
- **WHEN** `RefreshTokenAsync` is called with a valid stored refresh token
- **THEN** the new `access_token` SHALL replace the old one in SecureStorage, the new `refresh_token` (if present) SHALL replace the old one, and the method SHALL return `AuthResult.Success`

#### Scenario: Refresh with expired refresh token clears auth
- **WHEN** `RefreshTokenAsync` is called and the API returns 401
- **THEN** all auth data SHALL be cleared from SecureStorage and the method SHALL return `AuthResult.Failure`

#### Scenario: Refresh with no stored refresh token returns failure
- **WHEN** `RefreshTokenAsync` is called and no refresh token exists in SecureStorage
- **THEN** the method SHALL return `AuthResult.Failure` without making an API call

#### Scenario: Refresh request includes mobile header
- **WHEN** `RefreshTokenAsync` makes the API call
- **THEN** the request SHALL include `X-Include-Refresh-Token: true` header

### Requirement: Login request includes refresh token header
`LoginAsync` SHALL include `X-Include-Refresh-Token: true` header in the POST to `/api/v1/login` so the backend returns a refresh token in the response body for mobile clients.

#### Scenario: Login request has refresh token header
- **WHEN** `LoginAsync` makes the API call
- **THEN** the request SHALL include `X-Include-Refresh-Token: true` header

### Requirement: RequestProvider supports custom headers
`RequestProvider.PostAsync` SHALL accept an optional `Dictionary<string, string>? headers` parameter that adds custom headers to the request before sending.

#### Scenario: Custom headers are applied to request
- **WHEN** `PostAsync` is called with headers `{"X-Include-Refresh-Token": "true"}`
- **THEN** the HTTP request SHALL include the `X-Include-Refresh-Token: true` header
