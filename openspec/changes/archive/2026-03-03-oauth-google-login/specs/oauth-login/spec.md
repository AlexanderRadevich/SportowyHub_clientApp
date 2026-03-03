## MODIFIED Requirements

### Requirement: WebAuthenticator integration
#### Scenario: Platform URL scheme configuration for Android
- **WHEN** the app is built for Android
- **THEN** the `AndroidManifest.xml` SHALL include an intent filter with the callback URL scheme for the OAuth redirect

#### Scenario: Platform URL scheme configuration for iOS
- **WHEN** the app is built for iOS
- **THEN** the `Info.plist` SHALL include a `CFBundleURLSchemes` entry for the OAuth callback URL scheme

#### Scenario: Google OAuth client ID configuration
- **WHEN** the OAuth flow is initiated
- **THEN** the Google client ID SHALL be read from `ApiConfig.GoogleClientId` (not hardcoded in the ViewModel)

#### Scenario: User cancels Google sign-in
- **WHEN** the user dismisses the system browser without completing sign-in
- **THEN** `WebAuthenticator` SHALL throw `TaskCanceledException`
- **AND** the app SHALL silently return to the LoginPage without showing an error

#### Scenario: Google OAuth token extraction
- **WHEN** `WebAuthenticator.AuthenticateAsync` returns successfully
- **THEN** the app SHALL extract the `id_token` from `WebAuthenticatorResult.Properties["id_token"]` or `WebAuthenticatorResult.IdToken`
- **AND** pass it to `OAuthLoginAsync("google", idToken, null)`

#### Scenario: Successful Google login navigates home
- **WHEN** `OAuthLoginAsync` returns success
- **THEN** the app SHALL navigate back from the login page and then to `//home`
