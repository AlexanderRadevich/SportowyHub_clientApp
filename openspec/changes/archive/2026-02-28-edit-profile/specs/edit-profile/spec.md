## ADDED Requirements

### Requirement: Edit Profile page registration
The app SHALL have an `EditProfilePage` registered as Shell route `edit-profile`. The page SHALL hide the tab bar. The page SHALL use `EditProfileViewModel` as its `BindingContext`, injected via constructor. Both the page and ViewModel SHALL be registered in the DI container as transient services. The page title SHALL be the localized `EditProfileTitle` resource.

#### Scenario: Edit Profile route is navigable
- **WHEN** the user taps the Edit button on the Account Profile page
- **THEN** the app SHALL navigate to the `edit-profile` route and display the Edit Profile page

#### Scenario: Edit Profile page hides tab bar
- **WHEN** the Edit Profile page is displayed
- **THEN** the shell tab bar SHALL NOT be visible

### Requirement: Edit Profile form layout
The Edit Profile page SHALL display a scrollable form with editable fields: First Name (Entry), Last Name (Entry), Phone (Entry with Telephone keyboard), Notifications Enabled (Switch), Quiet Hours Start (Entry with HH:mm placeholder), Quiet Hours End (Entry with HH:mm placeholder). Each field SHALL have a localized label. The form SHALL follow the same styling pattern as the Login page (Border with RoundRectangle, Entry inside).

#### Scenario: Form displays all editable fields
- **WHEN** the Edit Profile page is displayed
- **THEN** the form SHALL show labeled inputs for first name, last name, phone, notifications toggle, quiet hours start, and quiet hours end

#### Scenario: Form fields use correct keyboard types
- **WHEN** the form is displayed
- **THEN** the Phone field SHALL use `Keyboard="Telephone"` and text fields SHALL use default keyboard

### Requirement: Edit Profile form pre-population
The `EditProfileViewModel` SHALL implement `IQueryAttributable` to receive the current profile data. On initialization, all form fields SHALL be pre-populated with the current profile values: `Account.FirstName`, `Account.LastName`, `Phone`, `Account.NotificationsEnabled`, `Account.QuietHoursStart`, `Account.QuietHoursEnd`.

#### Scenario: Form pre-populated with existing profile data
- **WHEN** the Edit Profile page opens and the profile has FirstName="John", LastName="Doe", Phone="123456789"
- **THEN** the form fields SHALL display "John", "Doe", and "123456789" respectively

#### Scenario: Form handles null profile fields
- **WHEN** the Edit Profile page opens and profile fields are null
- **THEN** the corresponding form fields SHALL be empty

### Requirement: Edit Profile quiet hours validation
The ViewModel SHALL validate quiet hours fields against HH:mm format (00:00–23:59). If the format is invalid, a localized error message SHALL be displayed below the field.

#### Scenario: Valid quiet hours accepted
- **WHEN** the user enters "23:00" in the Quiet Hours Start field
- **THEN** no validation error SHALL be displayed

#### Scenario: Invalid quiet hours shows error
- **WHEN** the user enters "25:00" or "abc" in a quiet hours field
- **THEN** a localized validation error SHALL be displayed below the field

#### Scenario: Empty quiet hours accepted
- **WHEN** the user leaves a quiet hours field empty
- **THEN** no validation error SHALL be displayed (quiet hours are optional)

### Requirement: Edit Profile save command
The `EditProfileViewModel` SHALL expose a `SaveCommand` that calls `IAuthService.UpdateProfileAsync()` with the form data. The command SHALL be disabled while `IsLoading` is true. On success, a localized success toast SHALL be shown and the page SHALL navigate back. On failure, field-level errors SHALL be displayed below the corresponding fields and a general error SHALL be shown for non-field errors.

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

### Requirement: Edit Profile cancel
The Edit Profile page SHALL have a cancel mechanism. Tapping the back button or a Cancel button SHALL navigate back without saving. No confirmation dialog is needed since there is no destructive action.

#### Scenario: Cancel navigates back without saving
- **WHEN** the user taps Cancel or the back button
- **THEN** the page SHALL navigate back without calling the API

### Requirement: Edit Profile localization strings
The app SHALL define localized strings across all 4 languages (pl, en, uk, ru) for: `EditProfileTitle`, `EditProfileFirstName`, `EditProfileLastName`, `EditProfilePhone`, `EditProfileNotifications`, `EditProfileQuietHoursStart`, `EditProfileQuietHoursEnd`, `EditProfileSave`, `EditProfileSuccess`, `EditProfileError`, `EditProfileInvalidTime`.

#### Scenario: All edit profile labels are localized
- **WHEN** the Edit Profile page is displayed in any supported language
- **THEN** all labels, buttons, and messages SHALL use localized strings from `AppResources`
