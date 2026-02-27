# Auth Screens

### Requirement: Profile logged-out state
When the user is not logged in, the Profile tab SHALL display a grouped-list hub layout. The Account section SHALL contain a "Sign In" row (`ProfileSignIn`) and a "Create Account" row (`ProfileCreateAccount`) as tappable list items with chevron indicators. The Sign In row Grid SHALL have `AutomationId="SignInRow"` and the Create Account row Grid SHALL have `AutomationId="CreateAccountRow"`. Tapping "Sign In" SHALL navigate to the Login page. Tapping "Create Account" SHALL navigate to the Registration page. All text labels SHALL be sourced from `AppResources` localized resources.

#### Scenario: Profile shows account rows when logged out
- **WHEN** the user navigates to the Profile tab and is not logged in
- **THEN** the screen SHALL display an Account section containing "Sign In" and "Create Account" tappable rows with chevron indicators

#### Scenario: Create Account row navigates to Register page
- **WHEN** the user taps the "Create Account" row
- **THEN** the app SHALL navigate to the Registration page

#### Scenario: Sign In row navigates to Login page
- **WHEN** the user taps the "Sign In" row
- **THEN** the app SHALL navigate to the Login page

#### Scenario: Sign In row is locatable by AutomationId
- **WHEN** the Profile page is displayed in logged-out state
- **THEN** the Sign In row Grid SHALL have `AutomationId="SignInRow"`

#### Scenario: Create Account row is locatable by AutomationId
- **WHEN** the Profile page is displayed in logged-out state
- **THEN** the Create Account row Grid SHALL have `AutomationId="CreateAccountRow"`

### Requirement: Registration screen with three fields
The Registration page SHALL display a vertical form with four labeled input fields: Email, Phone, Password, and Confirm Password. The Phone field SHALL use `Keyboard="Telephone"` and display a localized label (`AuthPhone`) and placeholder (`AuthEnterPhone`). The phone field SHALL appear between Email and Password. Each field label, placeholder, and the page title SHALL be sourced from `AppResources` localized resources. A primary "Create Account" button (`AuthCreateAccount`) SHALL be at the bottom. The Registration page SHALL display a general error message area above the Create Account button for server-side errors (email taken, validation failures). The headline Label SHALL have `AutomationId="RegisterHeadline"`. The Email Entry SHALL have `AutomationId="RegisterEmailEntry"`. The Phone Entry SHALL have `AutomationId="RegisterPhoneEntry"`. The Password Entry SHALL have `AutomationId="RegisterPasswordEntry"`. The Confirm Password Entry SHALL have `AutomationId="RegisterConfirmPasswordEntry"`.

#### Scenario: Registration form layout
- **WHEN** the Registration page is displayed
- **THEN** it SHALL show localized Email, Phone, Password, and Confirm Password fields in a vertical layout with a localized "Create Account" button at the bottom

#### Scenario: Phone field is required
- **WHEN** the user leaves the Phone field empty
- **THEN** the "Create Account" button SHALL be disabled

#### Scenario: Phone field error from server
- **WHEN** `RegisterAsync` returns a failed `AuthResult` with `FieldErrors` containing key "phone"
- **THEN** the error SHALL be displayed below the Phone field

#### Scenario: Registration button is disabled until form is valid
- **WHEN** the form has validation errors or empty required fields
- **THEN** the "Create Account" button SHALL be disabled (visually muted, not tappable)

#### Scenario: Registration button is enabled when form is valid
- **WHEN** all fields pass validation (valid email, phone filled, password meets strength requirements, passwords match)
- **THEN** the "Create Account" button SHALL be enabled

#### Scenario: Create Account button calls API
- **WHEN** the user taps the "Create Account" button with valid form data
- **THEN** the app SHALL call `IAuthService.RegisterAsync` with the entered email, password, confirm password, and phone

#### Scenario: Loading indicator during registration
- **WHEN** the registration API call is in progress
- **THEN** the Create Account button SHALL be disabled and an `ActivityIndicator` SHALL be visible

#### Scenario: Successful registration navigates to email verification
- **WHEN** `RegisterAsync` returns a successful `AuthResult` with `TrustLevel` other than `"TL0"`
- **THEN** the app SHALL navigate to the email verification page, passing the registered email address

#### Scenario: Successful registration without verification navigates to login
- **WHEN** `RegisterAsync` returns a successful `AuthResult` with `TrustLevel == "TL0"`
- **THEN** the app SHALL display a localized success alert (`AuthRegistrationSuccess` title, `AuthRegistrationSuccessMessage` body)
- **AND** after the user dismisses the alert, SHALL navigate back to the login page

#### Scenario: Registration with taken email shows error
- **WHEN** `RegisterAsync` returns a failed `AuthResult` with "Email already in use"
- **THEN** the error message SHALL be displayed in the email field error label or the general error area

#### Scenario: Registration with server validation errors shows field errors
- **WHEN** `RegisterAsync` returns a failed `AuthResult` with `FieldErrors`
- **THEN** each field error SHALL be displayed below the corresponding input field

#### Scenario: Network error during registration shows message
- **WHEN** `RegisterAsync` fails due to a network error
- **THEN** a user-friendly connection error message SHALL be displayed

#### Scenario: Registration headline is locatable by AutomationId
- **WHEN** the Registration page is displayed
- **THEN** the headline Label SHALL have `AutomationId="RegisterHeadline"`

#### Scenario: Register Email Entry is locatable by AutomationId
- **WHEN** the Registration page is displayed
- **THEN** the Email Entry SHALL have `AutomationId="RegisterEmailEntry"`

#### Scenario: Register Phone Entry is locatable by AutomationId
- **WHEN** the Registration page is displayed
- **THEN** the Phone Entry SHALL have `AutomationId="RegisterPhoneEntry"`

#### Scenario: Register Password Entry is locatable by AutomationId
- **WHEN** the Registration page is displayed
- **THEN** the Password Entry SHALL have `AutomationId="RegisterPasswordEntry"`

#### Scenario: Register Confirm Password Entry is locatable by AutomationId
- **WHEN** the Registration page is displayed
- **THEN** the Confirm Password Entry SHALL have `AutomationId="RegisterConfirmPasswordEntry"`

### Requirement: Email validation on Registration
The email field SHALL validate the input in real-time as the user types. A localized error message (`AuthInvalidEmail`) SHALL appear below the field when the email format is invalid.

#### Scenario: Valid email shows no error
- **WHEN** the user enters a valid email address (e.g., "user@example.com")
- **THEN** no error message SHALL be displayed below the email field

#### Scenario: Invalid email shows error
- **WHEN** the user enters an invalid email (e.g., "user@", "plaintext")
- **THEN** a localized error message SHALL appear below the email field indicating the email format is invalid

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
The Login page SHALL display a vertical form with localized Email and Password fields, a primary "Login" button (`AuthLogin`), a "Forgot password?" link (`AuthForgotPassword`), and a "Create Account" link (`AuthCreateAccount`) with a "Don't have an account?" prefix (`AuthNoAccount`). All text SHALL be sourced from `AppResources` localized resources. The Login page SHALL display a localized email validation error label below the email field when the email format is invalid, matching the Registration page pattern. The Login page SHALL display a general error message area above the Login button for server-side errors (invalid credentials, connection failures). The headline Label SHALL have `AutomationId="LoginHeadline"`. The Email Entry SHALL have `AutomationId="LoginEmailEntry"`. The Password Entry SHALL have `AutomationId="LoginPasswordEntry"`.

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

#### Scenario: Login headline is locatable by AutomationId
- **WHEN** the Login page is displayed
- **THEN** the headline Label SHALL have `AutomationId="LoginHeadline"`

#### Scenario: Login Email Entry is locatable by AutomationId
- **WHEN** the Login page is displayed
- **THEN** the Email Entry SHALL have `AutomationId="LoginEmailEntry"`

#### Scenario: Login Password Entry is locatable by AutomationId
- **WHEN** the Login page is displayed
- **THEN** the Password Entry SHALL have `AutomationId="LoginPasswordEntry"`

### Requirement: Error messages below fields
All form validation error messages across Registration and Login pages SHALL appear directly below the respective input field in red/error-colored text. Error messages SHALL appear and disappear in real-time as the user modifies input.

#### Scenario: Error message positioning
- **WHEN** a field has a validation error
- **THEN** the error message SHALL appear directly below that field's input, above the next field

#### Scenario: Error message clears on valid input
- **WHEN** the user corrects an invalid field value to a valid one
- **THEN** the error message SHALL disappear immediately

### Requirement: LoginViewModel API integration
The `LoginViewModel` SHALL inject `IAuthService` via constructor. The `LoginCommand` SHALL call `IAuthService.LoginAsync(Email, Password)`. On a failed result with `ErrorCode="EMAIL_NOT_VERIFIED"`, it SHALL navigate to the email verification page passing the email. On a failed result with `ErrorCode="USER_BANNED"`, it SHALL display the error in `LoginError`. On a failed result with `FieldErrors`, it SHALL display field-level errors (e.g., `EmailError` for an "email" field error). On a failed result without field errors, it SHALL display the error in `LoginError`. The ViewModel SHALL expose an `IsLoading` observable property that is true during API calls. The ViewModel SHALL expose a `LoginError` observable property for displaying server error messages. The ViewModel SHALL expose an `EmailError` observable property for email validation (matching the RegisterViewModel pattern). The `LoginCommand` SHALL be disabled when `IsLoading` is true or email/password are empty.

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

#### Scenario: Login with EMAIL_NOT_VERIFIED navigates to verification
- **WHEN** `LoginAsync` returns a failed `AuthResult` with `ErrorCode="EMAIL_NOT_VERIFIED"`
- **THEN** the ViewModel SHALL navigate to `email-verification?email={Email}`

#### Scenario: Login with USER_BANNED shows error
- **WHEN** `LoginAsync` returns a failed `AuthResult` with `ErrorCode="USER_BANNED"`
- **THEN** `LoginError` SHALL display the server's error message

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
