## Context

After a successful `POST /api/v1/register`, the backend returns a `RegisterResponse` with a `TrustLevel` field. Currently `RegisterViewModel.CreateAccount()` unconditionally navigates to the `email-verification` page (line 177). When the backend returns `TL0` (no verification required), the user lands on a verification page that serves no purpose.

`RegisterResponse` already contains the `TrustLevel` string — no model changes needed.

## Goals / Non-Goals

**Goals:**
- Branch post-registration navigation based on `TrustLevel`
- Show a localized toast/alert on successful unverified registration before navigating to login

**Non-Goals:**
- Changing the backend trust level logic
- Handling other trust levels beyond TL0 vs needs-verification
- Persisting registration state or auto-login after unverified registration

## Decisions

### 1. Navigation branching in RegisterViewModel

Check `registerResult.Data.TrustLevel` after success. If `"TL0"`, display a success alert and navigate to login. Otherwise, navigate to email verification as before.

**Rationale**: Keeps the logic in one place (`CreateAccount` method) with a simple string comparison. No new services or abstractions needed for a single conditional branch.

### 2. Use `Shell.Current.DisplayAlert` for the success message

Show a native alert with the localized success message before navigating to login. This ensures the user sees the confirmation before the screen changes.

**Alternative considered**: Toast via CommunityToolkit — dismissed too quickly for an important message. An inline success label on the registration page would require preventing navigation, adding unnecessary complexity.

### 3. Navigate to login via `..` (pop to parent)

Use `await Shell.Current.GoToAsync("..")` to return to the login page (registration is pushed on top of login in the navigation stack).

**Rationale**: The register page is navigated to from login, so popping back is the natural path. No need for absolute routes.

## Risks / Trade-offs

- **String comparison on TrustLevel** — If the backend introduces new trust levels, the current approach defaults to email verification (safe fallback). Only `TL0` explicitly skips verification.
- **Alert blocks navigation** — `DisplayAlert` is awaited, so the user must tap OK before navigating. This is intentional to ensure they see the message.
