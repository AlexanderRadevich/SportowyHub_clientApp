## Why

The current client auth implementation has critical mismatches with the actual backend API. The login response model uses the wrong field names (`token` instead of `access_token`), registration doesn't send the required `password_confirm` field, and the entire email verification flow is missing. After registration, the backend requires email verification before login is allowed (returns 403 `EMAIL_NOT_VERIFIED`), but the client currently tries to auto-login immediately. Additionally, the backend supports refresh tokens for mobile clients via `X-Include-Refresh-Token` header, and a resend-verification endpoint — none of which are implemented.

## What Changes

- **BREAKING**: Remap `LoginResponse` fields to match backend: `access_token` (not `token`), add `expires_in`, `token_type`, optional `refresh_token`
- **BREAKING**: Add `PasswordConfirm` to `RegisterRequest` — backend requires `password_confirm` field
- **BREAKING**: Remove auto-login after registration; navigate to new "Check Your Email" screen instead
- Add `ResendVerificationRequest`/`ResendVerificationResponse` models for `/api/v1/resend-verification`
- Add `RefreshTokenResponse` model for `/api/v1/refresh`
- Add new `EmailVerificationPage` + `EmailVerificationViewModel` shown after successful registration
- Handle 403 `EMAIL_NOT_VERIFIED` on login — show email verification screen with resend option
- Handle 403 `USER_BANNED` on login — show appropriate error
- Add `ResendVerificationAsync` method to `IAuthService`
- Add `RefreshTokenAsync` method to `IAuthService` with `X-Include-Refresh-Token` header support
- Store refresh token in SecureStorage alongside access token
- Add localization strings for email verification UI (4 locales: pl, en, uk, ru)

## Capabilities

### New Capabilities
- `email-verification`: Email verification screen shown after registration and when login fails with EMAIL_NOT_VERIFIED. Includes resend verification functionality.
- `token-refresh`: Refresh token storage and automatic token refresh via `/api/v1/refresh` with `X-Include-Refresh-Token` header for mobile clients.

### Modified Capabilities
- `auth-api-models`: LoginResponse fields renamed to match backend (`access_token`, `expires_in`, `token_type`, `refresh_token`). RegisterRequest gains `PasswordConfirm`. New request/response models for resend-verification and refresh endpoints.
- `auth-service`: LoginAsync must handle 403 EMAIL_NOT_VERIFIED and USER_BANNED. RegisterAsync must send `password_confirm`. New methods: `ResendVerificationAsync`, `RefreshTokenAsync`. Token storage updated for access + refresh tokens.
- `auth-screens`: Registration no longer auto-logs in — navigates to email verification screen. Login handles EMAIL_NOT_VERIFIED by navigating to verification screen. New email verification page and navigation routes.

## Impact

- **Models**: `LoginResponse`, `RegisterRequest` — breaking field changes
- **Services**: `IAuthService`, `AuthService`, `RequestProvider` (needs header support for refresh)
- **ViewModels**: `RegisterViewModel` (remove auto-login, navigate to verification), `LoginViewModel` (handle 403 EMAIL_NOT_VERIFIED), new `EmailVerificationViewModel`
- **Views**: New `EmailVerificationPage.xaml`, route registration in `AppShell.xaml`
- **Resources**: New localization keys across 4 `.resx` files + `AppResources.Designer.cs`
- **DI**: Register new ViewModel and page
- **SecureStorage**: New keys for refresh token and token expiry
