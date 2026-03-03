## ADDED Requirements

### Requirement: IPhoneVerificationService interface
The app SHALL define an `IPhoneVerificationService` interface registered as singleton in DI with methods:
- `RequestVerificationAsync(string phone, CancellationToken ct)` returning `Task<PhoneVerificationRequestResponse>`
- `VerifyCodeAsync(string code, CancellationToken ct)` returning `Task<PhoneVerificationResult>`

#### Scenario: Service is injectable
- **WHEN** `IPhoneVerificationService` is resolved from DI
- **THEN** a singleton `PhoneVerificationService` instance SHALL be returned

### Requirement: Request phone verification
`RequestVerificationAsync` SHALL POST `{"phone":"..."}` to `/api/private/phone-verification/request` with Bearer auth.

#### Scenario: Request code successfully
- **WHEN** `RequestVerificationAsync("+48123456789")` is called
- **THEN** it SHALL POST to `/api/private/phone-verification/request` and return the response with `Message` and `ExpiresAt`

#### Scenario: Invalid phone number
- **WHEN** the backend returns 422 for an invalid phone format
- **THEN** the exception SHALL propagate to the caller

### Requirement: Verify phone code
`VerifyCodeAsync` SHALL POST `{"code":"..."}` to `/api/private/phone-verification/verify` with Bearer auth.

#### Scenario: Verify with correct code
- **WHEN** `VerifyCodeAsync("123456")` is called with the correct code
- **THEN** it SHALL return `PhoneVerificationResult` with `Message` and updated `TrustLevel`

#### Scenario: Verify with invalid code
- **WHEN** the backend returns 400 for an invalid or expired code
- **THEN** the exception SHALL propagate to the caller

### Requirement: PhoneVerificationRequestRequest model
The app SHALL define a `PhoneVerificationRequestRequest` record with `Phone` (string). Registered in `SportowyHubJsonContext`.

#### Scenario: Serialize request
- **WHEN** serialized with Phone="+48123456789"
- **THEN** the JSON SHALL be `{"phone":"+48123456789"}`

### Requirement: PhoneVerificationRequestResponse model
The app SHALL define a `PhoneVerificationRequestResponse` record with `Message` (string) and `ExpiresAt` (string). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize response
- **WHEN** the API returns `{"message":"Verification code sent","expires_at":"2026-03-03T11:00:00+00:00"}`
- **THEN** it SHALL deserialize with both fields populated

### Requirement: PhoneVerificationVerifyRequest model
The app SHALL define a `PhoneVerificationVerifyRequest` record with `Code` (string). Registered in `SportowyHubJsonContext`.

#### Scenario: Serialize verify request
- **WHEN** serialized with Code="123456"
- **THEN** the JSON SHALL be `{"code":"123456"}`

### Requirement: PhoneVerificationResult model
The app SHALL define a `PhoneVerificationResult` record with `Message` (string) and `TrustLevel` (string). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize verify response
- **WHEN** the API returns `{"message":"Phone verified successfully","trust_level":"TL1"}`
- **THEN** it SHALL deserialize with Message and TrustLevel populated
