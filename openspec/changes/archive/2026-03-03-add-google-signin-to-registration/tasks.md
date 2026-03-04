## 1. ViewModel Changes

- [x] 1.1 Add `IsGoogleLoading` observable property to `RegisterViewModel` (bool, default false)
- [x] 1.2 Add `OAuthLoginWithGoogleCommand` relay command to `RegisterViewModel` — call `AcquireGoogleIdTokenAsync`, then `OAuthLoginAsync("google", idToken, null)`, navigate to `//home` on success, silently catch `TaskCanceledException`, show error toast on other failures, clear `RegisterError` before starting, set `IsGoogleLoading` during flow

## 2. XAML Changes

- [x] 2.1 Add OR separator to `RegisterPage.xaml` below the Create Account button — three-column Grid with two `BoxView` lines and `OAuthOrSeparator` label, matching `LoginPage.xaml` layout
- [x] 2.2 Add "Sign in with Google" button to `RegisterPage.xaml` — `SecondaryButton` style, `OAuthSignInGoogle` text, `AutomationId="RegisterGoogleSignInButton"`, bound to `OAuthLoginWithGoogleCommand`, disabled when `IsGoogleLoading`
- [x] 2.3 Add `ActivityIndicator` below Google button bound to `IsGoogleLoading`

## 3. Verification

- [x] 3.1 Run `dotnet build` and fix any compilation errors
- [x] 3.2 Run `dotnet test` and verify all existing tests pass
- [x] 3.3 Verify on device/emulator: registration page shows OR separator and Google button, tapping Google button opens browser OAuth flow, successful OAuth navigates to home
