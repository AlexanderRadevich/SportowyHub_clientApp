## MODIFIED Requirements

### Requirement: Account Profile placeholder page
The app SHALL have an `AccountProfilePage` registered as shell route `account-profile`. The page SHALL display a centered localized placeholder label. The page SHALL hide the tab bar (`Shell.TabBarIsVisible="False"`). The page SHALL use `AccountProfileViewModel` as its `BindingContext`, injected via constructor. The page SHALL display a "Sign Out" button below the placeholder content.

#### Scenario: Account Profile page displays placeholder
- **WHEN** the user navigates to the Account Profile page
- **THEN** a centered placeholder label SHALL be visible

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
- **WHEN** the Account Profile page is displayed
- **THEN** a "Sign Out" button SHALL be visible with localized text from `AppResources.SignOut`
- **AND** the button SHALL have `AutomationId="SignOutButton"`

#### Scenario: Sign Out button has destructive styling
- **WHEN** the Account Profile page is displayed
- **THEN** the Sign Out button SHALL use red text color (`Primary` theme color) with transparent background to indicate a destructive action

## ADDED Requirements

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
