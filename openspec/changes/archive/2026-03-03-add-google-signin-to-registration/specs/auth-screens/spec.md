## MODIFIED Requirements

### Requirement: Registration screen with three fields
The Registration page SHALL display a vertical form with four labeled input fields: Email, Phone, Password, and Confirm Password. The Phone field SHALL use `Keyboard="Telephone"` and display a localized label (`AuthPhone`) and placeholder (`AuthEnterPhone`). The phone field SHALL appear between Email and Password. Each field label, placeholder, and the page title SHALL be sourced from `AppResources` localized resources. A primary "Create Account" button (`AuthCreateAccount`) SHALL be at the bottom of the form fields. Below the Create Account button, an "OR" separator and a "Sign in with Google" button SHALL be displayed, matching the Login page layout pattern. The OR separator SHALL use the existing `OAuthOrSeparator` localized string. The Google button SHALL use the `SecondaryButton` style, the `OAuthSignInGoogle` localized string, and `AutomationId="RegisterGoogleSignInButton"`. The Registration page SHALL display a general error message area above the Create Account button for server-side errors (email taken, validation failures). The headline Label SHALL have `AutomationId="RegisterHeadline"`. The Email Entry SHALL have `AutomationId="RegisterEmailEntry"`. The Phone Entry SHALL have `AutomationId="RegisterPhoneEntry"`. The Password Entry SHALL have `AutomationId="RegisterPasswordEntry"`. The Confirm Password Entry SHALL have `AutomationId="RegisterConfirmPasswordEntry"`.

#### Scenario: Registration form layout
- **WHEN** the Registration page is displayed
- **THEN** it SHALL show localized Email, Phone, Password, and Confirm Password fields in a vertical layout with a localized "Create Account" button, an "OR" separator, and a "Sign in with Google" button below

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

#### Scenario: Google sign-in button visible on registration page
- **WHEN** the Registration page is displayed
- **THEN** an "OR" separator and a "Sign in with Google" button SHALL be visible below the "Create Account" button

#### Scenario: Google sign-in button uses SecondaryButton style
- **WHEN** the Registration page is displayed
- **THEN** the Google sign-in button SHALL use the `SecondaryButton` style and the `OAuthSignInGoogle` localized text

#### Scenario: Register Google sign-in button is locatable by AutomationId
- **WHEN** the Registration page is displayed
- **THEN** the Google sign-in button SHALL have `AutomationId="RegisterGoogleSignInButton"`

### Requirement: RegisterViewModel API integration
The `RegisterViewModel` SHALL inject `IAuthService` via constructor. The `CreateAccountCommand` SHALL call `IAuthService.RegisterAsync(Email, Password, ConfirmPassword, Phone)`, passing the confirm password and required phone values. On success with `TrustLevel` other than `"TL0"`, it SHALL navigate to the email verification page with the registered email. On success with `TrustLevel == "TL0"`, it SHALL show a localized success alert and navigate back to the login page. The ViewModel SHALL expose `Phone` (string) as an observable property. The ViewModel SHALL expose an `IsLoading` observable property that is true during API calls. The ViewModel SHALL expose a `RegisterError` observable property for displaying server error messages. The `CreateAccountCommand` SHALL be disabled when `IsLoading` is true. The `CreateAccountCommand` SHALL require the password to be at least 8 characters (matching the API's minimum password length). The ViewModel SHALL expose an `OAuthLoginWithGoogleCommand` that calls `IAuthService.AcquireGoogleIdTokenAsync` followed by `IAuthService.OAuthLoginAsync("google", idToken, null)`. The ViewModel SHALL expose an `IsGoogleLoading` observable property that is true during the Google OAuth flow. On successful OAuth login, the ViewModel SHALL navigate to `//home`. On `TaskCanceledException` (user cancelled browser), the command SHALL silently return. On other exceptions, the ViewModel SHALL show an error toast.

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

#### Scenario: Google OAuth command triggers PKCE flow
- **WHEN** the user taps the "Sign in with Google" button on the Registration page
- **THEN** the `OAuthLoginWithGoogleCommand` SHALL call `IAuthService.AcquireGoogleIdTokenAsync` to obtain an ID token via the PKCE flow

#### Scenario: Google OAuth loading state on registration
- **WHEN** the Google OAuth flow is in progress on the Registration page
- **THEN** `IsGoogleLoading` SHALL be true, the Google button SHALL be disabled, and an `ActivityIndicator` SHALL be visible
- **AND** the registration form fields and "Create Account" button SHALL remain interactive

#### Scenario: Google OAuth success navigates to home from registration
- **WHEN** `OAuthLoginAsync` returns a successful result after Google sign-in on the Registration page
- **THEN** the ViewModel SHALL navigate to `//home`

#### Scenario: Google OAuth user cancellation on registration
- **WHEN** the user cancels the Google OAuth browser flow on the Registration page (throws `TaskCanceledException`)
- **THEN** the command SHALL silently return without showing an error

#### Scenario: Google OAuth failure on registration shows toast
- **WHEN** `AcquireGoogleIdTokenAsync` returns null or `OAuthLoginAsync` fails on the Registration page
- **THEN** the ViewModel SHALL show an error toast with `OAuthErrorFailed`

#### Scenario: Google OAuth clears RegisterError
- **WHEN** the user taps the "Sign in with Google" button on the Registration page
- **THEN** `RegisterError` SHALL be cleared before the OAuth flow begins
