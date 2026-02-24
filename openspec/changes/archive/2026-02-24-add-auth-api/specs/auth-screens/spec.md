## MODIFIED Requirements

### Requirement: Login screen with two fields
The Login page SHALL display a vertical form with localized Email and Password fields, a primary "Login" button (`AuthLogin`), a "Forgot password?" link (`AuthForgotPassword`), and a "Create Account" link (`AuthCreateAccount`) with a "Don't have an account?" prefix (`AuthNoAccount`). All text SHALL be sourced from `AppResources` localized resources. The Login page SHALL display a localized email validation error label below the email field when the email format is invalid, matching the Registration page pattern. The Login page SHALL display a general error message area above the Login button for server-side errors (invalid credentials, connection failures).

#### Scenario: Login form layout
- **WHEN** the Login page is displayed
- **THEN** it SHALL show localized Email and Password fields, a localized "Login" button, localized "Forgot password?" text, and a localized "Create Account" link

#### Scenario: Login button style
- **WHEN** the Login page is displayed
- **THEN** the "Login" button SHALL use the Primary button style (red background, white text, 16px radius, 50px height)

#### Scenario: Create Account link navigates to Registration
- **WHEN** the user taps the "Create Account" link on the Login page
- **THEN** the app SHALL navigate to the Registration page

#### Scenario: Forgot password link is present
- **WHEN** the Login page is displayed
- **THEN** a localized "Forgot password?" link SHALL be visible (non-functional in MVP — tapping it SHALL show no action)

#### Scenario: Invalid email shows error on Login page
- **WHEN** the user enters an invalid email format on the Login page
- **THEN** a localized error message SHALL appear below the email field

#### Scenario: Login button calls API
- **WHEN** the user taps the "Login" button with a valid email and password
- **THEN** the app SHALL call `IAuthService.LoginAsync` with the entered email and password

#### Scenario: Loading indicator during login
- **WHEN** the login API call is in progress
- **THEN** the Login button SHALL be disabled and an `ActivityIndicator` SHALL be visible

#### Scenario: Successful login navigates to home
- **WHEN** `LoginAsync` returns a successful `AuthResult`
- **THEN** the app SHALL navigate to the main shell (Home tab) using `//` absolute navigation

#### Scenario: Failed login shows server error
- **WHEN** `LoginAsync` returns a failed `AuthResult` with an error message
- **THEN** the error message SHALL be displayed in the general error area above the Login button

#### Scenario: Network error during login shows message
- **WHEN** `LoginAsync` fails due to a network error
- **THEN** a user-friendly connection error message SHALL be displayed

### Requirement: Registration screen with three fields
The Registration page SHALL display a vertical form with three labeled input fields: Email, Password, and Confirm Password. Each field label, placeholder, and the page title SHALL be sourced from `AppResources` localized resources (`AuthCreateAccount`, `AuthEmail`, `AuthEnterEmail`, `AuthPassword`, `AuthEnterPassword`, `AuthConfirmPassword`, `AuthConfirmYourPassword`). A primary "Create Account" button (`AuthCreateAccount`) SHALL be at the bottom. The Registration page SHALL display a general error message area above the Create Account button for server-side errors (email taken, validation failures).

#### Scenario: Registration form layout
- **WHEN** the Registration page is displayed
- **THEN** it SHALL show localized Email, Password, and Confirm Password fields in a vertical layout with a localized "Create Account" button at the bottom

#### Scenario: Registration button is disabled until form is valid
- **WHEN** the form has validation errors or empty required fields
- **THEN** the "Create Account" button SHALL be disabled (visually muted, not tappable)

#### Scenario: Registration button is enabled when form is valid
- **WHEN** all fields pass validation (valid email, password meets strength requirements, passwords match)
- **THEN** the "Create Account" button SHALL be enabled

#### Scenario: Create Account button calls API
- **WHEN** the user taps the "Create Account" button with valid form data
- **THEN** the app SHALL call `IAuthService.RegisterAsync` with the entered email and password

#### Scenario: Loading indicator during registration
- **WHEN** the registration API call is in progress
- **THEN** the Create Account button SHALL be disabled and an `ActivityIndicator` SHALL be visible

#### Scenario: Successful registration auto-logs in
- **WHEN** `RegisterAsync` returns a successful `AuthResult`
- **THEN** the app SHALL automatically call `LoginAsync` with the same credentials, and on success navigate to the main shell (Home tab)

#### Scenario: Registration with taken email shows error
- **WHEN** `RegisterAsync` returns a failed `AuthResult` with "Email already in use"
- **THEN** the error message SHALL be displayed in the email field error label or the general error area

#### Scenario: Registration with server validation errors shows field errors
- **WHEN** `RegisterAsync` returns a failed `AuthResult` with `FieldErrors`
- **THEN** each field error SHALL be displayed below the corresponding input field

#### Scenario: Network error during registration shows message
- **WHEN** `RegisterAsync` fails due to a network error
- **THEN** a user-friendly connection error message SHALL be displayed

## ADDED Requirements

### Requirement: LoginViewModel API integration
The `LoginViewModel` SHALL inject `IAuthService` via constructor. The `LoginCommand` SHALL call `IAuthService.LoginAsync(Email, Password)`. The ViewModel SHALL expose an `IsLoading` observable property that is true during API calls. The ViewModel SHALL expose a `LoginError` observable property for displaying server error messages. The ViewModel SHALL expose an `EmailError` observable property for email validation (matching the RegisterViewModel pattern). The `LoginCommand` SHALL be disabled when `IsLoading` is true or email/password are empty.

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

### Requirement: RegisterViewModel API integration
The `RegisterViewModel` SHALL inject `IAuthService` via constructor. The `CreateAccountCommand` SHALL call `IAuthService.RegisterAsync(Email, Password)`, then on success call `IAuthService.LoginAsync(Email, Password)` to auto-login. The ViewModel SHALL expose an `IsLoading` observable property that is true during API calls. The ViewModel SHALL expose a `RegisterError` observable property for displaying server error messages. The `CreateAccountCommand` SHALL be disabled when `IsLoading` is true.

#### Scenario: RegisterViewModel receives IAuthService via DI
- **WHEN** the `RegisterViewModel` is constructed
- **THEN** it SHALL receive an `IAuthService` instance from the DI container

#### Scenario: CreateAccountCommand disabled during loading
- **WHEN** `IsLoading` is true
- **THEN** `CreateAccountCommand.CanExecute` SHALL return false

#### Scenario: RegisterError cleared on new attempt
- **WHEN** the user taps Create Account after a previous failed attempt
- **THEN** `RegisterError` SHALL be cleared before the new API call begins

#### Scenario: Server email error shown in EmailError field
- **WHEN** the API returns a 409 "email taken" error
- **THEN** the `EmailError` property SHALL display the server's error message
