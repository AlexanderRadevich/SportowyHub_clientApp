# Design: Add Token Refresh Handler

## Context

The app has `AuthService.RefreshTokenAsync` but nothing calls it. Authenticated API calls use stale tokens, resulting in unhandled `ServiceAuthenticationException` crashes when the API returns 401.

## Goals / Non-Goals

### Goals
- Transparently refresh expired tokens without user interaction
- Retry the original request after successful refresh
- Redirect to login when refresh fails
- Pre-check token expiry to avoid unnecessary 401 round-trips

### Non-Goals
- Implement token caching beyond current SecureStorage approach
- Handle concurrent refresh requests with queuing (future improvement)

## Decisions

1. **DelegatingHandler approach** — Use `AuthenticatingDelegatingHandler` in the HttpClient pipeline. This centralizes auth logic and keeps it out of individual services.
2. **Retry once** — On 401, attempt one refresh + retry. If the retry also fails, redirect to login. No infinite retry loops.
3. **Expiry pre-check** — Before sending a request, check if the stored token is within a short window of expiry. If so, refresh proactively to avoid the 401 round-trip.
4. **SemaphoreSlim for concurrent requests** — Use a semaphore to prevent multiple simultaneous refresh calls when several requests hit 401 at the same time.

## Risks / Trade-offs

- **Circular dependency risk** — The handler depends on `AuthService` which may depend on `HttpClient`. Must ensure the auth refresh call uses a separate HttpClient instance (or raw handler) to avoid circular pipeline invocation.
- **Race condition** — Multiple concurrent 401s could trigger parallel refresh attempts. The semaphore mitigates this but adds complexity.
- **Token storage latency** — Reading/writing tokens to SecureStorage is async and may add latency to each request pre-check.
