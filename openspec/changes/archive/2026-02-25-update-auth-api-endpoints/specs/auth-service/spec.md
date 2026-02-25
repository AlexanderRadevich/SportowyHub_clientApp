## MODIFIED Requirements

### Requirement: LoginAsync calls API and stores token
`AuthService.LoginAsync` SHALL POST to `/api/v1/login` with email and password. On a 200 response, it SHALL store the JWT token in `SecureStorage` under key `auth_token` and the user info JSON under key `auth_user`, then return `AuthResult.Success(loginResponse)`. On a 401 response, it SHALL parse the error message from the `ApiError` body and return `AuthResult.Failure`. On a 422 response, it SHALL parse the `ApiError` body including `violations` and return `AuthResult.Failure` with `FieldErrors` populated from the violations.

#### Scenario: Successful login stores token and returns success
- **WHEN** `LoginAsync("user@example.com", "validpass")` is called and the API returns 200 with a JWT token
- **THEN** the token SHALL be stored in `SecureStorage["auth_token"]`, the user info SHALL be stored in `SecureStorage["auth_user"]`, and the method SHALL return an `AuthResult` with `IsSuccess=true`

#### Scenario: Login with invalid credentials returns failure
- **WHEN** `LoginAsync` is called and the API returns 401 with `{"locale":"pl","error":{"code":"INVALID_CREDENTIALS","message":"Invalid email or password"}}`
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false` and `ErrorMessage` containing the server error message

#### Scenario: Login with validation errors returns field errors
- **WHEN** `LoginAsync` is called and the API returns 422 with `{"locale":"pl","error":{"code":"VALIDATION_FAILED","message":"Validation failed","violations":{"email":"Invalid email format"}}}`
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false`, `ErrorMessage` containing the server error message, and `FieldErrors` populated with the violations (e.g., key "email" → "Invalid email format")

#### Scenario: Login with network failure returns failure
- **WHEN** `LoginAsync` is called and the HTTP request throws due to no network connectivity
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false` and a user-friendly connection error message

### Requirement: IAuthService interface
The app SHALL contain an `IAuthService` interface with the following methods:
- `Task<AuthResult<LoginResponse>> LoginAsync(string email, string password)`
- `Task<AuthResult<RegisterResponse>> RegisterAsync(string email, string password, string? phone = null)`
- `Task<string?> GetTokenAsync()`
- `Task<UserInfo?> GetCurrentUserAsync()`
- `Task<bool> IsLoggedInAsync()`
- `Task ClearAuthAsync()`

#### Scenario: IAuthService is injectable
- **WHEN** a ViewModel constructor requests `IAuthService`
- **THEN** the DI container SHALL provide a singleton `AuthService` instance

### Requirement: RegisterAsync calls API and returns result
`AuthService.RegisterAsync` SHALL POST to `/api/v1/register` with email, password, and optional phone. It SHALL pass the phone value to `RegisterRequest`. On a 201 response, it SHALL return `AuthResult.Success(registerResponse)`. It SHALL NOT automatically log the user in — the caller decides the next step. On a 409 response, it SHALL parse the error message from the `ApiError` body. On a 422 response, it SHALL parse the `ApiError` body including `violations` and return `AuthResult.Failure` with `FieldErrors` populated from the violations.

#### Scenario: Successful registration returns user data
- **WHEN** `RegisterAsync("new@example.com", "strongpass1!", "+48123456789")` is called and the API returns 201
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=true` and `Data` containing the user's id, email, and trust_level

#### Scenario: Registration with taken email returns failure
- **WHEN** `RegisterAsync` is called and the API returns 409 with `{"locale":"pl","error":{"code":"email_taken","message":"Email already in use"}}`
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false` and `ErrorMessage="Email already in use"`

#### Scenario: Registration with validation errors returns field errors
- **WHEN** `RegisterAsync` is called and the API returns 422 with `{"locale":"pl","error":{"code":"VALIDATION_FAILED","message":"Validation failed","violations":{"password":"Password must be at least 8 characters"}}}`
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false`, `ErrorMessage="Validation failed"`, and `FieldErrors` containing key "password" with the server's violation message

#### Scenario: Registration with network failure returns failure
- **WHEN** `RegisterAsync` is called and the HTTP request throws due to no network connectivity
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false` and a user-friendly connection error message
