## ADDED Requirements

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
