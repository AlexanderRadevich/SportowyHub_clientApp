## 1. Configuration

- [x] 1.1 Add `GoogleClientId` property to `ApiConfig` class with the Google OAuth client ID
- [x] 1.2 Add `OAuthCallbackScheme` property to `ApiConfig` (e.g., `"sportowyHub"`)

## 2. Platform Configuration

- [x] 2.1 Add intent filter to `Platforms/Android/AndroidManifest.xml` for the OAuth callback URL scheme
- [x] 2.2 Add `CFBundleURLSchemes` entry to `Platforms/iOS/Info.plist` for the OAuth callback URL scheme

## 3. Localization

- [x] 3.1 Add localized strings to all 4 `.resx` files: `OAuthOrSeparator`, `OAuthSignInGoogle`, `OAuthErrorCancelled`, `OAuthErrorFailed`
- [x] 3.2 Add corresponding properties to `AppResources.Designer.cs`

## 4. ViewModel

- [x] 4.1 Add `IsGoogleLoading` (bool) observable property to `LoginViewModel`
- [x] 4.2 Add `OAuthLoginWithGoogleCommand` relay command to `LoginViewModel` that: opens `WebAuthenticator.AuthenticateAsync` with Google OAuth URL, extracts `id_token` from result, calls `authService.OAuthLoginAsync("google", idToken, null)`, navigates home on success, handles cancellation silently, shows toast on other errors

## 5. UI

- [x] 5.1 Add "OR" separator (`OAuthOrSeparator`) between Login button and Create Account link in `LoginPage.xaml`
- [x] 5.2 Add "Sign in with Google" button below the separator, bound to `OAuthLoginWithGoogleCommand`, disabled when `IsGoogleLoading`

## 6. Verification

- [x] 6.1 Run `dotnet build` with 0 errors, 0 warnings
- [x] 6.2 Mark task 12.4 in `sync-api-endpoints/tasks.md` as complete
