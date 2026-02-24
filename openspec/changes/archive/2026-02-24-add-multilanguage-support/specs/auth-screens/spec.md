## MODIFIED Requirements

### Requirement: Profile logged-out state
When the user is not logged in, the Profile tab SHALL display a centered welcome layout containing: the SportowyHub logo (`logo_full.png`), a localized "Welcome" title (`ProfileWelcome`), a primary "Create Account" button (`ProfileCreateAccount`), and a secondary "Sign In" button (`ProfileSignIn`). All text labels SHALL be sourced from `AppResources` localized resources.

#### Scenario: Profile shows welcome view when logged out
- **WHEN** the user navigates to the Profile tab and is not logged in
- **THEN** the screen SHALL display the SportowyHub logo, localized "Welcome" title, localized "Create Account" primary button, and localized "Sign In" secondary button, all centered vertically

#### Scenario: Create Account button navigates to Register page
- **WHEN** the user taps the "Create Account" button
- **THEN** the app SHALL navigate to the Registration page

#### Scenario: Sign In button navigates to Login page
- **WHEN** the user taps the "Sign In" button
- **THEN** the app SHALL navigate to the Login page

### Requirement: Registration screen with three fields
The Registration page SHALL display a vertical form with three labeled input fields: Email, Password, and Confirm Password. Each field label, placeholder, and the page title SHALL be sourced from `AppResources` localized resources (`AuthCreateAccount`, `AuthEmail`, `AuthEnterEmail`, `AuthPassword`, `AuthEnterPassword`, `AuthConfirmPassword`, `AuthConfirmYourPassword`). A primary "Create Account" button (`AuthCreateAccount`) SHALL be at the bottom.

#### Scenario: Registration form layout
- **WHEN** the Registration page is displayed
- **THEN** it SHALL show localized Email, Password, and Confirm Password fields in a vertical layout with a localized "Create Account" button at the bottom

#### Scenario: Registration button is disabled until form is valid
- **WHEN** the form has validation errors or empty required fields
- **THEN** the "Create Account" button SHALL be disabled (visually muted, not tappable)

#### Scenario: Registration button is enabled when form is valid
- **WHEN** all fields pass validation (valid email, password meets strength requirements, passwords match)
- **THEN** the "Create Account" button SHALL be enabled

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

### Requirement: Login screen with two fields
The Login page SHALL display a vertical form with localized Email and Password fields, a primary "Login" button (`AuthLogin`), a "Forgot password?" link (`AuthForgotPassword`), and a "Create Account" link (`AuthCreateAccount`) with a "Don't have an account?" prefix (`AuthNoAccount`). All text SHALL be sourced from `AppResources` localized resources.

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
- **THEN** a localized "Forgot password?" link SHALL be visible (non-functional in MVP)
