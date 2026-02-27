## MODIFIED Requirements

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
