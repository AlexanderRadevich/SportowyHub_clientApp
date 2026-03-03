# Profile Hub

## Purpose

Defines the Profile tab layout, sections, auth-state-dependent rows, settings pickers, and the `ProfileViewModel`.

## Requirements

### Requirement: Profile hub grouped-list layout
The Profile tab SHALL display a scrollable grouped-list layout containing a branding header and multiple sections. The layout SHALL use a `ScrollView` wrapping a `VerticalStackLayout`. The page title SHALL be sourced from the localized resource `TabProfile`.

#### Scenario: Profile hub renders with branding and sections
- **WHEN** the user navigates to the Profile tab
- **THEN** the page SHALL display the SportowyHub logo (`logo_full.png`) at the top, followed by an "Account" section and a "Settings" section, all within a scrollable container

#### Scenario: Profile hub is scrollable
- **WHEN** the content exceeds the visible area
- **THEN** the user SHALL be able to scroll vertically to see all sections

### Requirement: Section header styling
Each section SHALL have a header label displayed on a `Surface`-colored background strip. The header label SHALL use the `SectionHeader` style (semibold, 16px). The header strip SHALL have horizontal padding of 16px and vertical padding of 8px.

#### Scenario: Section header appearance in light theme
- **WHEN** the profile hub is displayed in light theme
- **THEN** section headers SHALL display bold text on a `Surface` (#F7F9FC) background strip

#### Scenario: Section header appearance in dark theme
- **WHEN** the profile hub is displayed in dark theme
- **THEN** section headers SHALL display bold text on a `SurfaceDark` (#1E1E1E) background strip

### Requirement: Account section with auth rows
The Account section SHALL display a section header with the localized text `ProfileAccountSection`. Below the header, the section SHALL conditionally display rows based on auth state. When the user is NOT logged in, the section SHALL show two tappable rows: "Sign In" (`ProfileSignIn`) and "Create Account" (`ProfileCreateAccount`) with chevron indicators. When the user IS logged in, the section SHALL show a tappable "Account Profile" row (`ProfileAccountProfile`) with a chevron indicator that navigates to the Account Profile page, followed by a "Sign Out" row (`SignOut`) with red/Primary text color. Tapping "Sign Out" SHALL show a confirmation dialog and, on confirm, call `IAuthService.LogoutAsync()` and refresh auth state. Auth state SHALL be refreshed every time the Profile page appears.

#### Scenario: Account section displays Sign In and Create Account rows when logged out
- **WHEN** the profile hub is displayed and the user is not logged in
- **THEN** the Account section SHALL show "Sign In" and "Create Account" as tappable rows with chevron indicators

#### Scenario: Account section displays Account Profile and Sign Out rows when logged in
- **WHEN** the profile hub is displayed and the user is logged in
- **THEN** the Account section SHALL show an "Account Profile" tappable row with a chevron indicator, followed by a "Sign Out" row
- **AND** the "Sign In" and "Create Account" rows SHALL NOT be visible

#### Scenario: Sign Out row has destructive styling
- **WHEN** the profile hub is displayed and the user is logged in
- **THEN** the "Sign Out" row label SHALL use the `Primary` theme color (red) to indicate a destructive action

#### Scenario: Sign Out row is locatable by AutomationId
- **WHEN** the profile hub is displayed and the user is logged in
- **THEN** the Sign Out row Grid SHALL have `AutomationId="ProfileSignOutRow"`

#### Scenario: Tapping Sign Out shows confirmation dialog
- **WHEN** the user taps the "Sign Out" row on the Profile page
- **THEN** a confirmation dialog SHALL appear with localized title (`SignOutConfirmTitle`), message (`SignOutConfirmMessage`), accept (`SignOut`), and cancel (`Cancel`) buttons

#### Scenario: Confirming Sign Out logs out and refreshes state
- **WHEN** the user confirms sign-out in the dialog
- **THEN** the app SHALL call `IAuthService.LogoutAsync()` and refresh auth state so the UI switches to the logged-out view

#### Scenario: Cancelling Sign Out keeps state unchanged
- **WHEN** the user cancels sign-out in the dialog
- **THEN** the user SHALL remain on the Profile page in the logged-in state

#### Scenario: Tapping Sign In row navigates to Login page
- **WHEN** the user taps the "Sign In" row
- **THEN** the app SHALL navigate to the Login page via Shell navigation

#### Scenario: Tapping Create Account row navigates to Register page
- **WHEN** the user taps the "Create Account" row
- **THEN** the app SHALL navigate to the Registration page via Shell navigation

#### Scenario: Tapping Account Profile row navigates to Account Profile page
- **WHEN** the user taps the "Account Profile" row
- **THEN** the app SHALL navigate to the Account Profile page via Shell route `account-profile`

#### Scenario: Auth state refreshes on page appearance
- **WHEN** the user navigates to the Profile tab after logging in from another screen
- **THEN** the Account section SHALL reflect the updated auth state without requiring app restart

### Requirement: Settings section with picker rows
The Settings section SHALL display a section header with the localized text `ProfileSettingsSection`. Below the header, the section SHALL contain two rows: "Language" (`SettingsLanguage`) and "Theme" (`SettingsTheme`). Each row SHALL display the localized setting label on the left and the current selection value on the right. The settings section header label SHALL have `AutomationId="SettingsLabel"`. The language setting label SHALL have `AutomationId="LanguageLabel"`. The theme setting label SHALL have `AutomationId="ThemeLabel"`. The language Picker SHALL have `AutomationId="LanguagePicker"`. The theme Picker SHALL have `AutomationId="ThemePicker"`.

#### Scenario: Settings section displays Language and Theme rows
- **WHEN** the profile hub is displayed
- **THEN** the Settings section SHALL show "Language" and "Theme" rows with their current values displayed

#### Scenario: Settings elements are identifiable by AutomationId
- **WHEN** the profile hub is inspected via Appium or accessibility tools
- **THEN** the settings section label SHALL be locatable by `AutomationId` `SettingsLabel`, the language picker by `LanguagePicker`, the theme picker by `ThemePicker`, the language label by `LanguageLabel`, and the theme label by `ThemeLabel`

### Requirement: Row visual styling
Each tappable row SHALL have a minimum height of 48px, horizontal padding of 16px, and a bottom border separator using the theme's `Border` color. Row labels SHALL use the default Label style (14px). The current value text on the right side of settings rows SHALL use `TextSecondary` color.

#### Scenario: Row styling in light theme
- **WHEN** a row is displayed in light theme
- **THEN** it SHALL have a `Border` (#E5E7EB) bottom separator, `TextPrimary` (#1A1A1A) label, and `TextSecondary` (#6B7280) value text

#### Scenario: Row styling in dark theme
- **WHEN** a row is displayed in dark theme
- **THEN** it SHALL have a `BorderDark` (#2A2A2A) bottom separator, `TextPrimaryDark` (#FFFFFF) label, and `TextSecondaryDark` (#B0B3B8) value text

### Requirement: ProfileViewModel with CommunityToolkit.Mvvm
The profile hub SHALL be backed by a `ProfileViewModel` using `CommunityToolkit.Mvvm`. The ViewModel SHALL receive `IAuthService` via constructor injection. The ViewModel SHALL expose `RelayCommand`s for Sign In, Create Account, Account Profile navigation, and Sign Out. The Sign Out command SHALL show a confirmation dialog, call `IAuthService.LogoutAsync()` on confirm, and refresh auth state via `RefreshAuthStateCommand`. The ViewModel SHALL expose properties for the selected language and theme bound to Picker controls. The ViewModel SHALL expose an `IsLoggedIn` observable boolean property. The ViewModel SHALL expose a `RefreshAuthStateCommand` that calls `IAuthService.IsLoggedInAsync()` and updates `IsLoggedIn`.

#### Scenario: ViewModel provides navigation commands
- **WHEN** the ProfileViewModel is instantiated
- **THEN** it SHALL expose `SignInCommand`, `CreateAccountCommand`, `GoToAccountProfileCommand`, and `SignOutCommand` relay commands

#### Scenario: SignOutCommand shows confirmation and logs out
- **WHEN** `SignOutCommand` is executed and the user confirms
- **THEN** the ViewModel SHALL call `IAuthService.LogoutAsync()` and then execute `RefreshAuthStateCommand` to update `IsLoggedIn`

#### Scenario: SignOutCommand cancelled does nothing
- **WHEN** `SignOutCommand` is executed and the user cancels
- **THEN** no logout SHALL occur and `IsLoggedIn` SHALL remain true

#### Scenario: ViewModel checks auth state via RefreshAuthStateCommand
- **WHEN** `RefreshAuthStateCommand` is executed
- **THEN** the ViewModel SHALL call `IAuthService.IsLoggedInAsync()` and set `IsLoggedIn` to the result

#### Scenario: ViewModel receives IAuthService via DI
- **WHEN** the ProfileViewModel is constructed
- **THEN** it SHALL receive an `IAuthService` instance from the DI container

### Requirement: My Listings row in Account section
When the user is logged in, the Account section SHALL display a "My Listings" tappable row (`ProfileMyListings`) with a chevron indicator, positioned between "Account Profile" and "Sign Out". Tapping it SHALL navigate to the `my-listings` route. The row SHALL NOT be visible when logged out.

#### Scenario: My Listings row visible when logged in
- **WHEN** the profile hub is displayed and the user is logged in
- **THEN** a "My Listings" row with a chevron indicator SHALL be visible between "Account Profile" and "Sign Out"

#### Scenario: My Listings row hidden when logged out
- **WHEN** the profile hub is displayed and the user is not logged in
- **THEN** the "My Listings" row SHALL NOT be visible

#### Scenario: Tapping My Listings navigates to my-listings
- **WHEN** the user taps the "My Listings" row
- **THEN** the app SHALL navigate to the `my-listings` Shell route

### Requirement: My Listings localization in profile hub
The app SHALL define the localized string `ProfileMyListings` across all 4 languages (pl, en, uk, ru).

#### Scenario: My Listings label is localized
- **WHEN** the profile hub is displayed in any supported language
- **THEN** the "My Listings" row label SHALL use the localized `ProfileMyListings` string
