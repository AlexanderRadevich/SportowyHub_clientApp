## ADDED Requirements

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
