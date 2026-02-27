## MODIFIED Requirements

### Requirement: Account Profile placeholder page
The app SHALL have an `AccountProfilePage` registered as shell route `account-profile`. The page SHALL hide the tab bar (`Shell.TabBarIsVisible="False"`). The page SHALL use `AccountProfileViewModel` as its `BindingContext`, injected via constructor. The page SHALL display the user's profile data fetched from `GET /api/private/profile` in a scrollable grouped layout. The page SHALL display a "Sign Out" button at the bottom of the content. The page title SHALL be the localized `ProfileAccountProfile` resource.

#### Scenario: Account Profile page hides tab bar
- **WHEN** the Account Profile page is displayed
- **THEN** the shell tab bar SHALL NOT be visible

#### Scenario: Account Profile route is registered
- **WHEN** the app starts
- **THEN** the route `account-profile` SHALL be registered and navigable from the Profile tab

#### Scenario: Account Profile page has ViewModel
- **WHEN** the Account Profile page is constructed via DI
- **THEN** it SHALL receive `AccountProfileViewModel` as constructor parameter and set it as `BindingContext`

#### Scenario: Sign Out button is visible
- **WHEN** the Account Profile page is displayed and profile data is loaded
- **THEN** a "Sign Out" button SHALL be visible at the bottom with localized text from `AppResources.SignOut`
- **AND** the button SHALL have `AutomationId="SignOutButton"`

#### Scenario: Sign Out button has destructive styling
- **WHEN** the Account Profile page is displayed
- **THEN** the Sign Out button SHALL use red text color (`Primary` theme color) with transparent background to indicate a destructive action

### Requirement: AccountProfileViewModel sign-out integration
The `AccountProfileViewModel` SHALL inject `IAuthService` via constructor. It SHALL expose a `SignOutCommand` relay command. The command SHALL show a confirmation dialog, call `IAuthService.LogoutAsync()` on confirm, and navigate back via `Shell.Current.GoToAsync("..")`.

#### Scenario: AccountProfileViewModel receives IAuthService via DI
- **WHEN** the `AccountProfileViewModel` is constructed
- **THEN** it SHALL receive an `IAuthService` instance from the DI container

#### Scenario: SignOutCommand calls LogoutAsync on confirm
- **WHEN** the user confirms sign-out in the dialog
- **THEN** the ViewModel SHALL call `IAuthService.LogoutAsync()`

#### Scenario: SignOutCommand navigates back after logout
- **WHEN** `LogoutAsync()` completes
- **THEN** the ViewModel SHALL navigate to the Profile tab via `Shell.Current.GoToAsync("..")`

## ADDED Requirements

### Requirement: UserProfile API model
The app SHALL define a `UserProfile` record matching the `GET /api/private/profile` flat response. The record SHALL include all fields from the response: `Id`, `Email`, `FirstName`, `LastName`, `Locale`, `AvatarUrl`, `NotificationsEnabled`, `QuietHoursStart`, `QuietHoursEnd`, `Phone`, `PhoneVerified`, `EmailVerified`, `TrustLevel`. All nullable JSON fields SHALL map to nullable C# types. The type SHALL be registered in `SportowyHubJsonContext` for source-generated serialization with `SnakeCaseLower` naming policy.

#### Scenario: UserProfile deserializes full API response
- **WHEN** the API returns a complete `/api/private/profile` JSON payload
- **THEN** the `UserProfile` record SHALL contain all fields: `Id`, `Email`, `FirstName`, `LastName`, `Locale`, `AvatarUrl`, `NotificationsEnabled`, `QuietHoursStart`, `QuietHoursEnd`, `Phone`, `PhoneVerified`, `EmailVerified`, `TrustLevel`

#### Scenario: Nullable fields handled correctly
- **WHEN** the API returns `null` for optional fields (e.g., `first_name`, `avatar_url`, `phone`)
- **THEN** the corresponding C# properties SHALL be `null`, not default values

### Requirement: GetProfileAsync on auth service
`IAuthService` SHALL expose a `Task<UserProfile?> GetProfileAsync()` method. `AuthService` SHALL implement it by reading the access token from SecureStorage and calling `GET /api/private/profile` with Bearer authorization. The method SHALL return `null` on any error (auth failure, network error, deserialization error) without throwing exceptions.

#### Scenario: GetProfileAsync returns profile on success
- **WHEN** `GetProfileAsync()` is called and the user has a valid token
- **THEN** the method SHALL return a `UserProfile` object with data from the API

#### Scenario: GetProfileAsync returns null on auth failure
- **WHEN** `GetProfileAsync()` is called and the token is expired or invalid (401/403)
- **THEN** the method SHALL return `null`

#### Scenario: GetProfileAsync returns null on network error
- **WHEN** `GetProfileAsync()` is called and the network is unavailable
- **THEN** the method SHALL return `null`

#### Scenario: GetProfileAsync returns null when no token exists
- **WHEN** `GetProfileAsync()` is called and no token is stored
- **THEN** the method SHALL return `null`

### Requirement: Profile loading on page appearance
The `AccountProfileViewModel` SHALL expose a `LoadProfileCommand` that fetches the user profile via `IAuthService.GetProfileAsync()`. The page code-behind SHALL execute `LoadProfileCommand` in the `OnAppearing` lifecycle event. The ViewModel SHALL expose `IsLoading` (bool) and `HasError` (bool) observable properties to drive UI states.

#### Scenario: Loading state while fetching profile
- **WHEN** the Account Profile page appears and profile data is being fetched
- **THEN** `IsLoading` SHALL be `true` and an `ActivityIndicator` SHALL be visible

#### Scenario: Successful profile load
- **WHEN** `GetProfileAsync()` returns a `UserProfile` object
- **THEN** `IsLoading` SHALL be `false`, `HasError` SHALL be `false`, and the profile data SHALL be displayed

#### Scenario: Failed profile load shows error
- **WHEN** `GetProfileAsync()` returns `null`
- **THEN** `IsLoading` SHALL be `false`, `HasError` SHALL be `true`, and a localized error message SHALL be displayed

#### Scenario: Profile reloads on each page appearance
- **WHEN** the user navigates to the Account Profile page (including returning from another page)
- **THEN** `LoadProfileCommand` SHALL execute and fetch fresh data from the API

### Requirement: Profile header display
The Account Profile page SHALL display a header area at the top containing: an avatar circle placeholder (person icon), the user's display name (computed from `FirstName`/`LastName`, falling back to email), and a trust level indicator.

#### Scenario: Header shows full name when available
- **WHEN** the profile has `FirstName` and/or `LastName` set
- **THEN** the header SHALL display the combined name as the primary text and the email below it

#### Scenario: Header falls back to email when name is null
- **WHEN** the profile has both `FirstName` and `LastName` as null
- **THEN** the header SHALL display the email as the primary text

#### Scenario: Header shows trust level
- **WHEN** the profile is displayed
- **THEN** the trust level (e.g., "TL1") SHALL be visible in the header area

#### Scenario: Header shows avatar placeholder
- **WHEN** the profile is displayed (regardless of `AvatarUrl` value)
- **THEN** a circle placeholder with a person icon SHALL be displayed (avatar image loading is out of scope)

### Requirement: Contact section display
The Account Profile page SHALL display a "Contact" section with the user's email and phone number. Each row SHALL show a verification status indicator: localized "Verified" text in green (`Success` color) or localized "Not verified" text in `TextSecondary` color.

#### Scenario: Email row shows address and verified status
- **WHEN** the profile has `Email` = "alex@example.com" and `EmailVerified` = true
- **THEN** the Contact section SHALL display "alex@example.com" with a "Verified" indicator

#### Scenario: Email row shows not verified status
- **WHEN** the profile has `EmailVerified` = false
- **THEN** the Contact section SHALL display the email with a "Not verified" indicator

#### Scenario: Phone row shows number and verified status
- **WHEN** the profile has `Phone` = "123456789" and `PhoneVerified` = true
- **THEN** the Contact section SHALL display "123456789" with a "Verified" indicator

#### Scenario: Phone row shows not verified status
- **WHEN** the profile has `PhoneVerified` = false
- **THEN** the Contact section SHALL display the phone with a "Not verified" indicator

### Requirement: Account section display
The Account Profile page SHALL display an "Account" section with the trust level row.

#### Scenario: Trust level displayed
- **WHEN** the profile has `TrustLevel` = "TL1"
- **THEN** the Account section SHALL display a "Trust Level" row with value "TL1"

### Requirement: Null field display
When a profile field value is null, the corresponding row SHALL display a localized "Not set" placeholder text in `TextSecondary` color. All profile rows SHALL always be visible regardless of whether the value is null.

#### Scenario: Null field shows placeholder
- **WHEN** a profile field (e.g., `Phone`) is null
- **THEN** the row SHALL display localized "Not set" text in `TextSecondary` color

#### Scenario: Non-null field shows value
- **WHEN** a profile field has a value
- **THEN** the row SHALL display the actual value in default text color

### Requirement: Profile localization strings
The app SHALL define localized strings for all profile labels and status texts across all 4 languages (pl, en, uk, ru). Required keys: `ProfileContact`, `ProfileEmail`, `ProfilePhone`, `ProfileAccount`, `ProfileVerified`, `ProfileNotVerified`, `ProfileNotSet`, `ProfileTrustLevel`, `ProfileLoadError`.

#### Scenario: All profile labels are localized
- **WHEN** the Account Profile page is displayed in any supported language
- **THEN** all section headers, field labels, and status texts SHALL use localized strings from `AppResources`
