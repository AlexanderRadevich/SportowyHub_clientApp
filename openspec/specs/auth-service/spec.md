# Auth Service

### Requirement: IAuthService interface
The app SHALL contain an `IAuthService` interface with the following methods:
- `Task<AuthResult<LoginResponse>> LoginAsync(string email, string password)`
- `Task<AuthResult<RegisterResponse>> RegisterAsync(string email, string password, string passwordConfirm, string? phone = null)`
- `Task<AuthResult<ResendVerificationResponse>> ResendVerificationAsync(string email)`
- `Task<AuthResult<LoginResponse>> RefreshTokenAsync()`
- `Task<string?> GetTokenAsync()`
- `Task<UserInfo?> GetCurrentUserAsync()`
- `Task<bool> IsLoggedInAsync()`
- `Task ClearAuthAsync()`

#### Scenario: IAuthService is injectable
- **WHEN** a ViewModel constructor requests `IAuthService`
- **THEN** the DI container SHALL provide a singleton `AuthService` instance

### Requirement: LoginAsync calls API and stores token
`AuthService.LoginAsync` SHALL POST to `/api/v1/login` with email and password, including `X-Include-Refresh-Token: true` header. On a 200 response, it SHALL store the access token in `SecureStorage` under key `auth_token`, and if `refresh_token` is present store it under `auth_refresh_token`, and store the token expiry timestamp under `auth_token_expiry`, then return `AuthResult.Success(loginResponse)`. On a 401 response, it SHALL parse the error message and return `AuthResult.Failure` with the error code. On a 403 response, it SHALL parse the error code (`EMAIL_NOT_VERIFIED`, `USER_BANNED`) and return `AuthResult.Failure` with the `ErrorCode` populated. On a 422 response, it SHALL parse the `ApiError` body including `violations` and return `AuthResult.Failure` with `FieldErrors` populated from the violations. LoginAsync SHALL NOT store user info in SecureStorage (the login endpoint does not return user data).

#### Scenario: Successful login stores access token and returns success
- **WHEN** `LoginAsync("user@example.com", "validpass")` is called and the API returns 200 with an access token
- **THEN** the access token SHALL be stored in `SecureStorage["auth_token"]` and the method SHALL return an `AuthResult` with `IsSuccess=true`

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
- **WHEN** `LoginAsync` is called and the API returns 422 with `{"locale":"pl","error":{"code":"VALIDATION_FAILED","message":"Validation failed","violations":{"email":"Invalid email format"}}}`
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false`, `ErrorMessage` containing the server error message, and `FieldErrors` populated with the violations (e.g., key "email" -> "Invalid email format")

#### Scenario: Login with network failure returns failure
- **WHEN** `LoginAsync` is called and the HTTP request throws due to no network connectivity
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false` and a user-friendly connection error message

### Requirement: RegisterAsync calls API and returns result
`AuthService.RegisterAsync` SHALL POST to `/api/v1/register` with email, password, password_confirm, and optional phone. It SHALL pass all values to `RegisterRequest`. On a 201 response, it SHALL return `AuthResult.Success(registerResponse)`. It SHALL NOT automatically log the user in — the caller decides the next step. On a 409 response, it SHALL parse the error message from the `ApiError` body and return `AuthResult.Failure` with the error code. On a 422 response, it SHALL parse the `ApiError` body including `violations` and return `AuthResult.Failure` with `FieldErrors` populated from the violations.

#### Scenario: Successful registration returns user data
- **WHEN** `RegisterAsync("new@example.com", "strongpass1!", "strongpass1!", "+48123456789")` is called and the API returns 201
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=true` and `Data` containing the user's id, email, and trust_level

#### Scenario: Registration with taken email returns failure
- **WHEN** `RegisterAsync` is called and the API returns 409 with `{"locale":"pl","error":{"code":"email_taken","message":"Email already in use"}}`
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false`, `ErrorMessage="Email already in use"`, and `ErrorCode="email_taken"`

#### Scenario: Registration with validation errors returns field errors
- **WHEN** `RegisterAsync` is called and the API returns 422 with `{"locale":"pl","error":{"code":"VALIDATION_FAILED","message":"Validation failed","violations":{"password":"Password must be at least 8 characters"}}}`
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false`, `ErrorMessage="Validation failed"`, and `FieldErrors` containing key "password" with the server's violation message

#### Scenario: Registration with network failure returns failure
- **WHEN** `RegisterAsync` is called and the HTTP request throws due to no network connectivity
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false` and a user-friendly connection error message

### Requirement: GetTokenAsync retrieves stored JWT
`GetTokenAsync` SHALL read the JWT from `SecureStorage["auth_token"]`. If no token is stored, it SHALL return null.

#### Scenario: Token exists in storage
- **WHEN** `GetTokenAsync` is called after a successful login
- **THEN** it SHALL return the stored JWT string

#### Scenario: No token in storage
- **WHEN** `GetTokenAsync` is called with no prior login
- **THEN** it SHALL return null

### Requirement: GetCurrentUserAsync retrieves stored user info
`GetCurrentUserAsync` SHALL read user info JSON from `SecureStorage["auth_user"]` and deserialize it to a `UserInfo` object. If no user is stored, it SHALL return null.

#### Scenario: User info exists in storage
- **WHEN** `GetCurrentUserAsync` is called after a successful login
- **THEN** it SHALL return a `UserInfo` with the stored id, email, and trust_level

#### Scenario: No user in storage
- **WHEN** `GetCurrentUserAsync` is called with no prior login
- **THEN** it SHALL return null

### Requirement: IsLoggedInAsync checks for stored token
`IsLoggedInAsync` SHALL return true if `SecureStorage["auth_token"]` contains a non-empty value, false otherwise.

#### Scenario: User is logged in
- **WHEN** `IsLoggedInAsync` is called after a successful login
- **THEN** it SHALL return true

#### Scenario: User is not logged in
- **WHEN** `IsLoggedInAsync` is called with no stored token
- **THEN** it SHALL return false

### Requirement: ClearAuthAsync removes stored credentials
`ClearAuthAsync` SHALL remove `auth_token`, `auth_user`, `auth_refresh_token`, and `auth_token_expiry` from `SecureStorage`.

#### Scenario: Clearing auth removes all stored credentials
- **WHEN** `ClearAuthAsync` is called
- **THEN** `SecureStorage["auth_token"]`, `SecureStorage["auth_user"]`, `SecureStorage["auth_refresh_token"]`, and `SecureStorage["auth_token_expiry"]` SHALL be removed, and subsequent `IsLoggedInAsync` SHALL return false

### Requirement: DI registration for auth service
`MauiProgram.cs` SHALL register `IAuthService` as a singleton backed by `AuthService`.

#### Scenario: AuthService resolves from DI
- **WHEN** a ViewModel requests `IAuthService` from the DI container
- **THEN** it SHALL receive a singleton `AuthService` instance

### Requirement: Startup auth check
On app startup, `App.xaml.cs` SHALL check `IAuthService.IsLoggedInAsync()`. If true, the app SHALL navigate to the main shell (Home tab). If false, the app SHALL navigate to the Login page.

#### Scenario: App starts with stored token
- **WHEN** the app launches and a JWT token exists in SecureStorage
- **THEN** the app SHALL display the main tab bar with Home tab active, skipping the login screen

#### Scenario: App starts without stored token
- **WHEN** the app launches and no JWT token exists in SecureStorage
- **THEN** the app SHALL navigate to the Login page

### Requirement: ResendVerificationAsync calls API
`AuthService.ResendVerificationAsync` SHALL POST to `/api/v1/resend-verification` with the email. On a 200 response, it SHALL return `AuthResult.Success(response)`. On failure, it SHALL parse the error and return `AuthResult.Failure` with the error code.

#### Scenario: Successful resend returns success
- **WHEN** `ResendVerificationAsync("user@example.com")` is called and the API returns 200
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=true`

#### Scenario: Resend with invalid email returns failure
- **WHEN** `ResendVerificationAsync` is called and the API returns 404
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false` and an error message

#### Scenario: Resend with network failure returns failure
- **WHEN** `ResendVerificationAsync` is called and the HTTP request throws
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false` and a user-friendly connection error message

### Requirement: ServiceAuthenticationException carries status code
`ServiceAuthenticationException` SHALL include an `HttpStatusCode StatusCode` property so callers can distinguish 401 from 403 responses.

#### Scenario: 401 response carries Unauthorized status
- **WHEN** `HandleResponse` receives a 401 response
- **THEN** the thrown `ServiceAuthenticationException` SHALL have `StatusCode = HttpStatusCode.Unauthorized`

#### Scenario: 403 response carries Forbidden status
- **WHEN** `HandleResponse` receives a 403 response
- **THEN** the thrown `ServiceAuthenticationException` SHALL have `StatusCode = HttpStatusCode.Forbidden`

### Requirement: UpdateProfileAsync on auth service
`IAuthService` SHALL expose a `Task<UserProfile?> UpdateProfileAsync(UpdateProfileRequest request)` method. `AuthService` SHALL implement it by reading the access token from SecureStorage and calling `PUT /api/private/profile` with Bearer authorization and the request body. The method SHALL return the updated `UserProfile` on success and let exceptions propagate to the caller.

#### Scenario: UpdateProfileAsync returns updated profile on success
- **WHEN** `UpdateProfileAsync` is called with valid data and the user has a valid token
- **THEN** the method SHALL return the updated `UserProfile` from the API response

#### Scenario: UpdateProfileAsync returns null when no token exists
- **WHEN** `UpdateProfileAsync` is called and no token is stored
- **THEN** the method SHALL return `null`

#### Scenario: UpdateProfileAsync propagates API errors
- **WHEN** the API returns a 422 validation error
- **THEN** the exception SHALL propagate to the caller with the error response body for field-level error parsing
