## Why

The auth UI screens (Login, Register) are fully built with client-side validation but contain no backend integration — all commands just navigate back. The SportowyHub API is ready with `POST /api/v1/register` and `POST /api/v1/login` endpoints returning JWT tokens. Connecting the app to the API is the critical next step to enable real user accounts.

## What Changes

- Add an HTTP service layer inspired by eShop's `IRequestProvider` pattern: a generic HTTP client abstraction with centralized error handling, Bearer token injection, and source-generated JSON serialization
- Add an `IAuthService` with `RegisterAsync` and `LoginAsync` methods calling the SportowyHub API
- Add token persistence via `SecureStorage` (JWT token + user info)
- Add API request/response models matching the OpenAPI spec (`LoginRequest`, `LoginResponse`, `RegisterRequest`, `RegisterResponse`, `ApiError`, `ValidationError`)
- Wire `LoginViewModel` and `RegisterViewModel` to call the auth service instead of just navigating back
- Add loading states (`IsBusy`) and API error display (server validation errors, 409 conflict, 401 unauthorized, network errors) to both auth screens
- Register all new services in `MauiProgram.cs` DI container
- Add an auth state check on app startup to skip login if a valid token exists

## Capabilities

### New Capabilities
- `http-client-infrastructure`: Generic HTTP request provider (`IRequestProvider` / `RequestProvider`) with Bearer token support, centralized error handling, and source-generated JSON serialization context
- `auth-service`: `IAuthService` with register/login methods, JWT token storage via `SecureStorage`, and auth state management (check if user is logged in, get current token)
- `auth-api-models`: Request/response DTOs for the auth API endpoints (`LoginRequest`, `LoginResponse`, `RegisterRequest`, `RegisterResponse`, `ApiError`, `ValidationError`)

### Modified Capabilities
- `auth-screens`: Login and Register ViewModels gain backend integration — commands call `IAuthService`, display loading spinners during API calls, show server-side error messages (email taken, invalid credentials, validation errors), and navigate on success. Login page adds an email validation error label matching Register page pattern.

## Impact

- **New files**: Services (`IRequestProvider`, `RequestProvider`, `IAuthService`, `AuthService`), Models (`LoginRequest`, `LoginResponse`, `RegisterRequest`, `RegisterResponse`, `ApiError`), JSON serializer context
- **Modified files**: `LoginViewModel.cs`, `RegisterViewModel.cs` (add API calls, loading state, error handling), `MauiProgram.cs` (DI registration for HttpClient, services), `App.xaml.cs` (startup auth check)
- **Dependencies**: No new NuGet packages needed — `System.Net.Http` and `System.Text.Json` are included in .NET 10, `SecureStorage` is included in MAUI Essentials
- **API dependency**: Requires the SportowyHub backend at a configurable base URL
