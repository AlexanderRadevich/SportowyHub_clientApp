# Add Token Refresh Handler

## Why

`AuthService.RefreshTokenAsync` exists but is never called. All authenticated services call `GetTokenAsync()` which returns stale tokens. When the API returns 401, `HandleResponse` throws `ServiceAuthenticationException` unhandled, causing user-facing crashes instead of transparent token refresh.

## What Changes

Add an `AuthenticatingDelegatingHandler` in the HttpClient pipeline that intercepts 401 responses, calls `RefreshTokenAsync`, and retries the original request. If refresh also fails, redirect the user to the login page.

## Capabilities

### New
- `AuthenticatingDelegatingHandler` — DelegatingHandler that intercepts 401 responses and transparently refreshes tokens
- Token expiry pre-check to avoid unnecessary 401 round-trips
- Redirect-to-login fallback when refresh fails

### Modified
- `MauiProgram.cs` — register the handler in the HttpClient pipeline
- `AuthService.cs` — expose token expiry information for pre-check logic

## Impact

- **Users** — no more crashes on expired tokens; seamless session continuation
- **Security** — tokens refreshed proactively; stale tokens no longer sent to API
- **Architecture** — centralized auth handling instead of scattered token checks
