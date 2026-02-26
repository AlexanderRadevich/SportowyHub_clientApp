## MODIFIED Requirements

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
The Registration page SHALL display a vertical form with four labeled input fields: Email, Phone, Password, and Confirm Password. The Phone field SHALL use `Keyboard="Telephone"` and display a localized label (`AuthPhone`) and placeholder (`AuthEnterPhone`). The phone field SHALL appear between Email and Password. Each field label, placeholder, and the page title SHALL be sourced from `AppResources` localized resources. A primary "Create Account" button (`AuthCreateAccount`) SHALL be at the bottom. The Registration page SHALL display a general error message area above the Create Account button for server-side errors (email taken, validation failures). The headline Label SHALL have `AutomationId="RegisterHeadline"`.

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

### Requirement: Login screen with two fields
The Login page SHALL display a vertical form with localized Email and Password fields, a primary "Login" button (`AuthLogin`), a "Forgot password?" link (`AuthForgotPassword`), and a "Create Account" link (`AuthCreateAccount`) with a "Don't have an account?" prefix (`AuthNoAccount`). All text SHALL be sourced from `AppResources` localized resources. The Login page SHALL display a localized email validation error label below the email field when the email format is invalid, matching the Registration page pattern. The Login page SHALL display a general error message area above the Login button for server-side errors (invalid credentials, connection failures). The headline Label SHALL have `AutomationId="LoginHeadline"`.

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
