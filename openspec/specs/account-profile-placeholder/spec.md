# Account Profile Placeholder

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

### Requirement: Sign Out confirmation dialog
When the user taps the Sign Out button, the app SHALL display a confirmation dialog before proceeding. The dialog SHALL use localized strings for title (`SignOutConfirmTitle`), message (`SignOutConfirmMessage`), accept button (`SignOut`), and cancel button (`Cancel`).

#### Scenario: Tapping Sign Out shows confirmation
- **WHEN** the user taps the "Sign Out" button
- **THEN** a confirmation dialog SHALL appear with localized title, message, accept, and cancel buttons

#### Scenario: User cancels sign out
- **WHEN** the user taps "Cancel" on the sign-out confirmation dialog
- **THEN** the dialog SHALL dismiss and the user SHALL remain on the Account Profile page with no state changes

#### Scenario: User confirms sign out
- **WHEN** the user taps the accept button on the sign-out confirmation dialog
- **THEN** the app SHALL call `IAuthService.LogoutAsync()` and navigate back to the Profile tab

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

### Requirement: UserProfile API model
The app SHALL define a `UserProfile` record matching the `GET /api/private/profile` nested response. The record SHALL include top-level fields: `Id` (int), `Email` (string), `EmailVerified` (bool), `EmailVerifiedAt` (string?), `Phone` (string?), `PhoneVerified` (bool), `PhoneVerifiedAt` (string?), `TrustLevel` (string), `ReputationScore` (int), `OauthLinked` (OauthLinked?), `LastLoginAt` (string?), `LastActivityAt` (string?), `Locale` (string?). The record SHALL include a nested `Account` property of type `UserAccount?`. All nullable JSON fields SHALL map to nullable C# types. The type SHALL be registered in `SportowyHubJsonContext` for source-generated serialization with `SnakeCaseLower` naming policy.

#### Scenario: UserProfile deserializes full nested API response
- **WHEN** the API returns a complete `/api/private/profile` JSON payload with nested `account` and `oauth_linked` objects
- **THEN** the `UserProfile` record SHALL contain all top-level fields and nested `Account` and `OauthLinked` objects with correct values

#### Scenario: UserProfile new top-level fields deserialized
- **WHEN** the API returns `"reputation_score": 5, "email_verified_at": "2026-01-15T10:00:00+01:00", "last_login_at": null`
- **THEN** `ReputationScore` SHALL be 5, `EmailVerifiedAt` SHALL be "2026-01-15T10:00:00+01:00", `LastLoginAt` SHALL be null

#### Scenario: Nullable nested objects handled correctly
- **WHEN** the API returns `"account": null` or `"oauth_linked": null`
- **THEN** `Account` SHALL be null and `OauthLinked` SHALL be null respectively

### Requirement: GetProfileAsync on auth service
`IAuthService` SHALL expose a `Task<UserProfile?> GetProfileAsync()` method. `AuthService` SHALL implement it by reading the access token from SecureStorage and calling `GET /api/private/profile` with Bearer authorization. The method SHALL return `null` when no token exists and let exceptions propagate to the caller for network/auth errors.

#### Scenario: GetProfileAsync returns profile on success
- **WHEN** `GetProfileAsync()` is called and the user has a valid token
- **THEN** the method SHALL return a `UserProfile` object with data from the API

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
The Account Profile page SHALL display a header area at the top containing: an avatar circle placeholder (person icon), the user's display name, and a trust level indicator with reputation score.

#### Scenario: Header shows full name from Account.FullName when available
- **WHEN** the profile has `Account.FullName` set (e.g., "John Doe")
- **THEN** the header SHALL display `Account.FullName` as the primary text and the email below it

#### Scenario: Header falls back to FirstName/LastName when FullName is null
- **WHEN** the profile has `Account.FullName` as null but `Account.FirstName` and/or `Account.LastName` set
- **THEN** the header SHALL display the combined FirstName+LastName as the primary text and the email below it

#### Scenario: Header falls back to email when all names are null
- **WHEN** the profile has `Account.FullName`, `Account.FirstName`, and `Account.LastName` all null (or Account is null)
- **THEN** the header SHALL display the email as the primary text

#### Scenario: Header shows trust level and reputation score
- **WHEN** the profile is displayed with TrustLevel="TL1" and ReputationScore=5
- **THEN** the trust level and reputation score SHALL both be visible in the header area, formatted as "TL1 · 5 rep"

#### Scenario: Header shows avatar placeholder
- **WHEN** the profile is displayed (regardless of `Account.AvatarUrl` value)
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
The Account Profile page SHALL display an "Account" section with rows for trust level, reputation score, balance, and Google account link status.

#### Scenario: Trust level displayed
- **WHEN** the profile has `TrustLevel` = "TL1"
- **THEN** the Account section SHALL display a "Trust Level" row with value "TL1"

#### Scenario: Reputation score displayed
- **WHEN** the profile has `ReputationScore` = 5
- **THEN** the Account section SHALL display a localized "Reputation Score" row with value "5"

#### Scenario: Balance displayed as currency
- **WHEN** the profile has `Account.BalanceGrosze` = 1500
- **THEN** the Account section SHALL display a localized "Balance" row with value "15.00 zł"

#### Scenario: Zero balance displayed
- **WHEN** the profile has `Account.BalanceGrosze` = 0
- **THEN** the Account section SHALL display a localized "Balance" row with value "0.00 zł"

#### Scenario: Balance row hidden when Account is null
- **WHEN** the profile has `Account` = null
- **THEN** the Balance row SHALL display "0.00 zł" (default)

#### Scenario: Google linked status displayed as linked
- **WHEN** the profile has `OauthLinked.Google` = true
- **THEN** the Account section SHALL display a "Google Account" row with localized "Linked" text in `Success` color

#### Scenario: Google linked status displayed as not linked
- **WHEN** the profile has `OauthLinked.Google` = false (or `OauthLinked` is null)
- **THEN** the Account section SHALL display a "Google Account" row with localized "Not linked" text in `TextSecondary` color

### Requirement: AccountProfileViewModel nested property access
The `AccountProfileViewModel` SHALL access profile properties through the nested structure. `DisplayName` SHALL use `Profile.Account?.FullName` first, then fall back to combined `Profile.Account?.FirstName`/`Profile.Account?.LastName`, then `Profile.Email`. The ViewModel SHALL expose computed properties for formatted balance and reputation display.

#### Scenario: DisplayName uses FullName when available
- **WHEN** `Profile.Account.FullName` = "John Doe"
- **THEN** `DisplayName` SHALL return "John Doe"

#### Scenario: DisplayName falls back to FirstName+LastName
- **WHEN** `Profile.Account.FullName` is null and `FirstName` = "John", `LastName` = "Doe"
- **THEN** `DisplayName` SHALL return "John Doe"

#### Scenario: DisplayName falls back to Email
- **WHEN** `Profile.Account` is null or all name fields are null
- **THEN** `DisplayName` SHALL return `Profile.Email`

#### Scenario: FormattedBalance computes currency string
- **WHEN** `Profile.Account.BalanceGrosze` = 1500
- **THEN** `FormattedBalance` SHALL return "15.00 zł"

#### Scenario: TrustInfo combines trust level and reputation
- **WHEN** `Profile.TrustLevel` = "TL1" and `Profile.ReputationScore` = 5
- **THEN** the header subtitle SHALL display "TL1 · 5 rep"

### Requirement: Null field display
When a profile field value is null, the corresponding row SHALL display a localized "Not set" placeholder text in `TextSecondary` color. All profile rows SHALL always be visible regardless of whether the value is null.

#### Scenario: Null field shows placeholder
- **WHEN** a profile field (e.g., `Phone`) is null
- **THEN** the row SHALL display localized "Not set" text in `TextSecondary` color

#### Scenario: Non-null field shows value
- **WHEN** a profile field has a value
- **THEN** the row SHALL display the actual value in default text color

### Requirement: Profile localization strings
The app SHALL define localized strings for all profile labels and status texts across all 4 languages (pl, en, uk, ru). Required keys: `ProfileContact`, `ProfileEmail`, `ProfilePhone`, `ProfileAccount`, `ProfileVerified`, `ProfileNotVerified`, `ProfileNotSet`, `ProfileTrustLevel`, `ProfileLoadError`, `ProfileReputationScore`, `ProfileBalance`, `ProfileGoogleLinked`, `ProfileLinked`, `ProfileNotLinked`.

#### Scenario: All profile labels are localized
- **WHEN** the Account Profile page is displayed in any supported language
- **THEN** all section headers, field labels, and status texts SHALL use localized strings from `AppResources`

#### Scenario: New localization keys exist in all languages
- **WHEN** the app is built
- **THEN** `ProfileReputationScore`, `ProfileBalance`, `ProfileGoogleLinked`, `ProfileLinked`, `ProfileNotLinked` SHALL exist in all 4 `.resx` files (en, pl, uk, ru)

### Requirement: Edit button on Account Profile page
The Account Profile page SHALL display an "Edit" button that navigates to the `edit-profile` route. The button SHALL be visible when profile data is loaded (not in loading or error state). The button SHALL pass the current profile data to the Edit Profile page.

#### Scenario: Edit button visible when profile is loaded
- **WHEN** the Account Profile page displays profile data
- **THEN** an "Edit" button SHALL be visible in the header or toolbar area

#### Scenario: Edit button navigates to edit page
- **WHEN** the user taps the Edit button
- **THEN** the app SHALL navigate to the `edit-profile` route with the current profile data

#### Scenario: Profile refreshes after returning from edit
- **WHEN** the user saves changes on the Edit Profile page and navigates back
- **THEN** the Account Profile page SHALL re-fetch the profile via `LoadProfileCommand` in `OnAppearing`
