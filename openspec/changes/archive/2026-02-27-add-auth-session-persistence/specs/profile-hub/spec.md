## MODIFIED Requirements

### Requirement: Account section with auth rows
The Account section SHALL display a section header with the localized text `ProfileAccountSection`. Below the header, the section SHALL conditionally display rows based on auth state. When the user is NOT logged in, the section SHALL show two tappable rows: "Sign In" (`ProfileSignIn`) and "Create Account" (`ProfileCreateAccount`) with chevron indicators. When the user IS logged in, the section SHALL show a single tappable "Account Profile" row (`ProfileAccountProfile`) with a chevron indicator that navigates to the Account Profile page. Auth state SHALL be refreshed every time the Profile page appears.

#### Scenario: Account section displays Sign In and Create Account rows when logged out
- **WHEN** the profile hub is displayed and the user is not logged in
- **THEN** the Account section SHALL show "Sign In" and "Create Account" as tappable rows with chevron indicators

#### Scenario: Account section displays Account Profile row when logged in
- **WHEN** the profile hub is displayed and the user is logged in
- **THEN** the Account section SHALL show an "Account Profile" tappable row with a chevron indicator
- **AND** the "Sign In" and "Create Account" rows SHALL NOT be visible

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
The profile hub SHALL be backed by a `ProfileViewModel` using `CommunityToolkit.Mvvm`. The ViewModel SHALL receive `IAuthService` via constructor injection. The ViewModel SHALL expose `RelayCommand`s for Sign In, Create Account, and Account Profile navigation, and properties for the selected language and theme bound to Picker controls. The ViewModel SHALL expose an `IsLoggedIn` observable boolean property. The ViewModel SHALL expose a `RefreshAuthStateCommand` that calls `IAuthService.IsLoggedInAsync()` and updates `IsLoggedIn`.

#### Scenario: ViewModel provides navigation commands
- **WHEN** the ProfileViewModel is instantiated
- **THEN** it SHALL expose `SignInCommand`, `CreateAccountCommand`, and `GoToAccountProfileCommand` relay commands that navigate to the respective pages

#### Scenario: ViewModel checks auth state via RefreshAuthStateCommand
- **WHEN** `RefreshAuthStateCommand` is executed
- **THEN** the ViewModel SHALL call `IAuthService.IsLoggedInAsync()` and set `IsLoggedIn` to the result

#### Scenario: ViewModel receives IAuthService via DI
- **WHEN** the ProfileViewModel is constructed
- **THEN** it SHALL receive an `IAuthService` instance from the DI container
