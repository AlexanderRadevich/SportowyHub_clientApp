## ADDED Requirements

### Requirement: Profile logged-out state
When the user is not logged in, the Profile tab SHALL display a centered welcome layout containing: the app logo, a "Welcome" title, a primary "Create Account" button, and a secondary "Sign In" button.

#### Scenario: Profile shows welcome view when logged out
- **WHEN** the user navigates to the Profile tab and is not logged in
- **THEN** the screen SHALL display the app logo, "Welcome" title, "Create Account" primary button, and "Sign In" secondary button, all centered vertically

#### Scenario: Create Account button navigates to Register page
- **WHEN** the user taps the "Create Account" button
- **THEN** the app SHALL navigate to the Registration page

#### Scenario: Sign In button navigates to Login page
- **WHEN** the user taps the "Sign In" button
- **THEN** the app SHALL navigate to the Login page

### Requirement: Registration screen with three fields
The Registration page SHALL display a vertical form with three labeled input fields: Email, Password, and Confirm Password. Each field SHALL have a label above it and space for an error message below it. A primary "Create Account" button SHALL be at the bottom.

#### Scenario: Registration form layout
- **WHEN** the Registration page is displayed
- **THEN** it SHALL show Email, Password, and Confirm Password fields in a vertical layout with a "Create Account" button at the bottom

#### Scenario: Registration button is disabled until form is valid
- **WHEN** the form has validation errors or empty required fields
- **THEN** the "Create Account" button SHALL be disabled (visually muted, not tappable)

#### Scenario: Registration button is enabled when form is valid
- **WHEN** all fields pass validation (valid email, password meets strength requirements, passwords match)
- **THEN** the "Create Account" button SHALL be enabled

### Requirement: Email validation on Registration
The email field SHALL validate the input in real-time as the user types. An error message SHALL appear below the field when the email format is invalid.

#### Scenario: Valid email shows no error
- **WHEN** the user enters a valid email address (e.g., "user@example.com")
- **THEN** no error message SHALL be displayed below the email field

#### Scenario: Invalid email shows error
- **WHEN** the user enters an invalid email (e.g., "user@", "plaintext")
- **THEN** an error message SHALL appear below the email field indicating the email format is invalid

### Requirement: Password strength indicator on Registration
The password field SHALL display a strength indicator below it that updates in real-time. Strength levels SHALL be: Weak (less than 6 characters), Medium (6+ characters with letters and numbers), Strong (8+ characters with letters, numbers, and special characters).

#### Scenario: Weak password
- **WHEN** the user enters a password shorter than 6 characters
- **THEN** the strength indicator SHALL show "Weak"

#### Scenario: Medium password
- **WHEN** the user enters a password of 6+ characters containing letters and numbers
- **THEN** the strength indicator SHALL show "Medium"

#### Scenario: Strong password
- **WHEN** the user enters a password of 8+ characters containing letters, numbers, and special characters
- **THEN** the strength indicator SHALL show "Strong"

### Requirement: Confirm password match validation
The Confirm Password field SHALL validate that its value matches the Password field. An error message SHALL appear when the values do not match.

#### Scenario: Passwords match
- **WHEN** the Confirm Password field value matches the Password field value
- **THEN** no error message SHALL be displayed

#### Scenario: Passwords do not match
- **WHEN** the Confirm Password field value differs from the Password field value
- **THEN** an error message SHALL appear indicating the passwords do not match

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
The Login page SHALL display a vertical form with Email and Password fields, a primary "Login" button, a "Forgot password?" link, and a "Create Account" link to the Registration page.

#### Scenario: Login form layout
- **WHEN** the Login page is displayed
- **THEN** it SHALL show Email and Password fields, a "Login" button, "Forgot password?" text, and a "Create Account" link

#### Scenario: Login button style
- **WHEN** the Login page is displayed
- **THEN** the "Login" button SHALL use the Primary button style (red background, white text, 16px radius, 50px height)

#### Scenario: Create Account link navigates to Registration
- **WHEN** the user taps the "Create Account" link on the Login page
- **THEN** the app SHALL navigate to the Registration page

#### Scenario: Forgot password link is present
- **WHEN** the Login page is displayed
- **THEN** a "Forgot password?" link SHALL be visible (non-functional in MVP — tapping it SHALL show no action)

### Requirement: Error messages below fields
All form validation error messages across Registration and Login pages SHALL appear directly below the respective input field in red/error-colored text. Error messages SHALL appear and disappear in real-time as the user modifies input.

#### Scenario: Error message positioning
- **WHEN** a field has a validation error
- **THEN** the error message SHALL appear directly below that field's input, above the next field

#### Scenario: Error message clears on valid input
- **WHEN** the user corrects an invalid field value to a valid one
- **THEN** the error message SHALL disappear immediately
