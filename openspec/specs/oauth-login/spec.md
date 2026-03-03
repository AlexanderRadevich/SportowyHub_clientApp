## Purpose

Defines the OAuth login flow, including the service method for authenticating with third-party providers (Google, Facebook) and the WebAuthenticator integration for capturing callback tokens.

## Requirements

### Requirement: OAuth login service method
`IAuthService` SHALL expose `OAuthLoginAsync(string provider, string? idToken, string? accessToken)` returning `Task<AuthResult<LoginResponse>>`. It SHALL POST to `/api/v1/auth/oauth/{provider}` with the provided tokens in the request body.

#### Scenario: Google OAuth login with id_token
- **WHEN** `OAuthLoginAsync("google", idToken: "google.jwt", accessToken: null)` is called
- **THEN** the service SHALL POST `{"id_token":"google.jwt"}` to `/api/v1/auth/oauth/google` and store auth tokens on success

#### Scenario: Facebook OAuth login with access_token
- **WHEN** `OAuthLoginAsync("facebook", idToken: null, accessToken: "fb.token")` is called
- **THEN** the service SHALL POST `{"access_token":"fb.token"}` to `/api/v1/auth/oauth/facebook`

#### Scenario: OAuth login for new user returns 201
- **WHEN** the backend returns 201 Created (new account)
- **THEN** the method SHALL treat it as success and store tokens

#### Scenario: OAuth login with invalid token
- **WHEN** the backend returns 401
- **THEN** the method SHALL return `AuthResult.Failure` with the error code

### Requirement: OAuthLoginRequest model
The app SHALL define an `OAuthLoginRequest` record with `IdToken` (string?) and `AccessToken` (string?). It SHALL be registered in `SportowyHubJsonContext`.

#### Scenario: Serialize with id_token only
- **WHEN** `OAuthLoginRequest` with IdToken="abc" and AccessToken=null is serialized
- **THEN** the JSON SHALL include `"id_token":"abc"`

### Requirement: WebAuthenticator integration
The app SHALL use `WebAuthenticator.AuthenticateAsync` to open the OAuth provider's login page and receive the callback token. Each provider SHALL have platform-specific URL scheme configuration.

#### Scenario: User completes Google sign-in
- **WHEN** the user completes the Google OAuth flow via WebAuthenticator
- **THEN** the app SHALL extract the `id_token` from the callback and pass it to `OAuthLoginAsync("google", idToken, null)`

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
