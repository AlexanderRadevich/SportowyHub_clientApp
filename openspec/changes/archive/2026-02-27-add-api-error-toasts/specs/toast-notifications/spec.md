## ADDED Requirements

### Requirement: Toast notification service
The app SHALL provide an `IToastService` interface with a `ShowError(string message)` method. The implementation SHALL be registered as a singleton in the DI container. The method SHALL display the message using `CommunityToolkit.Maui` Snackbar with a 6-second auto-dismiss duration and no action button. The method SHALL dispatch to the main thread to ensure it works from background async contexts.

#### Scenario: ShowError displays a snackbar for 6 seconds
- **WHEN** `IToastService.ShowError("Network error")` is called
- **THEN** a Snackbar SHALL appear with text "Network error"
- **AND** the Snackbar SHALL auto-dismiss after 6 seconds

#### Scenario: ShowError works from background thread
- **WHEN** `ShowError` is called from a non-UI thread (e.g., inside an async service callback)
- **THEN** the Snackbar SHALL still display correctly by dispatching to the main thread

#### Scenario: IToastService is registered in DI
- **WHEN** the app starts
- **THEN** `IToastService` SHALL be resolvable from the DI container as a singleton

### Requirement: Error toast on login failure
The `LoginViewModel` SHALL show an error toast via `IToastService.ShowError` when an unhandled exception occurs during the login command. Existing inline error display (`LoginError` property) SHALL remain unchanged for `AuthResult` failures.

#### Scenario: Unhandled exception during login shows toast
- **WHEN** the user submits the login form
- **AND** an unhandled exception occurs (e.g., network timeout)
- **THEN** an error toast SHALL appear with the exception message

#### Scenario: AuthResult failure still shows inline error
- **WHEN** the user submits the login form
- **AND** the API returns a structured error (e.g., invalid credentials)
- **THEN** the inline `LoginError` label SHALL display the error message
- **AND** no toast SHALL appear for the structured error

### Requirement: Error toast on registration failure
The `RegisterViewModel` SHALL show an error toast via `IToastService.ShowError` when an unhandled exception occurs during the create account command. Existing inline error display SHALL remain unchanged for `AuthResult` failures.

#### Scenario: Unhandled exception during registration shows toast
- **WHEN** the user submits the registration form
- **AND** an unhandled exception occurs
- **THEN** an error toast SHALL appear with the exception message

### Requirement: Error toast on resend verification failure
The `EmailVerificationViewModel` SHALL show an error toast via `IToastService.ShowError` when an unhandled exception occurs during the resend command. Existing inline status message display SHALL remain unchanged for `AuthResult` failures.

#### Scenario: Unhandled exception during resend shows toast
- **WHEN** the user taps "Resend verification email"
- **AND** an unhandled exception occurs
- **THEN** an error toast SHALL appear with the exception message

### Requirement: Error toast on profile load failure
The `AccountProfileViewModel` SHALL show an error toast via `IToastService.ShowError` when `GetProfileAsync()` throws an exception. The `HasError` flag SHALL still be set to `true` for the UI error state.

#### Scenario: Exception during profile load shows toast
- **WHEN** the Account Profile page appears
- **AND** `GetProfileAsync()` throws an exception
- **THEN** an error toast SHALL appear with the exception message
- **AND** `HasError` SHALL be `true`

#### Scenario: Null profile without exception shows error state only
- **WHEN** `GetProfileAsync()` returns `null` (e.g., no token)
- **THEN** `HasError` SHALL be `true`
- **AND** no toast SHALL appear (no exception occurred)

### Requirement: Error toast on sign-out failure
The `AccountProfileViewModel.SignOutCommand` and `ProfileViewModel.SignOutCommand` SHALL show an error toast via `IToastService.ShowError` if `LogoutAsync()` throws an exception. The local sign-out (token clearing and navigation) SHALL still proceed regardless of the exception.

#### Scenario: Exception during sign-out shows toast but completes sign-out
- **WHEN** the user confirms sign-out
- **AND** `LogoutAsync()` throws an exception
- **THEN** an error toast SHALL appear with the exception message
- **AND** the user SHALL still be signed out locally and navigated away

#### Scenario: Successful sign-out shows no toast
- **WHEN** the user confirms sign-out
- **AND** `LogoutAsync()` completes without exception
- **THEN** no toast SHALL appear
