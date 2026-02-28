# Auth API Models

### Requirement: LoginRequest model
The app SHALL contain a `LoginRequest` record with `Email` (string) and `Password` (string) properties. It SHALL serialize to JSON matching the API schema: `{"email": "...", "password": "..."}`.

#### Scenario: LoginRequest serializes to API format
- **WHEN** a `LoginRequest` with Email="user@example.com" and Password="secret123" is serialized
- **THEN** the JSON output SHALL be `{"email":"user@example.com","password":"secret123"}`

### Requirement: LoginResponse model
The app SHALL contain a `LoginResponse` record with `AccessToken` (string), `ExpiresIn` (int), `TokenType` (string), `RefreshToken` (string, nullable), and `Locale` (string, nullable) properties. It SHALL deserialize from the API response: `{"locale": "pl", "access_token": "...", "expires_in": 3600, "token_type": "Bearer", "refresh_token": "..."}`. The `RefreshToken` field is optional and only present when the request includes `X-Include-Refresh-Token: true` header.

#### Scenario: LoginResponse deserializes from API format
- **WHEN** the API returns `{"locale":"pl","access_token":"jwt.token.here","expires_in":3600,"token_type":"Bearer","refresh_token":"rt.token"}`
- **THEN** it SHALL deserialize to a `LoginResponse` with AccessToken="jwt.token.here", ExpiresIn=3600, TokenType="Bearer", RefreshToken="rt.token", and Locale="pl"

#### Scenario: LoginResponse deserializes without refresh token
- **WHEN** the API returns `{"locale":"pl","access_token":"jwt.token.here","expires_in":3600,"token_type":"Bearer"}`
- **THEN** it SHALL deserialize to a `LoginResponse` with RefreshToken=null

#### Scenario: LoginResponse deserializes without locale
- **WHEN** the API returns a response without the `locale` field
- **THEN** `Locale` SHALL be null, and `AccessToken`, `ExpiresIn`, `TokenType` SHALL deserialize normally

### Requirement: RegisterRequest model
The app SHALL contain a `RegisterRequest` record with `Email` (string), `Password` (string), `PasswordConfirm` (string), and `Phone` (string, nullable) properties. It SHALL serialize to JSON matching the API schema: `{"email": "...", "password": "...", "password_confirm": "...", "phone": "..."}`.

#### Scenario: RegisterRequest serializes with all fields
- **WHEN** a `RegisterRequest` with Email="user@example.com", Password="secret123", PasswordConfirm="secret123", Phone="+48123456789" is serialized
- **THEN** the JSON output SHALL include `"email"`, `"password"`, `"password_confirm"`, and `"phone"` fields

#### Scenario: RegisterRequest serializes without optional phone
- **WHEN** a `RegisterRequest` with Phone=null is serialized
- **THEN** the JSON output SHALL include `email`, `password`, and `password_confirm` fields, and `phone` SHALL be null or omitted

### Requirement: RegisterResponse model
The app SHALL contain a `RegisterResponse` record with `Id` (int), `Email` (string), `TrustLevel` (string), and `Locale` (string, nullable) properties. It SHALL deserialize from the API 201 response.

#### Scenario: RegisterResponse deserializes from API format
- **WHEN** the API returns `{"locale":"en","id":42,"email":"user@example.com","trust_level":"TL0"}`
- **THEN** it SHALL deserialize to a `RegisterResponse` with Id=42, TrustLevel="TL0", and Locale="en"

### Requirement: ApiError model
The app SHALL contain an `ApiError` record with a `Locale` (string, nullable) property and a nested `Error` object (`ErrorDetail`). `ErrorDetail` SHALL have `Code` (string), `Message` (string), and `Violations` (Dictionary<string, string>, nullable) properties. It SHALL deserialize from both simple error responses and validation error responses.

#### Scenario: ApiError deserializes from 409 response
- **WHEN** the API returns a 409 with `{"locale":"pl","error":{"code":"email_taken","message":"Email already in use"}}`
- **THEN** it SHALL deserialize to an `ApiError` with Locale="pl", Error.Code="email_taken", Error.Message="Email already in use", and Error.Violations=null

#### Scenario: ApiError deserializes from 401 response
- **WHEN** the API returns a 401 with `{"locale":"pl","error":{"code":"INVALID_CREDENTIALS","message":"Invalid email or password"}}`
- **THEN** it SHALL deserialize to an `ApiError` with Error.Code="INVALID_CREDENTIALS" and Error.Violations=null

#### Scenario: ApiError deserializes from 422 validation response
- **WHEN** the API returns a 422 with `{"locale":"pl","error":{"code":"VALIDATION_FAILED","message":"Validation failed","violations":{"email":"Invalid email format","password":"Password must be at least 8 characters"}}}`
- **THEN** it SHALL deserialize to an `ApiError` with Error.Code="VALIDATION_FAILED", Error.Message="Validation failed", and Error.Violations containing keys "email" and "password" with their respective messages

#### Scenario: ApiError deserializes with empty violations
- **WHEN** the API returns a 422 where `violations` is an empty object `{}`
- **THEN** Error.Violations SHALL be an empty dictionary

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

### Requirement: ResendVerificationRequest model
The app SHALL contain a `ResendVerificationRequest` record with `Email` (string) property. It SHALL serialize to JSON: `{"email": "..."}`.

#### Scenario: ResendVerificationRequest serializes to API format
- **WHEN** a `ResendVerificationRequest` with Email="user@example.com" is serialized
- **THEN** the JSON output SHALL be `{"email":"user@example.com"}`

### Requirement: ResendVerificationResponse model
The app SHALL contain a `ResendVerificationResponse` record with `Message` (string) and `Locale` (string, nullable) properties. It SHALL deserialize from the API response.

#### Scenario: ResendVerificationResponse deserializes from API format
- **WHEN** the API returns `{"locale":"en","message":"Verification email sent"}`
- **THEN** it SHALL deserialize to a `ResendVerificationResponse` with Message="Verification email sent" and Locale="en"

### Requirement: AuthResult ErrorCode property
The `AuthResult<T>` record SHALL include an `ErrorCode` (string, nullable) property. The `Failure` factory method SHALL accept an optional `errorCode` parameter. `ErrorCode` SHALL be populated from `ApiError.Error.Code` when parsing API error responses.

#### Scenario: AuthResult carries error code
- **WHEN** `AuthResult.Failure("Email not verified", errorCode: "EMAIL_NOT_VERIFIED")` is called
- **THEN** `ErrorCode` SHALL be "EMAIL_NOT_VERIFIED" and `ErrorMessage` SHALL be "Email not verified"

#### Scenario: Successful AuthResult has null error code
- **WHEN** `AuthResult.Success(data)` is called
- **THEN** `ErrorCode` SHALL be null

### Requirement: JSON context registrations for new models
`SportowyHubJsonContext` SHALL include `[JsonSerializable]` attributes for `ResendVerificationRequest` and `ResendVerificationResponse`.

#### Scenario: New models are serializable via source generation
- **WHEN** `ResendVerificationRequest` or `ResendVerificationResponse` is serialized/deserialized
- **THEN** the operation SHALL succeed using the source-generated JSON context

### Requirement: UserAccount nested model
The app SHALL define a `UserAccount` record with properties: `FirstName` (string?), `LastName` (string?), `FullName` (string?), `AvatarUrl` (string?), `AvatarThumbnailUrl` (string?), `NotificationsEnabled` (bool), `QuietHoursStart` (string?), `QuietHoursEnd` (string?), `Locale` (string?), `BalanceGrosze` (int), `BalanceUpdatedAt` (string?). The type SHALL be registered in `SportowyHubJsonContext` for source-generated serialization with `SnakeCaseLower` naming policy.

#### Scenario: UserAccount deserializes from nested account object
- **WHEN** the API returns a profile response with `"account": {"first_name": "John", "last_name": "Doe", "full_name": "John Doe", "balance_grosze": 1500, "notifications_enabled": true}`
- **THEN** the `UserAccount` record SHALL contain FirstName="John", LastName="Doe", FullName="John Doe", BalanceGrosze=1500, NotificationsEnabled=true

#### Scenario: UserAccount handles all-null optional fields
- **WHEN** the API returns an account object with all nullable fields as null
- **THEN** the corresponding C# properties SHALL be `null`, and `BalanceGrosze` SHALL be 0, `NotificationsEnabled` SHALL deserialize to its actual value

### Requirement: OauthLinked nested model
The app SHALL define an `OauthLinked` record with property `Google` (bool). The type SHALL be registered in `SportowyHubJsonContext` for source-generated serialization with `SnakeCaseLower` naming policy.

#### Scenario: OauthLinked deserializes from API response
- **WHEN** the API returns `"oauth_linked": {"google": true}`
- **THEN** the `OauthLinked` record SHALL contain Google=true

#### Scenario: OauthLinked with no providers linked
- **WHEN** the API returns `"oauth_linked": {"google": false}`
- **THEN** the `OauthLinked` record SHALL contain Google=false

### Requirement: JSON context registrations for new profile models
`SportowyHubJsonContext` SHALL include `[JsonSerializable]` attributes for `UserAccount` and `OauthLinked`.

#### Scenario: New models are serializable via source generation
- **WHEN** `UserAccount` or `OauthLinked` is deserialized via the source-generated JSON context
- **THEN** the operation SHALL succeed without runtime reflection

### Requirement: UpdateProfileRequest model
The app SHALL define an `UpdateProfileRequest` record with properties: `Phone` (string?) and `Account` (UpdateProfileAccountRequest?). The type SHALL be registered in `SportowyHubJsonContext` for source-generated serialization with `SnakeCaseLower` naming policy.

#### Scenario: UpdateProfileRequest serializes to API format
- **WHEN** an `UpdateProfileRequest` with Phone="123456789" and Account with FirstName="John" is serialized
- **THEN** the JSON output SHALL be `{"phone":"123456789","account":{"first_name":"John",...}}`

#### Scenario: UpdateProfileRequest with null optional fields
- **WHEN** an `UpdateProfileRequest` with Phone=null is serialized
- **THEN** `phone` SHALL be null in the JSON output

### Requirement: UpdateProfileAccountRequest model
The app SHALL define an `UpdateProfileAccountRequest` record with properties: `FirstName` (string?), `LastName` (string?), `NotificationsEnabled` (bool), `QuietHoursStart` (string?), `QuietHoursEnd` (string?). The type SHALL be registered in `SportowyHubJsonContext` for source-generated serialization with `SnakeCaseLower` naming policy.

#### Scenario: UpdateProfileAccountRequest serializes nested fields
- **WHEN** an `UpdateProfileAccountRequest` with FirstName="John", LastName="Doe", NotificationsEnabled=true is serialized
- **THEN** the JSON output SHALL include `"first_name":"John"`, `"last_name":"Doe"`, `"notifications_enabled":true`

### Requirement: PutAsync with separate request and response types
`IRequestProvider` SHALL expose a `PutAsync<TRequest, TResponse>` method that accepts a request body of type `TRequest` and returns a response of type `TResponse`. The implementation SHALL follow the same pattern as `PostAsync<TRequest, TResponse>`.

#### Scenario: PutAsync sends request type and deserializes response type
- **WHEN** `PutAsync<UpdateProfileRequest, UserProfile>` is called
- **THEN** the request SHALL be serialized as `UpdateProfileRequest` and the response SHALL be deserialized as `UserProfile`
