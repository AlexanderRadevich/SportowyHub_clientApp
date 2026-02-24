## ADDED Requirements

### Requirement: LoginRequest model
The app SHALL contain a `LoginRequest` record with `Email` (string) and `Password` (string) properties. It SHALL serialize to JSON matching the API schema: `{"email": "...", "password": "..."}`.

#### Scenario: LoginRequest serializes to API format
- **WHEN** a `LoginRequest` with Email="user@example.com" and Password="secret123" is serialized
- **THEN** the JSON output SHALL be `{"email":"user@example.com","password":"secret123"}`

### Requirement: LoginResponse model
The app SHALL contain a `LoginResponse` record with `Token` (string) and `User` (`UserInfo`) properties. `UserInfo` SHALL have `Id` (int), `Email` (string), and `TrustLevel` (string) properties. It SHALL deserialize from the API response: `{"token": "...", "user": {"id": 1, "email": "...", "trust_level": "TL0"}}`.

#### Scenario: LoginResponse deserializes from API format
- **WHEN** the API returns `{"token":"jwt.token.here","user":{"id":1,"email":"user@example.com","trust_level":"TL0"}}`
- **THEN** it SHALL deserialize to a `LoginResponse` with Token="jwt.token.here" and User.TrustLevel="TL0"

### Requirement: RegisterRequest model
The app SHALL contain a `RegisterRequest` record with `Email` (string), `Password` (string), and `Phone` (string, nullable) properties. It SHALL serialize to JSON matching the API schema.

#### Scenario: RegisterRequest serializes without optional phone
- **WHEN** a `RegisterRequest` with Email="user@example.com", Password="secret123", Phone=null is serialized
- **THEN** the JSON output SHALL include `email` and `password` fields, and `phone` SHALL be null or omitted

#### Scenario: RegisterRequest serializes with phone
- **WHEN** a `RegisterRequest` with Phone="+48123456789" is serialized
- **THEN** the JSON output SHALL include `"phone":"+48123456789"`

### Requirement: RegisterResponse model
The app SHALL contain a `RegisterResponse` record with `Id` (int), `Email` (string), and `TrustLevel` (string) properties. It SHALL deserialize from the API 201 response.

#### Scenario: RegisterResponse deserializes from API format
- **WHEN** the API returns `{"id":42,"email":"user@example.com","trust_level":"TL0"}`
- **THEN** it SHALL deserialize to a `RegisterResponse` with Id=42 and TrustLevel="TL0"

### Requirement: ApiError model
The app SHALL contain an `ApiError` record matching the API error schema: a nested `Error` object with `Code` (string) and `Message` (string) properties. It SHALL deserialize from error responses like `{"error":{"code":"email_taken","message":"Email already in use"}}`.

#### Scenario: ApiError deserializes from 409 response
- **WHEN** the API returns a 409 with `{"error":{"code":"email_taken","message":"Email already in use"}}`
- **THEN** it SHALL deserialize to an `ApiError` with Error.Code="email_taken" and Error.Message="Email already in use"

#### Scenario: ApiError deserializes from 401 response
- **WHEN** the API returns a 401 with `{"error":{"code":"invalid_credentials","message":"Invalid email or password"}}`
- **THEN** it SHALL deserialize to an `ApiError` with Error.Code="invalid_credentials"

### Requirement: AuthResult generic result wrapper
The app SHALL contain an `AuthResult<T>` record with `IsSuccess` (bool), `Data` (T?, nullable), `ErrorMessage` (string, nullable), and `FieldErrors` (Dictionary<string, string>, nullable) properties. A static `Success(T data)` factory method SHALL create a successful result. A static `Failure(string message, Dictionary<string, string>? fieldErrors)` factory method SHALL create a failed result.

#### Scenario: Successful AuthResult carries data
- **WHEN** `AuthResult<LoginResponse>.Success(loginResponse)` is called
- **THEN** `IsSuccess` SHALL be true, `Data` SHALL contain the response, and `ErrorMessage` SHALL be null

#### Scenario: Failed AuthResult carries error message
- **WHEN** `AuthResult<LoginResponse>.Failure("Invalid credentials")` is called
- **THEN** `IsSuccess` SHALL be false, `Data` SHALL be null, and `ErrorMessage` SHALL be "Invalid credentials"

#### Scenario: Failed AuthResult carries field-level errors
- **WHEN** `AuthResult.Failure("Validation failed", new Dictionary{{"Email", "Email already taken"}})` is called
- **THEN** `FieldErrors["Email"]` SHALL be "Email already taken"
