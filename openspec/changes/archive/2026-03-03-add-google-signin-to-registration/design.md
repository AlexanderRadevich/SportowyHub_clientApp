## Context

The login page already has a fully working Google sign-in flow: OR separator → "Sign in with Google" button → PKCE code exchange via `IAuthService.AcquireGoogleIdTokenAsync` → `IAuthService.OAuthLoginAsync("google", idToken, null)`. The backend `OAuthLoginAsync` endpoint returns 201 for new accounts, so it functions as both login and registration. The registration page currently only offers manual account creation.

## Goals / Non-Goals

**Goals:**
- Add Google sign-in option to the registration page with identical UX to the login page
- Reuse existing `IAuthService` OAuth methods without modification
- Maintain independent loading states for form submission vs Google sign-in

**Non-Goals:**
- Extracting a shared OAuth base class or helper — the command is ~15 lines and duplication is acceptable for two view models
- Adding other OAuth providers (Apple, Facebook) — out of scope
- Changing the post-OAuth navigation flow (always goes to `//home`)

## Decisions

### 1. Duplicate the command pattern rather than extract a shared base

The `OAuthLoginWithGoogleCommand` in `LoginViewModel` is straightforward: set loading → acquire token → call API → navigate. Extracting this to a base class or helper would add indirection for minimal benefit, especially since the two view models have different error property names (`LoginError` vs `RegisterError`) and different overall responsibilities. Copy the pattern into `RegisterViewModel`.

**Alternative considered:** Create an `OAuthLoginMixin` or base `AuthViewModelBase`. Rejected because the shared surface is small and the view models are otherwise unrelated in their fields and validation logic.

### 2. Place Google sign-in after the "Create Account" button

Match the login page layout: primary action button → OR separator → Google button. This keeps the manual form as the primary path and offers Google as a secondary option.

### 3. Independent `IsGoogleLoading` property

Same pattern as `LoginViewModel`: a separate `IsGoogleLoading` bool that only affects the Google button and its `ActivityIndicator`. The form fields and "Create Account" button remain interactive while Google auth is in progress (and vice versa — `IsLoading` doesn't disable the Google button).

### 4. Reuse existing localization keys

`OAuthOrSeparator` ("OR") and `OAuthSignInGoogle` ("Sign in with Google") already exist in all 4 locale .resx files. No new strings needed — the button text is the same on both pages.

### 5. Post-OAuth navigation: go to `//home`

When `OAuthLoginAsync` succeeds (200 existing account or 201 new account), navigate the same way as `LoginViewModel`: `GoToAsync("..")` then `GoToAsync("//home")`. The backend handles account creation transparently.

## Risks / Trade-offs

- **Duplicated OAuth command logic** → Acceptable for 2 occurrences. If a third screen needs it, extract at that point.
- **User confusion: "Sign in" on registration page** → The button text says "Sign in with Google" which is standard UX. Google's own guidelines use "Sign in with Google" for both login and signup flows.
- **Registration page gets longer** → Adds ~25 lines of XAML (separator + button + indicator). The page is already scrollable so no layout concern.
