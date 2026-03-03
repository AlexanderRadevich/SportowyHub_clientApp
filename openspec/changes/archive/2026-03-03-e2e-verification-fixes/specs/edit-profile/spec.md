## ADDED Requirements

### Requirement: UpdateProfileResponse model
The app SHALL define an `UpdateProfileResponse` record with a single property: `Account` (`UserAccount?`). This record SHALL match the actual PATCH `/api/private/profile` response shape, which returns only the `account` object. The type SHALL be registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize PATCH profile response
- **WHEN** the backend returns `{"account": {"first_name": "John", "last_name": "Doe", "notifications_enabled": true, ...}}`
- **THEN** `UpdateProfileResponse.Account` SHALL contain the deserialized `UserAccount` with all fields populated

#### Scenario: UpdateProfileAsync returns UpdateProfileResponse
- **WHEN** `AuthService.UpdateProfileAsync` is called
- **THEN** the PATCH response SHALL be deserialized as `UpdateProfileResponse` instead of `UserProfile`

## MODIFIED Requirements

### Requirement: UpdateProfileAccountRequest model
The app SHALL define an `UpdateProfileAccountRequest` record with properties: `FirstName` (string?), `LastName` (string?), `NotificationsEnabled` (bool?), `QuietHoursStart` (string?), `QuietHoursEnd` (string?). The type SHALL be registered in `SportowyHubJsonContext` for source-generated serialization with `SnakeCaseLower` naming policy. When `NotificationsEnabled` is `null`, the field SHALL serialize as `null` in JSON, which the backend treats as no-op.

#### Scenario: UpdateProfileAccountRequest serializes nested fields
- **WHEN** an `UpdateProfileAccountRequest` with FirstName="John", LastName="Doe", NotificationsEnabled=true is serialized
- **THEN** the JSON output SHALL include `"first_name":"John"`, `"last_name":"Doe"`, `"notifications_enabled":true`

#### Scenario: UpdateProfileAccountRequest omits notifications when null
- **WHEN** an `UpdateProfileAccountRequest` with NotificationsEnabled=null is serialized
- **THEN** the JSON output SHALL include `"notifications_enabled":null`, and the backend SHALL treat this as no change

### Requirement: Edit Profile save command
The `EditProfileViewModel` SHALL expose a `SaveCommand` that calls `IAuthService.UpdateProfileAsync()` with the form data. The command SHALL be disabled while `IsLoading` is true. On success, a localized success toast SHALL be shown and the page SHALL navigate back. On failure, field-level errors SHALL be displayed below the corresponding fields and a general error SHALL be shown for non-field errors. The `UpdateProfileAsync` return type SHALL be `AuthResult<UpdateProfileResponse>`.

#### Scenario: Successful save navigates back with toast
- **WHEN** the user taps Save and the API returns success
- **THEN** a success toast SHALL be displayed and the page SHALL navigate back via `Shell.Current.GoToAsync("..")`

#### Scenario: Save shows loading state
- **WHEN** the user taps Save
- **THEN** `IsLoading` SHALL be true, the Save button SHALL be disabled, and an ActivityIndicator SHALL be visible until the API responds

#### Scenario: Save shows field-level API errors
- **WHEN** the API returns a 422 with field violations (e.g., `{"phone": "Invalid phone format"}`)
- **THEN** the error message SHALL be displayed below the Phone field

#### Scenario: Save shows general error on failure
- **WHEN** the API returns a non-field error (e.g., network error)
- **THEN** a localized general error message SHALL be displayed and the user SHALL remain on the edit page
