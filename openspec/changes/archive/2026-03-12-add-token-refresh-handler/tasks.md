# Tasks: Add Token Refresh Handler

## Tasks

- [x] Create `AuthenticatingDelegatingHandler` class that intercepts 401 responses
- [x] Add token expiry pre-check to refresh proactively before sending requests
- [x] Add SemaphoreSlim to prevent concurrent refresh attempts
- [x] Register handler in HttpClient pipeline in `MauiProgram.cs`
- [x] Expose token expiry from `AuthService` for the pre-check
- [x] Add redirect-to-login fallback when refresh fails
- [x] Unit test: handler retries request after successful refresh
- [x] Unit test: handler redirects to login after failed refresh
- [x] Unit test: concurrent 401s trigger only one refresh call
