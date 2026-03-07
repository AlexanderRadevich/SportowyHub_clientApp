## MODIFIED Requirements

### Requirement: Edit Profile form layout
The Edit Profile page SHALL display a scrollable form with editable fields: First Name (Entry), Last Name (Entry), Phone (Entry with Telephone keyboard), Voivodeship (Picker), City (Picker), Notifications Enabled (Switch), Quiet Hours Start (Entry with HH:mm placeholder), Quiet Hours End (Entry with HH:mm placeholder). Each field SHALL have a localized label. The form SHALL follow the same styling pattern as the Login page (Border with RoundRectangle, Entry inside). The Voivodeship and City pickers SHALL use Border with RoundRectangle styling consistent with other form fields.

#### Scenario: Form displays all editable fields
- **WHEN** the Edit Profile page is displayed
- **THEN** the form SHALL show labeled inputs for first name, last name, phone, voivodeship, city, notifications toggle, quiet hours start, and quiet hours end

#### Scenario: Form fields use correct keyboard types
- **WHEN** the form is displayed
- **THEN** the Phone field SHALL use `Keyboard="Telephone"` and text fields SHALL use default keyboard

### Requirement: Edit Profile form pre-population
The `EditProfileViewModel` SHALL implement `IQueryAttributable` to receive the current profile data. On initialization, all form fields SHALL be pre-populated with the current profile values: `Account.FirstName`, `Account.LastName`, `Phone`, `Account.VoivodeshipId`, `Account.CityId`, `Account.NotificationsEnabled`, `Account.QuietHoursStart`, `Account.QuietHoursEnd`.

#### Scenario: Form pre-populated with existing profile data
- **WHEN** the Edit Profile page opens and the profile has FirstName="John", LastName="Doe", Phone="123456789"
- **THEN** the form fields SHALL display "John", "Doe", and "123456789" respectively

#### Scenario: Form pre-populated with geography data
- **WHEN** the Edit Profile page opens and the profile has VoivodeshipId=7 and CityId=42
- **THEN** the voivodeship picker SHALL select the matching voivodeship and the city picker SHALL select the matching city

#### Scenario: Form handles null profile fields
- **WHEN** the Edit Profile page opens and profile fields are null
- **THEN** the corresponding form fields SHALL be empty

### Requirement: Edit Profile geography pickers
The `EditProfileViewModel` SHALL load voivodeships from `GET /api/v1/geography/voivodeships` on page initialization. When the user selects a voivodeship, it SHALL load cities from `GET /api/v1/geography/cities?voivodeship_id={id}`. When the voivodeship selection changes, the city picker SHALL reset to empty and reload cities for the new voivodeship.

#### Scenario: Voivodeships loaded on page init
- **WHEN** the Edit Profile page is displayed
- **THEN** the voivodeship picker SHALL be populated with voivodeships from the API

#### Scenario: Cities loaded after voivodeship selection
- **WHEN** the user selects a voivodeship with id=7
- **THEN** the city picker SHALL be populated with cities from `GET /api/v1/geography/cities?voivodeship_id=7`

#### Scenario: City picker resets on voivodeship change
- **WHEN** the user changes the voivodeship selection from id=7 to id=3
- **THEN** the city picker SHALL clear its selection and reload cities for voivodeship id=3

### Requirement: Edit Profile save includes geography fields
The `SaveCommand` SHALL include `VoivodeshipId` and `CityId` in the `UpdateProfileAccountRequest` when calling `IAuthService.UpdateProfileAsync()`.

#### Scenario: Save sends geography fields
- **WHEN** the user selects voivodeship id=7 and city id=42, then taps Save
- **THEN** the request SHALL include `voivodeship_id: 7` and `city_id: 42` in the account payload

#### Scenario: Save sends null geography when not selected
- **WHEN** the user leaves voivodeship and city pickers empty, then taps Save
- **THEN** the request SHALL include `voivodeship_id: null` and `city_id: null` in the account payload

### Requirement: Edit Profile localization strings
The app SHALL define localized strings across all 4 languages (pl, en, uk, ru) for: `EditProfileTitle`, `EditProfileFirstName`, `EditProfileLastName`, `EditProfilePhone`, `EditProfileVoivodeship`, `EditProfileCity`, `EditProfileNotifications`, `EditProfileQuietHoursStart`, `EditProfileQuietHoursEnd`, `EditProfileSave`, `EditProfileSuccess`, `EditProfileError`, `EditProfileInvalidTime`.

#### Scenario: All edit profile labels are localized
- **WHEN** the Edit Profile page is displayed in any supported language
- **THEN** all labels, buttons, and messages SHALL use localized strings from `AppResources`

## ADDED Requirements

### Requirement: UserAccount geography fields
The `UserAccount` record SHALL include `VoivodeshipId` (int?) and `CityId` (int?) properties matching the backend `profile.account.voivodeship_id` and `profile.account.city_id` fields.

#### Scenario: Deserialize profile with geography
- **WHEN** the API returns a profile with `"voivodeship_id": 7, "city_id": 42` in the account object
- **THEN** `UserAccount.VoivodeshipId` SHALL be 7 and `UserAccount.CityId` SHALL be 42

#### Scenario: Deserialize profile without geography
- **WHEN** the API returns a profile without geography fields (or with null values)
- **THEN** `UserAccount.VoivodeshipId` and `UserAccount.CityId` SHALL be null

### Requirement: UpdateProfileAccountRequest geography fields
The `UpdateProfileAccountRequest` record SHALL include `VoivodeshipId` (int?) and `CityId` (int?) properties for the PATCH profile endpoint.

#### Scenario: Serialize profile update with geography
- **WHEN** the app sends a profile update with VoivodeshipId=7 and CityId=42
- **THEN** the JSON payload SHALL include `"voivodeship_id": 7` and `"city_id": 42`

#### Scenario: Serialize profile update without geography
- **WHEN** the app sends a profile update with VoivodeshipId=null and CityId=null
- **THEN** the JSON payload SHALL include `"voivodeship_id": null` and `"city_id": null`
