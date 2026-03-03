## MODIFIED Requirements

### Requirement: UpdateProfileAsync on auth service
`IAuthService` SHALL expose a `Task<UserProfile?> UpdateProfileAsync(UpdateProfileRequest request)` method. `AuthService` SHALL implement it by reading the access token from SecureStorage and calling `PATCH /api/private/profile` with Bearer authorization and the request body. The method SHALL return the updated `UserProfile` on success and let exceptions propagate to the caller.

#### Scenario: UpdateProfileAsync uses PATCH method
- **WHEN** `UpdateProfileAsync` is called with valid data and the user has a valid token
- **THEN** the method SHALL call `PATCH /api/private/profile` (not PUT) with Bearer auth and return the updated `UserProfile`

#### Scenario: UpdateProfileAsync returns null when no token exists
- **WHEN** `UpdateProfileAsync` is called and no token is stored
- **THEN** the method SHALL return `null`

#### Scenario: UpdateProfileAsync propagates API errors
- **WHEN** the API returns a 422 validation error
- **THEN** the exception SHALL propagate to the caller with the error response body for field-level error parsing

### Requirement: LoginAsync calls API and stores token
`AuthService.LoginAsync` SHALL POST to `/api/v1/login` with email and password, including `X-Include-Refresh-Token: true` header. On a 200 response, it SHALL store the access token in `SecureStorage` under key `auth_token`, and if `refresh_token` is present store it under `auth_refresh_token`, and store the token expiry timestamp under `auth_token_expiry`, then return `AuthResult.Success(loginResponse)`. If the response includes a `User` object, it SHALL store the user info (id, email, trust_level) in `SecureStorage["auth_user"]` as serialized JSON. On a 401 response, it SHALL parse the error message and return `AuthResult.Failure` with the error code. On a 403 response, it SHALL parse the error code (`EMAIL_NOT_VERIFIED`, `USER_BANNED`) and return `AuthResult.Failure` with the `ErrorCode` populated. On a 422 response, it SHALL parse the `ApiError` body including `violations` and return `AuthResult.Failure` with `FieldErrors` populated from the violations.

#### Scenario: Successful login stores access token and returns success
- **WHEN** `LoginAsync("user@example.com", "validpass")` is called and the API returns 200 with an access token
- **THEN** the access token SHALL be stored in `SecureStorage["auth_token"]` and the method SHALL return an `AuthResult` with `IsSuccess=true`

#### Scenario: Successful login stores user info when present
- **WHEN** `LoginAsync` succeeds and the response contains a `User` object with id, email, and trust_level
- **THEN** the user info SHALL be serialized and stored in `SecureStorage["auth_user"]`

#### Scenario: Successful login stores refresh token when present
- **WHEN** `LoginAsync` succeeds and the response contains `refresh_token`
- **THEN** the refresh token SHALL be stored in `SecureStorage["auth_refresh_token"]`

#### Scenario: Login with invalid credentials returns failure
- **WHEN** `LoginAsync` is called and the API returns 401 with `{"locale":"pl","error":{"code":"INVALID_CREDENTIALS","message":"Invalid email or password"}}`
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false`, `ErrorMessage` containing the server error message, and `ErrorCode="INVALID_CREDENTIALS"`

#### Scenario: Login with unverified email returns failure with error code
- **WHEN** `LoginAsync` is called and the API returns 403 with `{"locale":"pl","error":{"code":"EMAIL_NOT_VERIFIED","message":"Please verify your email address"}}`
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false`, `ErrorCode="EMAIL_NOT_VERIFIED"`, and `ErrorMessage` containing the server message

#### Scenario: Login with banned user returns failure with error code
- **WHEN** `LoginAsync` is called and the API returns 403 with `{"locale":"pl","error":{"code":"USER_BANNED","message":"Your account has been suspended"}}`
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false`, `ErrorCode="USER_BANNED"`, and `ErrorMessage` containing the server message

#### Scenario: Login with validation errors returns field errors
- **WHEN** `LoginAsync` is called and the API returns 422
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false` and `FieldErrors` populated from the violations

#### Scenario: Login with network failure returns failure
- **WHEN** `LoginAsync` is called and the HTTP request throws due to no network connectivity
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false` and a user-friendly connection error message

## ADDED Requirements

### Requirement: LogoutAsync with server-side revocation
`IAuthService` SHALL expose a `Task LogoutAsync()` method. `AuthService` SHALL implement it by calling `POST /api/v1/logout` with the current Bearer token, then clearing all local auth data via `ClearAuthAsync()` and calling `IFavoritesService.ClearCache()`. If the network call fails, local auth data SHALL still be cleared.

#### Scenario: Logout calls server and clears local data
- **WHEN** `LogoutAsync()` is called with a valid token
- **THEN** the service SHALL POST to `/api/v1/logout` with Bearer auth, then clear `SecureStorage` auth keys and favorites cache

#### Scenario: Logout clears local data even on network failure
- **WHEN** `LogoutAsync()` is called and the network call fails
- **THEN** local auth data SHALL still be cleared via `ClearAuthAsync()`

### Requirement: OAuthLoginAsync for social login
`IAuthService` SHALL expose a `Task<AuthResult<LoginResponse>> OAuthLoginAsync(string provider, string? idToken, string? accessToken)` method. It SHALL POST to `/api/v1/auth/oauth/{provider}` with the provided tokens. On success, it SHALL store auth tokens and user info the same way as `LoginAsync`.

#### Scenario: OAuth login with Google id_token
- **WHEN** `OAuthLoginAsync("google", idToken: "google.id.token", accessToken: null)` is called and the API returns 200
- **THEN** the method SHALL store the access token, refresh token, and user info, and return `AuthResult.Success`

#### Scenario: OAuth login with invalid token
- **WHEN** `OAuthLoginAsync` is called and the API returns 401
- **THEN** the method SHALL return `AuthResult.Failure` with the error code

#### Scenario: OAuth login creates new account
- **WHEN** `OAuthLoginAsync` is called and the API returns 201 (new user)
- **THEN** the method SHALL store tokens and return `AuthResult.Success`
