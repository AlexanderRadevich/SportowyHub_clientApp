## ADDED Requirements

### Requirement: IAuthService interface
The app SHALL contain an `IAuthService` interface with the following methods:
- `Task<AuthResult<LoginResponse>> LoginAsync(string email, string password)`
- `Task<AuthResult<RegisterResponse>> RegisterAsync(string email, string password)`
- `Task<string?> GetTokenAsync()`
- `Task<UserInfo?> GetCurrentUserAsync()`
- `Task<bool> IsLoggedInAsync()`
- `Task ClearAuthAsync()`

#### Scenario: IAuthService is injectable
- **WHEN** a ViewModel constructor requests `IAuthService`
- **THEN** the DI container SHALL provide a singleton `AuthService` instance

### Requirement: LoginAsync calls API and stores token
`AuthService.LoginAsync` SHALL POST to `/api/v1/login` with email and password. On a 200 response, it SHALL store the JWT token in `SecureStorage` under key `auth_token` and the user info JSON under key `auth_user`, then return `AuthResult.Success(loginResponse)`.

#### Scenario: Successful login stores token and returns success
- **WHEN** `LoginAsync("user@example.com", "validpass")` is called and the API returns 200 with a JWT token
- **THEN** the token SHALL be stored in `SecureStorage["auth_token"]`, the user info SHALL be stored in `SecureStorage["auth_user"]`, and the method SHALL return an `AuthResult` with `IsSuccess=true`

#### Scenario: Login with invalid credentials returns failure
- **WHEN** `LoginAsync` is called and the API returns 401 with `{"error":{"code":"invalid_credentials","message":"Invalid email or password"}}`
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false` and `ErrorMessage` containing the server error message

#### Scenario: Login with network failure returns failure
- **WHEN** `LoginAsync` is called and the HTTP request throws due to no network connectivity
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false` and a user-friendly connection error message

### Requirement: RegisterAsync calls API and returns result
`AuthService.RegisterAsync` SHALL POST to `/api/v1/register` with email and password. On a 201 response, it SHALL return `AuthResult.Success(registerResponse)`. It SHALL NOT automatically log the user in — the caller decides the next step.

#### Scenario: Successful registration returns user data
- **WHEN** `RegisterAsync("new@example.com", "strongpass1!")` is called and the API returns 201
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=true` and `Data` containing the user's id, email, and trust_level

#### Scenario: Registration with taken email returns failure
- **WHEN** `RegisterAsync` is called and the API returns 409 with `{"error":{"code":"email_taken","message":"Email already in use"}}`
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false` and `ErrorMessage="Email already in use"`

#### Scenario: Registration with validation errors returns field errors
- **WHEN** `RegisterAsync` is called and the API returns 422 with validation details
- **THEN** the method SHALL return `AuthResult` with `IsSuccess=false` and `FieldErrors` populated from the response

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
`ClearAuthAsync` SHALL remove both `auth_token` and `auth_user` from `SecureStorage`.

#### Scenario: Clearing auth removes all stored credentials
- **WHEN** `ClearAuthAsync` is called
- **THEN** `SecureStorage["auth_token"]` and `SecureStorage["auth_user"]` SHALL be removed, and subsequent `IsLoggedInAsync` SHALL return false

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
