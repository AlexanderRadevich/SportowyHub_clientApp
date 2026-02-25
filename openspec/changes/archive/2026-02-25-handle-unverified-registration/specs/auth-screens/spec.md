## MODIFIED Requirements

### Requirement: Successful registration navigates to email verification
- **WHEN** `RegisterAsync` returns a successful `AuthResult`
- **THEN** the app SHALL navigate to the email verification page, passing the registered email address

#### Scenario: Successful registration navigates to email verification
- **WHEN** `RegisterAsync` returns a successful `AuthResult` with `TrustLevel` other than `"TL0"`
- **THEN** the ViewModel SHALL navigate to `email-verification?email={Email}`

#### Scenario: Successful registration without verification navigates to login
- **WHEN** `RegisterAsync` returns a successful `AuthResult` with `TrustLevel == "TL0"`
- **THEN** the ViewModel SHALL display a localized success alert (`AuthRegistrationSuccess` title, `AuthRegistrationSuccessMessage` body)
- **AND** after the user dismisses the alert, SHALL navigate back to the login page

### Requirement: RegisterViewModel API integration
The `RegisterViewModel` SHALL inject `IAuthService` via constructor. The `CreateAccountCommand` SHALL call `IAuthService.RegisterAsync(Email, Password, ConfirmPassword, Phone)`, passing the confirm password and required phone values. On success with `TrustLevel` other than `"TL0"`, it SHALL navigate to the email verification page with the registered email. On success with `TrustLevel == "TL0"`, it SHALL show a localized success alert and navigate back to the login page. The ViewModel SHALL expose `Phone` (string) as an observable property. The ViewModel SHALL expose an `IsLoading` observable property that is true during API calls. The ViewModel SHALL expose a `RegisterError` observable property for displaying server error messages. The `CreateAccountCommand` SHALL be disabled when `IsLoading` is true. The `CreateAccountCommand` SHALL require the password to be at least 8 characters (matching the API's minimum password length).

#### Scenario: RegisterViewModel receives IAuthService via DI
- **WHEN** the `RegisterViewModel` is constructed
- **THEN** it SHALL receive an `IAuthService` instance from the DI container

#### Scenario: CreateAccountCommand disabled during loading
- **WHEN** `IsLoading` is true
- **THEN** `CreateAccountCommand.CanExecute` SHALL return false

#### Scenario: CreateAccountCommand disabled with short password
- **WHEN** the password is fewer than 8 characters
- **THEN** `CreateAccountCommand.CanExecute` SHALL return false

#### Scenario: RegisterError cleared on new attempt
- **WHEN** the user taps Create Account after a previous failed attempt
- **THEN** `RegisterError` SHALL be cleared before the new API call begins

#### Scenario: Server email error shown in EmailError field
- **WHEN** the API returns a 409 "email taken" error
- **THEN** the `EmailError` property SHALL display the server's error message

#### Scenario: Successful registration navigates to email verification
- **WHEN** `RegisterAsync` returns a successful `AuthResult` with `TrustLevel` other than `"TL0"`
- **THEN** the ViewModel SHALL navigate to `email-verification?email={Email}`

#### Scenario: Successful unverified registration shows alert and navigates to login
- **WHEN** `RegisterAsync` returns a successful `AuthResult` with `TrustLevel == "TL0"`
- **THEN** the ViewModel SHALL display a `DisplayAlert` with localized title `AuthRegistrationSuccess` and message `AuthRegistrationSuccessMessage`
- **AND** after dismissal, SHALL navigate to the login page via `Shell.Current.GoToAsync("..")`
