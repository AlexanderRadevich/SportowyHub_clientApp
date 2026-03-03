## Context

The service layer for OAuth login is fully implemented (`IAuthService.OAuthLoginAsync`, `OAuthLoginRequest` model, JSON context registration). The backend accepts `POST /api/v1/auth/oauth/{provider}` with `id_token` or `access_token`. What's missing is the UI trigger and platform configuration for `WebAuthenticator`.

## Goals / Non-Goals

**Goals:**
- Let users sign in with Google from the LoginPage
- Use MAUI `WebAuthenticator` for the OAuth flow
- Configure Android and iOS URL schemes for the callback
- Handle cancellation, errors, and loading state

**Non-Goals:**
- Facebook or Apple OAuth (future changes)
- Custom Google Sign-In SDK integration (WebAuthenticator is sufficient)
- Server-side Google client ID management (already configured on backend)

## Decisions

**Decision 1: Use `WebAuthenticator.AuthenticateAsync` (not Google Sign-In SDK)**
- Rationale: MAUI's built-in `WebAuthenticator` opens the system browser for OAuth, captures the callback, and requires no native SDK dependencies. The backend already handles token verification server-side.
- Trade-off: Slightly less polished UX than native Google Sign-In button, but much simpler to maintain cross-platform.

**Decision 2: Google OAuth URL with `response_type=id_token`**
- Rationale: The backend's `OAuthLoginController` prefers `id_token` for Google. Using the implicit flow with `response_type=id_token` gets the token directly in the callback fragment without an extra server-side code exchange.
- The Google OAuth URL will be: `https://accounts.google.com/o/oauth2/v2/auth` with params: `client_id`, `redirect_uri`, `response_type=id_token`, `scope=openid email profile`, `nonce`.

**Decision 3: Google Client ID from app configuration, not hardcoded**
- Rationale: The client ID varies per environment (dev/staging/prod). Store it alongside `ApiConfig.BaseUrl` in the existing `ApiConfig` class so it can be swapped without code changes.
- File: `SportowyHub.App/Services/Api/ApiConfig.cs`

**Decision 4: Place OAuth button below the Login button with an "OR" divider**
- Rationale: Primary action remains email/password login. Google sign-in is a secondary alternative. The "OR" visual separator is a standard pattern in login forms.

**Decision 5: Separate `IsGoogleLoading` state from `IsLoading`**
- Rationale: The user could cancel the WebAuthenticator flow (browser dismissal). Using a separate loading flag prevents the email/password form from being disabled during the OAuth flow, and vice versa.

## Risks / Trade-offs

- [Risk] `WebAuthenticator` requires HTTPS callback URL on Android 12+ — Mitigation: Use the app's custom URL scheme (`sportowyHub://`) registered as an intent filter, which works on all Android versions.
- [Risk] Google Cloud Console must have the correct redirect URI registered — Mitigation: Document the required URI format in the design so backend team can configure it.
- [Risk] Token could expire between WebAuthenticator callback and API call — Mitigation: The `id_token` has a 1-hour expiry; the round-trip is milliseconds. Negligible risk.
- [Trade-off] No offline account creation — Google OAuth always requires network. This is acceptable for a login flow.
