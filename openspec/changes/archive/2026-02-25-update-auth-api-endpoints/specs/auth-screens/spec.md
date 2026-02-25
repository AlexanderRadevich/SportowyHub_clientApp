## MODIFIED Requirements

### Requirement: Password strength indicator on Registration
The password field SHALL display a localized strength indicator below it that updates in real-time. Strength levels SHALL be: Weak (`PasswordStrengthWeak` — less than 8 characters), Medium (`PasswordStrengthMedium` — 8+ characters with letters and digits), Strong (`PasswordStrengthStrong` — 10+ characters with letters, digits, and special characters).

#### Scenario: Weak password
- **WHEN** the user enters a password shorter than 8 characters
- **THEN** the strength indicator SHALL show the localized "Weak" label

#### Scenario: Medium password
- **WHEN** the user enters a password of 8+ characters containing letters and digits
- **THEN** the strength indicator SHALL show the localized "Medium" label

#### Scenario: Strong password
- **WHEN** the user enters a password of 10+ characters containing letters, digits, and special characters
- **THEN** the strength indicator SHALL show the localized "Strong" label

### Requirement: Registration screen with three fields
The Registration page SHALL display a vertical form with four labeled input fields: Email, Phone, Password, and Confirm Password. The Phone field SHALL use `Keyboard="Telephone"` and display a localized label (`AuthPhone`) and placeholder (`AuthEnterPhone`). The phone field SHALL appear between Email and Password. Each field label, placeholder, and the page title SHALL be sourced from `AppResources` localized resources. A primary "Create Account" button (`AuthCreateAccount`) SHALL be at the bottom. The Registration page SHALL display a general error message area above the Create Account button for server-side errors (email taken, validation failures).

#### Scenario: Registration form layout
- **WHEN** the Registration page is displayed
- **THEN** it SHALL show localized Email, Phone, Password, and Confirm Password fields in a vertical layout with a localized "Create Account" button at the bottom

#### Scenario: Phone field is required
- **WHEN** the user leaves the Phone field empty
- **THEN** the "Create Account" button SHALL be disabled

#### Scenario: Phone field error from server
- **WHEN** `RegisterAsync` returns a failed `AuthResult` with `FieldErrors` containing key "phone"
- **THEN** the error SHALL be displayed below the Phone field

### Requirement: RegisterViewModel API integration
The `RegisterViewModel` SHALL inject `IAuthService` via constructor. The `CreateAccountCommand` SHALL call `IAuthService.RegisterAsync(Email, Password, Phone)`, passing the required phone value, then on success call `IAuthService.LoginAsync(Email, Password)` to auto-login. The ViewModel SHALL expose `Phone` (string) as an observable property. The ViewModel SHALL expose an `IsLoading` observable property that is true during API calls. The ViewModel SHALL expose a `RegisterError` observable property for displaying server error messages. The `CreateAccountCommand` SHALL be disabled when `IsLoading` is true. The `CreateAccountCommand` SHALL require the password to be at least 8 characters (matching the API's minimum password length).

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

### Requirement: LoginViewModel API integration
The `LoginViewModel` SHALL inject `IAuthService` via constructor. The `LoginCommand` SHALL call `IAuthService.LoginAsync(Email, Password)`. On a failed result with `FieldErrors`, it SHALL display field-level errors (e.g., `EmailError` for an "email" field error). On a failed result without field errors, it SHALL display the error in `LoginError`. The ViewModel SHALL expose an `IsLoading` observable property that is true during API calls. The ViewModel SHALL expose a `LoginError` observable property for displaying server error messages. The ViewModel SHALL expose an `EmailError` observable property for email validation (matching the RegisterViewModel pattern). The `LoginCommand` SHALL be disabled when `IsLoading` is true or email/password are empty.

#### Scenario: LoginViewModel receives IAuthService via DI
- **WHEN** the `LoginViewModel` is constructed
- **THEN** it SHALL receive an `IAuthService` instance from the DI container

#### Scenario: LoginCommand disabled during loading
- **WHEN** `IsLoading` is true
- **THEN** `LoginCommand.CanExecute` SHALL return false

#### Scenario: LoginCommand disabled with empty fields
- **WHEN** Email or Password is empty
- **THEN** `LoginCommand.CanExecute` SHALL return false

#### Scenario: LoginError cleared on new attempt
- **WHEN** the user taps Login after a previous failed attempt
- **THEN** `LoginError` SHALL be cleared before the new API call begins

#### Scenario: Login with validation field errors shows EmailError
- **WHEN** `LoginAsync` returns a failed `AuthResult` with `FieldErrors` containing key "email"
- **THEN** the `EmailError` property SHALL display the server's field error message
