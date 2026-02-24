# Auth Screens

### Requirement: Profile logged-out state
When the user is not logged in, the Profile tab SHALL display a grouped-list hub layout. The Account section SHALL contain a "Sign In" row (`ProfileSignIn`) and a "Create Account" row (`ProfileCreateAccount`) as tappable list items with chevron indicators. Tapping "Sign In" SHALL navigate to the Login page. Tapping "Create Account" SHALL navigate to the Registration page. All text labels SHALL be sourced from `AppResources` localized resources.

#### Scenario: Profile shows account rows when logged out
- **WHEN** the user navigates to the Profile tab and is not logged in
- **THEN** the screen SHALL display an Account section containing "Sign In" and "Create Account" tappable rows with chevron indicators

#### Scenario: Create Account row navigates to Register page
- **WHEN** the user taps the "Create Account" row
- **THEN** the app SHALL navigate to the Registration page

#### Scenario: Sign In row navigates to Login page
- **WHEN** the user taps the "Sign In" row
- **THEN** the app SHALL navigate to the Login page

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

### Requirement: Email validation on Registration
The email field SHALL validate the input in real-time as the user types. A localized error message (`AuthInvalidEmail`) SHALL appear below the field when the email format is invalid.

#### Scenario: Valid email shows no error
- **WHEN** the user enters a valid email address (e.g., "user@example.com")
- **THEN** no error message SHALL be displayed below the email field

#### Scenario: Invalid email shows error
- **WHEN** the user enters an invalid email (e.g., "user@", "plaintext")
- **THEN** a localized error message SHALL appear below the email field indicating the email format is invalid

### Requirement: Password strength indicator on Registration
The password field SHALL display a localized strength indicator below it that updates in real-time. Strength levels SHALL be: Weak (`PasswordStrengthWeak` — less than 6 characters), Medium (`PasswordStrengthMedium` — 6+ characters with letters and numbers), Strong (`PasswordStrengthStrong` — 8+ characters with letters, numbers, and special characters).

#### Scenario: Weak password
- **WHEN** the user enters a password shorter than 6 characters
- **THEN** the strength indicator SHALL show the localized "Weak" label

#### Scenario: Medium password
- **WHEN** the user enters a password of 6+ characters containing letters and numbers
- **THEN** the strength indicator SHALL show the localized "Medium" label

#### Scenario: Strong password
- **WHEN** the user enters a password of 8+ characters containing letters, numbers, and special characters
- **THEN** the strength indicator SHALL show the localized "Strong" label

### Requirement: Confirm password match validation
The Confirm Password field SHALL validate that its value matches the Password field. A localized error message (`AuthPasswordsDoNotMatch`) SHALL appear when the values do not match.

#### Scenario: Passwords match
- **WHEN** the Confirm Password field value matches the Password field value
- **THEN** no error message SHALL be displayed

#### Scenario: Passwords do not match
- **WHEN** the Confirm Password field value differs from the Password field value
- **THEN** a localized error message SHALL appear indicating the passwords do not match

### Requirement: Show/hide password toggle
Both the Password and Confirm Password fields on the Registration page, and the Password field on the Login page, SHALL include an eye icon toggle to show or hide the password text.

#### Scenario: Password is hidden by default
- **WHEN** a password field is displayed
- **THEN** the text SHALL be masked (dots/bullets) and the eye icon SHALL indicate "show"

#### Scenario: Toggling password visibility
- **WHEN** the user taps the eye icon on a password field
- **THEN** the password text SHALL become visible and the icon SHALL change to indicate "hide"

#### Scenario: Toggling back to hidden
- **WHEN** the user taps the eye icon again
- **THEN** the password text SHALL be masked again

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

### Requirement: Error messages below fields
All form validation error messages across Registration and Login pages SHALL appear directly below the respective input field in red/error-colored text. Error messages SHALL appear and disappear in real-time as the user modifies input.

#### Scenario: Error message positioning
- **WHEN** a field has a validation error
- **THEN** the error message SHALL appear directly below that field's input, above the next field

#### Scenario: Error message clears on valid input
- **WHEN** the user corrects an invalid field value to a valid one
- **THEN** the error message SHALL disappear immediately

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
