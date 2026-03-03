## MODIFIED Requirements

### Requirement: Login page layout
#### Scenario: Google OAuth button visible below login form
- **WHEN** the user views the LoginPage
- **THEN** an "OR" separator and a "Sign in with Google" button SHALL be visible below the Login button and above the "Create Account" link

#### Scenario: Google OAuth button triggers WebAuthenticator flow
- **WHEN** the user taps the "Sign in with Google" button
- **THEN** the `OAuthLoginWithGoogleCommand` SHALL execute and open the system browser via `WebAuthenticator`

#### Scenario: Google OAuth loading state
- **WHEN** the Google OAuth flow is in progress (`IsGoogleLoading` is true)
- **THEN** the Google button SHALL be disabled and show a loading indicator
- **AND** the email/password login form SHALL remain interactive

## ADDED Requirements

### Requirement: OAuth localization strings
The app SHALL define localized strings for the OAuth login UI across all 4 languages (pl, en, uk, ru): `OAuthOrSeparator` ("lub" / "or" / "або" / "или"), `OAuthSignInGoogle` ("Zaloguj przez Google" / "Sign in with Google" / "Увійти через Google" / "Войти через Google"), `OAuthErrorCancelled` (user cancelled message), `OAuthErrorFailed` (generic OAuth failure message).
