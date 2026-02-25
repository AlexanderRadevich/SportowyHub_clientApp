## MODIFIED Requirements

### Requirement: LoginResponse model
The app SHALL contain a `LoginResponse` record with `Token` (string), `User` (`UserInfo`), and `Locale` (string, nullable) properties. `UserInfo` SHALL have `Id` (int), `Email` (string), and `TrustLevel` (string) properties. It SHALL deserialize from the API response: `{"locale": "pl", "token": "...", "user": {"id": 1, "email": "...", "trust_level": "TL0"}}`.

#### Scenario: LoginResponse deserializes from API format
- **WHEN** the API returns `{"locale":"pl","token":"jwt.token.here","user":{"id":1,"email":"user@example.com","trust_level":"TL0"}}`
- **THEN** it SHALL deserialize to a `LoginResponse` with Token="jwt.token.here", User.TrustLevel="TL0", and Locale="pl"

#### Scenario: LoginResponse deserializes without locale
- **WHEN** the API returns a response without the `locale` field
- **THEN** `Locale` SHALL be null, and `Token` and `User` SHALL deserialize normally

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
