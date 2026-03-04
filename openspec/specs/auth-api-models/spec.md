# Auth API Models

### Requirement: LoginRequest model
The app SHALL contain a `LoginRequest` record with `Email` (string) and `Password` (string) properties. It SHALL serialize to JSON matching the API schema: `{"email": "...", "password": "..."}`.

#### Scenario: LoginRequest serializes to API format
- **WHEN** a `LoginRequest` with Email="user@example.com" and Password="secret123" is serialized
- **THEN** the JSON output SHALL be `{"email":"user@example.com","password":"secret123"}`

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

### Requirement: RegisterRequest model
The app SHALL contain a `RegisterRequest` record with `Email` (string), `Password` (string), `PasswordConfirm` (string), and `Phone` (string, nullable) properties. It SHALL serialize to JSON matching the API schema: `{"email": "...", "password": "...", "password_confirm": "...", "phone": "..."}`.

#### Scenario: RegisterRequest serializes with all fields
- **WHEN** a `RegisterRequest` with Email="user@example.com", Password="secret123", PasswordConfirm="secret123", Phone="+48123456789" is serialized
- **THEN** the JSON output SHALL include `"email"`, `"password"`, `"password_confirm"`, and `"phone"` fields

#### Scenario: RegisterRequest serializes without optional phone
- **WHEN** a `RegisterRequest` with Phone=null is serialized
- **THEN** the JSON output SHALL include `email`, `password`, and `password_confirm` fields, and `phone` SHALL be null or omitted

### Requirement: RegisterResponse model
The app SHALL contain a `RegisterResponse` record with `Id` (int), `Email` (string), `TrustLevel` (string), `Locale` (string, nullable), and `VerificationUrl` (string, nullable) properties. It SHALL deserialize from the API 201 response.

#### Scenario: RegisterResponse deserializes from API format
- **WHEN** the API returns `{"locale":"en","id":42,"email":"user@example.com","trust_level":"TL0"}`
- **THEN** it SHALL deserialize to a `RegisterResponse` with Id=42, TrustLevel="TL0", and Locale="en"

#### Scenario: RegisterResponse deserializes with verification URL
- **WHEN** the API returns `{"id":42,"email":"user@example.com","trust_level":"TL0","verification_url":"https://example.com/verify?token=abc"}`
- **THEN** it SHALL deserialize to a `RegisterResponse` with VerificationUrl="https://example.com/verify?token=abc"

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

### Requirement: UserProfile model
The app SHALL define a `UserProfile` record with properties: `Id` (int), `Email` (string), `EmailVerified` (bool), `EmailVerifiedAt` (string?), `Phone` (string?), `PhoneVerified` (bool), `PhoneVerifiedAt` (string?), `TrustLevel` (string), `ReputationScore` (decimal, with `[JsonConverter(typeof(FlexibleDecimalConverter))]`), `OauthLinked` (OauthLinked?), `LastLoginAt` (string?), `LastActivityAt` (string?), `Account` (UserAccount?). The type SHALL NOT have a top-level `Locale` property — locale is accessible only via `Account.Locale`. The type SHALL be registered in `SportowyHubJsonContext` for source-generated serialization with `SnakeCaseLower` naming policy.

#### Scenario: UserProfile deserializes from GET /api/private/profile
- **WHEN** the API returns a profile response with `"reputation_score": 4.75`
- **THEN** `UserProfile.ReputationScore` SHALL be `4.75m`

#### Scenario: UserProfile deserializes integer reputation score
- **WHEN** the API returns a profile response with `"reputation_score": 5`
- **THEN** `UserProfile.ReputationScore` SHALL be `5m`

#### Scenario: UserProfile has no top-level Locale property
- **WHEN** the API returns a profile response with `"account": {"locale": "pl"}`
- **THEN** locale SHALL be accessible only via `UserProfile.Account.Locale`, and no top-level `Locale` property SHALL exist

#### Scenario: Nullable nested objects handled correctly
- **WHEN** the API returns `"account": null` or `"oauth_linked": null`
- **THEN** `Account` SHALL be null and `OauthLinked` SHALL be null respectively

### Requirement: UpdateProfileRequest model
The app SHALL define an `UpdateProfileRequest` record with properties: `Phone` (string?), `Locale` (string?), and `Account` (UpdateProfileAccountRequest?). The type SHALL be registered in `SportowyHubJsonContext` for source-generated serialization with `SnakeCaseLower` naming policy.

#### Scenario: UpdateProfileRequest serializes to API format
- **WHEN** an `UpdateProfileRequest` with Phone="123456789", Locale="en", and Account with FirstName="John" is serialized
- **THEN** the JSON output SHALL include `"phone":"123456789"`, `"locale":"en"`, and nested account object

#### Scenario: UpdateProfileRequest with null optional fields
- **WHEN** an `UpdateProfileRequest` with Phone=null and Locale=null is serialized
- **THEN** `phone` and `locale` SHALL be null in the JSON output

### Requirement: UpdateProfileAccountRequest model
The app SHALL define an `UpdateProfileAccountRequest` record with properties: `FirstName` (string?), `LastName` (string?), `NotificationsEnabled` (bool?), `QuietHoursStart` (string?), `QuietHoursEnd` (string?). The type SHALL be registered in `SportowyHubJsonContext` for source-generated serialization with `SnakeCaseLower` naming policy. When `NotificationsEnabled` is `null`, the field SHALL serialize as `null` in JSON, which the backend treats as no-op.

#### Scenario: UpdateProfileAccountRequest serializes nested fields
- **WHEN** an `UpdateProfileAccountRequest` with FirstName="John", LastName="Doe", NotificationsEnabled=true is serialized
- **THEN** the JSON output SHALL include `"first_name":"John"`, `"last_name":"Doe"`, `"notifications_enabled":true`

#### Scenario: UpdateProfileAccountRequest omits notifications when null
- **WHEN** an `UpdateProfileAccountRequest` with NotificationsEnabled=null is serialized
- **THEN** the JSON output SHALL include `"notifications_enabled":null`, and the backend SHALL treat this as no change

### Requirement: PutAsync with separate request and response types
`IRequestProvider` SHALL expose a `PutAsync<TRequest, TResponse>` method that accepts a request body of type `TRequest` and returns a response of type `TResponse`. The implementation SHALL follow the same pattern as `PostAsync<TRequest, TResponse>`.

#### Scenario: PutAsync sends request type and deserializes response type
- **WHEN** `PutAsync<UpdateProfileRequest, UserProfile>` is called
- **THEN** the request SHALL be serialized as `UpdateProfileRequest` and the response SHALL be deserialized as `UserProfile`

### Requirement: PatchAsync with separate request and response types
`IRequestProvider` SHALL expose a `PatchAsync<TRequest, TResponse>` method that accepts a URI, request body of type `TRequest`, and an optional auth token, then returns a response of type `TResponse`. The implementation SHALL follow the same pattern as `PostAsync<TRequest, TResponse>` but use `HttpMethod.Patch`.

#### Scenario: PatchAsync sends PATCH request and deserializes response
- **WHEN** `PatchAsync<UpdateProfileRequest, UserProfile>` is called with a URI and body
- **THEN** the request SHALL use HTTP PATCH method, serialize the body as JSON, and deserialize the response as the target type

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

### Requirement: UpdateProfileResponse model
The app SHALL define an `UpdateProfileResponse` record with a single property: `Account` (`UserAccount?`). This record SHALL match the actual PATCH `/api/private/profile` response shape, which returns only the `account` object. The type SHALL be registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize PATCH profile response
- **WHEN** the backend returns `{"account": {"first_name": "John", "last_name": "Doe", "notifications_enabled": true, ...}}`
- **THEN** `UpdateProfileResponse.Account` SHALL contain the deserialized `UserAccount` with all fields populated

#### Scenario: UpdateProfileAsync returns UpdateProfileResponse
- **WHEN** `AuthService.UpdateProfileAsync` is called
- **THEN** the PATCH response SHALL be deserialized as `UpdateProfileResponse` instead of `UserProfile`

### Requirement: FavoriteActionResponse includes favorites count
The `FavoriteActionResponse` record SHALL include a `FavoritesCount` field of type `int?` that reflects the total number of favorites for the user after the add operation, as returned by the backend `POST /api/private/favorites/{listingId}` response field `favorites_count`.

#### Scenario: Add favorite returns favorites count
- **GIVEN** the user is authenticated and the listing is published
- **WHEN** `FavoritesService.AddAsync` receives a successful `201` response from the backend
- **THEN** the deserialized `FavoriteActionResponse` SHALL have `FavoritesCount` populated with the integer value of `favorites_count` from the JSON response

#### Scenario: FavoritesCount is null-safe when field is absent
- **GIVEN** a backend response that omits `favorites_count`
- **WHEN** `System.Text.Json` deserializes the response using `SportowyHubJsonContext`
- **THEN** `FavoriteActionResponse.FavoritesCount` SHALL be `null` and no deserialization exception SHALL occur

### Requirement: FavoriteItem includes serial ID
The `FavoriteItem` record SHALL include a `SerialId` field of type `int` mapping to the `serial_id` field returned by `GET /api/private/favorites`. This field SHALL be available for use in display and deep-link navigation.

#### Scenario: Favorites list response contains serial ID per item
- **GIVEN** the user is authenticated and has at least one favorite listing
- **WHEN** `FavoritesService.GetFavoritesAsync` receives a `200` response
- **THEN** each item in the deserialized `FavoritesListResponse.Items` list SHALL have `SerialId` populated with a positive integer

#### Scenario: FavoriteItem is registered in SportowyHubJsonContext
- **GIVEN** `FavoriteItem` has a `SerialId` property of type `int`
- **WHEN** the source-generated `SportowyHubJsonContext` serializes or deserializes `FavoriteItem`
- **THEN** `serial_id` is correctly mapped to `SerialId` via the `SnakeCaseLower` naming policy

### Requirement: Endpoint URI strings contain no trailing slashes on mutation endpoints
Every URI string passed to `IRequestProvider` methods (`PostAsync`, `PutAsync`, `PatchAsync`, `DeleteAsync`) throughout all service classes SHALL NOT end with a trailing slash (`/`). This applies to `ListingManagementService`, `MessagingService`, and `MediaService` which currently have trailing slashes on their create endpoints.

#### Scenario: CreateListingAsync sends POST without trailing slash
- **GIVEN** `ListingManagementService.CreateListingAsync` is called with a valid `CreateListingRequest`
- **WHEN** the URI is constructed
- **THEN** the URI SHALL be `/api/private/listings` with no trailing slash

#### Scenario: CreateConversationAsync sends POST without trailing slash
- **GIVEN** `MessagingService.CreateConversationAsync` is called with a listing ID
- **WHEN** the URI is constructed
- **THEN** the URI SHALL be `/api/private/conversations` with no trailing slash

#### Scenario: MediaService.UploadAsync sends POST without trailing slash
- **GIVEN** `MediaService.UploadAsync` is called with a listing ID and a file stream
- **WHEN** the multipart URI is constructed
- **THEN** the URI SHALL be `/api/private/media` with no trailing slash
