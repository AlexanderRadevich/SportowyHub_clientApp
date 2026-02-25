## Context

The MAUI client app currently has a basic auth flow: register → auto-login → home, and login → home. The backend API has diverged significantly from what the client expects:

- **Login response** returns `access_token`, `expires_in`, `token_type`, and optionally `refresh_token` — the client currently expects `token` and `user` fields
- **Registration** requires a `password_confirm` field the client doesn't send
- **Email verification** is mandatory — unverified users get 403 `EMAIL_NOT_VERIFIED` on login
- **Refresh tokens** are available for mobile clients via `X-Include-Refresh-Token: true` header

Current architecture: `RequestProvider` handles HTTP, throws `ServiceAuthenticationException` for 401/403 and `HttpRequestException` for others. `AuthService` wraps calls in `AuthResult<T>`. ViewModels consume `IAuthService`. Shell routes: `login`, `register`.

## Goals / Non-Goals

**Goals:**
- Fix login response model to match actual backend fields (`access_token`, `expires_in`, `token_type`)
- Send `password_confirm` in registration requests
- Add email verification screen after registration with resend functionality
- Handle 403 `EMAIL_NOT_VERIFIED` on login by navigating to verification screen
- Handle 403 `USER_BANNED` with appropriate error message
- Store and use refresh tokens for mobile session management
- Add `ResendVerificationAsync` and `RefreshTokenAsync` to auth service

**Non-Goals:**
- OAuth/social login (backend supports it, but out of scope)
- Forgot password flow (already a non-functional placeholder in the app)
- Automatic token refresh middleware/interceptor (store and expose tokens; interceptor is a separate change)
- Configurable password policy (keep current hardcoded thresholds; align later)
- Verify-email deep link handling (user verifies in browser, then comes back to app login)

## Decisions

### 1. LoginResponse field mapping

**Decision:** Rename the `LoginResponse` record fields to match backend: `AccessToken`, `ExpiresIn`, `TokenType`, `RefreshToken`. Remove the `User` sub-object — the backend login endpoint does not return user info.

**Rationale:** The JSON source generator with `SnakeCaseLower` policy maps `AccessToken` → `access_token` automatically. No custom `JsonPropertyName` needed.

**User info after login:** Since login no longer returns user info, remove the `auth_user` SecureStorage write from `LoginAsync`. `GetCurrentUserAsync` will need a future `/api/v1/me` call or can be deferred. For now, store only the token and mark the user as logged in.

**Alternative considered:** Keeping a `User` property and allowing nulls — rejected because it would mean carrying dead code and a model that doesn't match the API.

### 2. RegisterRequest gains PasswordConfirm

**Decision:** Add `PasswordConfirm` (string) to `RegisterRequest`. The ViewModel already has `ConfirmPassword` — pass it through to the service layer.

**Rationale:** Backend requires `password_confirm` field and validates it matches `password`. Sending it from the client avoids a guaranteed 422 rejection.

**Service signature change:** `RegisterAsync(string email, string password, string passwordConfirm, string? phone = null)` — add `passwordConfirm` parameter before the optional `phone`.

### 3. Email verification screen after registration

**Decision:** Create `EmailVerificationPage` + `EmailVerificationViewModel`. After successful registration, navigate to `//email-verification?email={email}` instead of auto-logging in. The screen shows:
- An icon/message telling the user to check their email
- The email address they registered with
- A "Resend Verification Email" button with cooldown
- A "Back to Login" button

**Rationale:** The backend mandates email verification before login. Auto-login would always fail with `EMAIL_NOT_VERIFIED`. Better UX to guide the user to check their email.

**Navigation:** Register route as `email-verification` in `AppShell.xaml.cs`. Use query parameter to pass the email address.

### 4. Handling 403 EMAIL_NOT_VERIFIED on login

**Decision:** In `AuthService.LoginAsync`, when parsing a 403 `ServiceAuthenticationException`, check the error code. If `EMAIL_NOT_VERIFIED`, return `AuthResult.Failure` with the error code embedded. `LoginViewModel` checks for this code and navigates to the email verification screen (passing the email).

**Rationale:** Keeps the service layer returning data, and the ViewModel decides on navigation. This follows the existing MVVM pattern.

**Error code propagation:** Add an `ErrorCode` property to `AuthResult<T>` so callers can distinguish error types without string-matching the message.

### 5. AuthResult gains ErrorCode

**Decision:** Add `ErrorCode` (string?, nullable) to `AuthResult<T>` and the `Failure` factory method. Populate it from `ApiError.Error.Code` when available.

**Rationale:** Enables ViewModels to react to specific error conditions (EMAIL_NOT_VERIFIED, USER_BANNED) without parsing localized error messages.

### 6. Refresh token storage and retrieval

**Decision:** When login succeeds, store `access_token` under `auth_token`, `refresh_token` under `auth_refresh_token` (if present), and `expires_in` under `auth_token_expiry` in SecureStorage. Add `RefreshTokenAsync` to `IAuthService` that POSTs to `/api/v1/refresh` with `X-Include-Refresh-Token: true` header and the stored refresh token as Bearer auth.

**Rationale:** Mobile clients need refresh tokens for silent re-auth. The `X-Include-Refresh-Token: true` header tells the backend to return a new refresh token in the response body instead of an httpOnly cookie.

**RequestProvider change:** Add an overload or parameter to `PostAsync` that accepts custom headers, since the refresh endpoint needs both `Authorization: Bearer {refresh_token}` and `X-Include-Refresh-Token: true`.

### 7. ResendVerificationAsync

**Decision:** Add `ResendVerificationAsync(string email)` to `IAuthService`. POSTs to `/api/v1/resend-verification` with `{"email": "..."}`. Returns `AuthResult<ResendVerificationResponse>`.

**Rationale:** The email verification screen needs this to let users request a new verification email.

### 8. HandleResponse needs status code propagation

**Decision:** Modify `ServiceAuthenticationException` to carry the `HttpStatusCode` alongside the response content. This lets `AuthService` distinguish 401 (bad credentials) from 403 (EMAIL_NOT_VERIFIED or USER_BANNED).

**Alternative considered:** Parsing the error code from the JSON body for all 4xx responses — but 401 and 403 have different semantic meanings that the status code already conveys. Keeping both the code and the body is more robust.

## Risks / Trade-offs

- **Breaking LoginResponse model** → All code referencing `response.Token` and `response.User` must be updated. Mitigation: limited blast radius — only `AuthService.LoginAsync` and tests reference it.
- **Removing user info from login** → `GetCurrentUserAsync` returns null after login (no user object stored). Mitigation: profile screen already handles null; add a `/api/v1/me` endpoint call later or remove `GetCurrentUserAsync` for now.
- **Refresh token flow adds complexity** → More SecureStorage keys, new endpoint, header management. Mitigation: implement as a separate capability so it can be tested independently.
- **Email verification screen is a dead-end** → User must leave the app to verify email, then come back. Mitigation: clear instructions on the screen, resend button, "Back to Login" button. Deep-link handling is a non-goal for now.

## Open Questions

- Should we remove `GetCurrentUserAsync` and `UserInfo` model entirely now that login doesn't return user data, or keep a stub for future `/api/v1/me` support? → **Decision: Keep the interface method but have it return null until we add a `/me` endpoint call. Remove the SecureStorage write from LoginAsync.**
