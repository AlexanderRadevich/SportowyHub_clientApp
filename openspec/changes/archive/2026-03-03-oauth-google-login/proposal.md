## Why

The app has a complete `OAuthLoginAsync` service method but no UI to trigger it. Users cannot sign in with Google despite the backend fully supporting it. Adding a Google Sign-In button to the login page reduces friction for new users and completes the OAuth integration started in the sync-api-endpoints change.

## What Changes

- Add a "Sign in with Google" button to the LoginPage below the existing login form
- Add `OAuthLoginWithGoogleCommand` to `LoginViewModel` using `WebAuthenticator`
- Configure platform-specific URL schemes for the OAuth callback (Android, iOS, Windows)
- Add localized strings for the OAuth button and error messages across 4 languages
- Add an "OR" separator between the password form and OAuth section

## Capabilities

### New Capabilities

_None — this extends existing capabilities._

### Modified Capabilities

- `auth-screens`: Adding Google OAuth button to LoginPage UI and localized strings
- `oauth-login`: Adding WebAuthenticator platform configuration and token extraction flow

## Impact

- `SportowyHub.App/ViewModels/LoginViewModel.cs` — new command and OAuth state
- `SportowyHub.App/Views/Auth/LoginPage.xaml` — new button and separator
- `SportowyHub.App/Platforms/Android/AndroidManifest.xml` — intent filter for callback URL
- `SportowyHub.App/Platforms/iOS/Info.plist` — URL scheme registration
- `SportowyHub.App/Resources/Strings/AppResources.*.resx` — 4 new strings × 4 languages
- `SportowyHub.App/Resources/Strings/AppResources.Designer.cs` — new properties
