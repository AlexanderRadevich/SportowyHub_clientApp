## Context

SportowyHub is a .NET MAUI app with polished auth UI screens (Login, Register) backed by ViewModels with client-side validation. Currently all auth commands are stubs that just navigate back. The backend API is ready at a configurable URL with two public endpoints: `POST /api/v1/register` (returns user object) and `POST /api/v1/login` (returns JWT token + user). The app uses CommunityToolkit.Mvvm for MVVM, has no HTTP infrastructure, and stores preferences via MAUI `Preferences`. The eShop reference app provides proven patterns for HTTP abstraction, token management, and service architecture in .NET MAUI.

## Goals / Non-Goals

**Goals:**
- Establish a reusable HTTP client layer that future API features (listings, search, profile) can build on
- Implement working registration and login flows against the real API
- Persist JWT tokens securely so the user stays logged in across app restarts
- Show meaningful error messages from the server (email taken, invalid credentials, validation errors)
- Show loading states during API calls

**Non-Goals:**
- Token refresh (API currently returns a single JWT with no refresh token endpoint)
- Logout (no server-side logout endpoint in the public API — defer to a later change)
- Profile screen changes after login (showing user info, logged-in state)
- Password reset / forgot password flow
- Offline mode or request caching
- Mock service implementations (eShop pattern) — not needed yet with a live backend

## Decisions

### 1. IRequestProvider generic HTTP abstraction (eShop pattern)

Adopt the eShop `IRequestProvider` pattern: a single service that wraps `HttpClient` with generic `GetAsync<T>`, `PostAsync<TRequest, TResponse>`, etc. Token is passed per-call, not baked into the client.

- Provides a clean abstraction that all future API services (listings, search) can use
- Centralizes error handling, serialization, and auth header injection in one place
- Alternative considered: registering typed `HttpClient` per service via `IHttpClientFactory` — rejected because the eShop pattern is simpler for a mobile app with one backend, and matches the reference the user specified

### 2. Source-generated JSON serialization

Use `System.Text.Json` with a `[JsonSerializable]` source-generated context for all API models.

- Required for AOT/trimming compatibility in .NET MAUI
- Better startup performance than reflection-based serialization
- This is the standard eShop pattern

### 3. SecureStorage for JWT token

Store the JWT token string in `SecureStorage.SetAsync("auth_token", token)`. Store user info (id, email, trust_level) as serialized JSON in a separate SecureStorage key.

- `SecureStorage` uses Keychain (iOS), KeyStore (Android), DPAPI (Windows) — encrypted at rest
- Alternative considered: storing in `Preferences` — rejected because tokens are sensitive credentials
- The token is a single JWT string (no refresh token from this API), so storage is simple

### 4. IAuthService as a focused auth contract

Create `IAuthService` with: `RegisterAsync`, `LoginAsync`, `GetTokenAsync`, `GetCurrentUserAsync`, `IsLoggedInAsync`, `ClearAuthAsync`.

- Keeps auth logic separate from the generic HTTP layer
- ViewModels depend on `IAuthService`, not on `IRequestProvider` directly
- `GetTokenAsync` will be called by future services that need authenticated requests
- Alternative considered: putting auth methods directly on `IRequestProvider` — rejected to maintain single responsibility

### 5. Error handling: two-tier exception model (eShop pattern)

`RequestProvider` throws:
- `ServiceAuthenticationException` for 401/403 responses
- `HttpRequestException` for other HTTP failures

`AuthService` catches these and returns result objects rather than throwing — ViewModels receive structured results with success/failure and error messages, not exceptions.

- ViewModels don't need try/catch blocks — they check `result.IsSuccess`
- Server validation errors (422) are parsed from the response body and surfaced as user-readable messages
- Network failures are caught and returned as a generic "connection error" message

### 6. AuthResult<T> result pattern for ViewModel consumption

Auth methods return `AuthResult<T>` with `IsSuccess`, `Data`, `ErrorMessage`, and `FieldErrors` (dictionary for per-field server validation).

- Cleaner than exceptions for expected failures (wrong password, email taken)
- `FieldErrors` maps directly to the existing error label bindings in the auth ViewModels (`EmailError`, etc.)
- Alternative considered: throwing domain exceptions — rejected because login failures are expected flow, not exceptional

### 7. Startup auth check in App.xaml.cs

On app startup, check `SecureStorage` for a stored token. If present, navigate directly to the main app shell (Home tab). If absent, show login.

- Simple token-exists check is sufficient since we don't have token refresh or expiry validation
- Navigation decision happens in `App.xaml.cs` or `AppShell` before the first page renders
- The check is fast (SecureStorage read) and doesn't block the UI

### 8. API base URL configuration

Store the API base URL in a static `ApiConfig` class with a compile-time default. Allow override via `Preferences` for development/testing.

- Simple and explicit — no need for `appsettings.json` complexity in a mobile app
- The ngrok URL will change; developers can update it without recompiling via a hidden settings tap or debug menu later

### 9. DI registration in MauiProgram.cs

Register services following eShop grouping pattern:
- `HttpClient` as singleton (one instance, connection pooling)
- `IRequestProvider` as singleton (wraps the single HttpClient)
- `IAuthService` as singleton (stateless, delegates to RequestProvider)
- ViewModels remain transient

### 10. Project structure

```
SportowyHub.App/
├── Models/
│   └── Api/
│       ├── LoginRequest.cs
│       ├── LoginResponse.cs
│       ├── RegisterRequest.cs
│       ├── RegisterResponse.cs
│       ├── ApiError.cs
│       └── AuthResult.cs
├── Services/
│   ├── Api/
│   │   ├── IRequestProvider.cs
│   │   ├── RequestProvider.cs
│   │   ├── ApiConfig.cs
│   │   └── SportowyHubJsonContext.cs
│   ├── Auth/
│   │   ├── IAuthService.cs
│   │   └── AuthService.cs
│   └── Exceptions/
│       └── ServiceAuthenticationException.cs
├── ViewModels/
│   ├── LoginViewModel.cs      (modified)
│   └── RegisterViewModel.cs   (modified)
└── MauiProgram.cs              (modified)
```

Feature-based folder structure within Services/ and Models/, consistent with the existing codebase organization.

## Risks / Trade-offs

- **No token expiry handling** → The API returns a JWT but we don't know its expiry. If the token expires, API calls will fail with 401. Mitigation: catch 401 in `RequestProvider`, clear stored auth, and redirect to login. Full token refresh can be added when the API supports it.
- **Ngrok URL instability** → The development API URL changes on ngrok restarts. Mitigation: configurable base URL via `ApiConfig` with easy override.
- **No HTTPS certificate pinning** → Mobile app communicates over HTTPS but doesn't pin certificates. Acceptable for development phase; can be added before production.
- **SecureStorage limitations on Windows** → `SecureStorage` on Windows uses DPAPI which is user-scoped but less secure than mobile equivalents. Acceptable since Windows is a secondary platform.
- **No retry/resilience** → API calls fail on first network error with no retry. Mitigation: sufficient for initial implementation; Polly can be added later if needed.
