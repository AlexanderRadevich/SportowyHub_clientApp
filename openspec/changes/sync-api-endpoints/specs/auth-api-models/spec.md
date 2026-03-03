## MODIFIED Requirements

### Requirement: LoginResponse model
The app SHALL contain a `LoginResponse` record with `AccessToken` (string), `ExpiresIn` (int), `TokenType` (string), `RefreshToken` (string, nullable), `Locale` (string, nullable), and `User` (LoginUser, nullable) properties. It SHALL deserialize from the API response: `{"locale": "pl", "access_token": "...", "expires_in": 3600, "token_type": "Bearer", "refresh_token": "...", "user": {"id": 1, "email": "...", "trust_level": "TL1"}}`. The `RefreshToken` field is optional and only present when the request includes `X-Include-Refresh-Token: true` header. The `User` field is optional.

#### Scenario: LoginResponse deserializes from API format
- **WHEN** the API returns `{"locale":"pl","access_token":"jwt.token.here","expires_in":3600,"token_type":"Bearer","refresh_token":"rt.token","user":{"id":1,"email":"user@example.com","trust_level":"TL1"}}`
- **THEN** it SHALL deserialize to a `LoginResponse` with AccessToken="jwt.token.here", ExpiresIn=3600, TokenType="Bearer", RefreshToken="rt.token", Locale="pl", and User with Id=1, Email="user@example.com", TrustLevel="TL1"

#### Scenario: LoginResponse deserializes without refresh token
- **WHEN** the API returns `{"locale":"pl","access_token":"jwt.token.here","expires_in":3600,"token_type":"Bearer"}`
- **THEN** it SHALL deserialize to a `LoginResponse` with RefreshToken=null

#### Scenario: LoginResponse deserializes without locale
- **WHEN** the API returns a response without the `locale` field
- **THEN** `Locale` SHALL be null, and `AccessToken`, `ExpiresIn`, `TokenType` SHALL deserialize normally

#### Scenario: LoginResponse deserializes without user object
- **WHEN** the API returns a response without the `user` field
- **THEN** `User` SHALL be null

### Requirement: RegisterResponse model
The app SHALL contain a `RegisterResponse` record with `Id` (int), `Email` (string), `TrustLevel` (string), `Locale` (string, nullable), and `VerificationUrl` (string, nullable) properties. It SHALL deserialize from the API 201 response.

#### Scenario: RegisterResponse deserializes from API format
- **WHEN** the API returns `{"locale":"en","id":42,"email":"user@example.com","trust_level":"TL0"}`
- **THEN** it SHALL deserialize to a `RegisterResponse` with Id=42, TrustLevel="TL0", and Locale="en"

#### Scenario: RegisterResponse deserializes with verification URL
- **WHEN** the API returns `{"id":42,"email":"user@example.com","trust_level":"TL0","verification_url":"https://example.com/verify?token=abc"}`
- **THEN** it SHALL deserialize to a `RegisterResponse` with VerificationUrl="https://example.com/verify?token=abc"

### Requirement: UpdateProfileRequest model
The app SHALL define an `UpdateProfileRequest` record with properties: `Phone` (string?), `Locale` (string?), and `Account` (UpdateProfileAccountRequest?). The type SHALL be registered in `SportowyHubJsonContext` for source-generated serialization with `SnakeCaseLower` naming policy.

#### Scenario: UpdateProfileRequest serializes to API format
- **WHEN** an `UpdateProfileRequest` with Phone="123456789", Locale="en", and Account with FirstName="John" is serialized
- **THEN** the JSON output SHALL include `"phone":"123456789"`, `"locale":"en"`, and nested account object

#### Scenario: UpdateProfileRequest with null optional fields
- **WHEN** an `UpdateProfileRequest` with Phone=null and Locale=null is serialized
- **THEN** `phone` and `locale` SHALL be null in the JSON output

### Requirement: PatchAsync with separate request and response types
`IRequestProvider` SHALL expose a `PatchAsync<TRequest, TResponse>` method that accepts a URI, request body of type `TRequest`, and an optional auth token, then returns a response of type `TResponse`. The implementation SHALL follow the same pattern as `PostAsync<TRequest, TResponse>` but use `HttpMethod.Patch`.

#### Scenario: PatchAsync sends PATCH request and deserializes response
- **WHEN** `PatchAsync<UpdateProfileRequest, UserProfile>` is called with a URI and body
- **THEN** the request SHALL use HTTP PATCH method, serialize the body as JSON, and deserialize the response as the target type

## ADDED Requirements

### Requirement: LoginUser nested model
The app SHALL define a `LoginUser` record with properties: `Id` (int), `Email` (string), `TrustLevel` (string). The type SHALL be registered in `SportowyHubJsonContext` for source-generated serialization with `SnakeCaseLower` naming policy.

#### Scenario: LoginUser deserializes from nested user object
- **WHEN** the API returns a login response with `"user": {"id": 1, "email": "user@example.com", "trust_level": "TL1"}`
- **THEN** the `LoginUser` record SHALL contain Id=1, Email="user@example.com", TrustLevel="TL1"

### Requirement: JSON context registrations for LoginUser
`SportowyHubJsonContext` SHALL include a `[JsonSerializable]` attribute for `LoginUser`.

#### Scenario: LoginUser is deserializable via source generation
- **WHEN** `LoginUser` is deserialized via the source-generated JSON context
- **THEN** the operation SHALL succeed without runtime reflection
