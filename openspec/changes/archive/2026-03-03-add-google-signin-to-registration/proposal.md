## Why

Users who prefer Google sign-in currently must navigate to the login page to use it. Adding Google sign-in to the registration page gives new users an immediate choice between creating a local account or using their Google account, reducing friction and matching common UX patterns where OAuth is available on both auth screens.

## What Changes

- Add an "OR" separator and "Sign in with Google" button to the registration page, matching the login page's existing layout
- Add `OAuthLoginWithGoogleCommand` and `IsGoogleLoading` property to `RegisterViewModel`, reusing the same `IAuthService.AcquireGoogleIdTokenAsync` + `OAuthLoginAsync` flow from `LoginViewModel`
- Add localization strings for the registration OAuth section (reuse existing `OAuthOrSeparator` and `OAuthSignInGoogle` keys)
- Add `GoogleSignInButton` AutomationId to the registration page for UI test targeting

## Capabilities

### New Capabilities

_(none — this reuses the existing OAuth infrastructure)_

### Modified Capabilities

- `auth-screens`: Registration page gains an OR separator and Google sign-in button with loading state, positioned between the "Create Account" button and the bottom of the form

## Impact

- **UI**: `RegisterPage.xaml` — new XAML elements (separator + button + loading indicator)
- **ViewModel**: `RegisterViewModel.cs` — new command and loading property
- **Specs**: `auth-screens` spec updated to document Google sign-in on registration
- **No backend changes** — `OAuthLoginAsync` already returns 201 for new accounts created via OAuth
- **No new dependencies** — reuses existing `IAuthService` methods
