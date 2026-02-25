# Email Verification

### Requirement: Email verification page layout
The `EmailVerificationPage` SHALL display a centered vertical layout with: a mail icon, a localized title (`EmailVerificationTitle`), a localized description (`EmailVerificationDescription`) that includes the user's email address, a primary "Resend Verification Email" button (`EmailVerificationResend`), and a "Back to Login" link (`EmailVerificationBackToLogin`). All text SHALL be sourced from `AppResources` localized resources.

#### Scenario: Email verification page displays after registration
- **WHEN** the user successfully registers
- **THEN** the app SHALL navigate to the email verification page showing the registered email address

#### Scenario: Email verification page layout
- **WHEN** the email verification page is displayed with email "user@example.com"
- **THEN** it SHALL show a mail icon, a localized title, a description containing "user@example.com", a "Resend Verification Email" button, and a "Back to Login" link

### Requirement: Resend verification email button
The "Resend Verification Email" button SHALL call `IAuthService.ResendVerificationAsync` with the displayed email. On success, the button SHALL show a localized success message (`EmailVerificationResent`) and be disabled for 60 seconds (cooldown). On failure, a localized error message SHALL be displayed.

#### Scenario: Resend verification succeeds
- **WHEN** the user taps "Resend Verification Email" and the API returns success
- **THEN** a localized success message SHALL be displayed and the button SHALL be disabled for 60 seconds

#### Scenario: Resend verification fails
- **WHEN** the user taps "Resend Verification Email" and the API returns failure
- **THEN** a localized error message SHALL be displayed

#### Scenario: Resend button cooldown
- **WHEN** the resend button cooldown is active
- **THEN** the button SHALL be disabled and show the remaining seconds in its text

#### Scenario: Loading indicator during resend
- **WHEN** the resend API call is in progress
- **THEN** the button SHALL be disabled and an `ActivityIndicator` SHALL be visible

### Requirement: Back to Login navigation
The "Back to Login" link SHALL navigate the user to the Login page using `//login` absolute navigation.

#### Scenario: Back to Login navigates to login page
- **WHEN** the user taps "Back to Login"
- **THEN** the app SHALL navigate to the Login page

### Requirement: EmailVerificationViewModel
The `EmailVerificationViewModel` SHALL accept an `email` query parameter via `IQueryAttributable`. It SHALL inject `IAuthService` via constructor. It SHALL expose `Email` (string), `IsLoading` (bool), `StatusMessage` (string), `IsStatusError` (bool), and `CooldownSeconds` (int) observable properties. The `ResendCommand` SHALL be disabled when `IsLoading` is true or `CooldownSeconds > 0`.

#### Scenario: ViewModel receives email via query parameter
- **WHEN** the email verification page is navigated to with `?email=user@example.com`
- **THEN** the `Email` property SHALL be "user@example.com"

#### Scenario: ResendCommand disabled during loading
- **WHEN** `IsLoading` is true
- **THEN** `ResendCommand.CanExecute` SHALL return false

#### Scenario: ResendCommand disabled during cooldown
- **WHEN** `CooldownSeconds` is greater than 0
- **THEN** `ResendCommand.CanExecute` SHALL return false

#### Scenario: Cooldown counts down after resend
- **WHEN** a resend succeeds
- **THEN** `CooldownSeconds` SHALL start at 60 and decrement every second until 0

### Requirement: Email verification route registration
The `email-verification` route SHALL be registered in `AppShell.xaml.cs` for `EmailVerificationPage`.

#### Scenario: Email verification route resolves
- **WHEN** the app navigates to `email-verification?email=user@example.com`
- **THEN** the `EmailVerificationPage` SHALL be displayed

### Requirement: Email verification DI registration
`MauiProgram.cs` SHALL register `EmailVerificationPage` as transient and `EmailVerificationViewModel` as transient in the DI container.

#### Scenario: EmailVerificationViewModel resolves from DI
- **WHEN** the DI container constructs `EmailVerificationViewModel`
- **THEN** it SHALL receive an `IAuthService` instance

### Requirement: Email verification localization strings
The following localization keys SHALL exist in all 4 locale `.resx` files (pl, en, uk, ru) and `AppResources.Designer.cs`: `EmailVerificationTitle`, `EmailVerificationDescription`, `EmailVerificationResend`, `EmailVerificationResent`, `EmailVerificationBackToLogin`, `EmailVerificationError`.

#### Scenario: Localization keys present in all locales
- **WHEN** the app loads any of the 4 supported locales
- **THEN** all email verification localization keys SHALL resolve to non-empty localized strings
