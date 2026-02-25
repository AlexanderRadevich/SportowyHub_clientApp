## MODIFIED Requirements

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

## ADDED Requirements

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

## REMOVED Requirements

### Requirement: LoginResponse model
**Reason**: Replaced by the MODIFIED LoginResponse above. The old model had `Token` (string) and `User` (UserInfo) fields that do not match the actual backend API response. The backend returns `access_token`, `expires_in`, `token_type`, and optionally `refresh_token` — no `user` object.
**Migration**: Update all code referencing `response.Token` to use `response.AccessToken`. Remove references to `response.User` — user info is no longer returned by the login endpoint.
