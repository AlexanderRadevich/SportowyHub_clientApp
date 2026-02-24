## 1. API models and JSON serialization

- [x] 1.1 Create `Models/Api/` directory and add `LoginRequest.cs`, `LoginResponse.cs` (with nested `UserInfo` class), `RegisterRequest.cs`, `RegisterResponse.cs`, `ApiError.cs` (with nested `ErrorDetail` class) as records with `System.Text.Json` attributes
- [x] 1.2 Create `Models/Api/AuthResult.cs` as a generic record with `IsSuccess`, `Data`, `ErrorMessage`, `FieldErrors` properties and static `Success`/`Failure` factory methods
- [x] 1.3 Create `Services/Api/SportowyHubJsonContext.cs` source-generated JSON serializer context with `[JsonSerializable]` for all API model types, using `PropertyNameCaseInsensitive = true` and `SnakeCaseLower` naming policy

## 2. HTTP client infrastructure

- [x] 2.1 Create `Services/Api/ApiConfig.cs` static class with `BaseUrl` property reading from `Preferences.Get("api_base_url", defaultValue)` with the ngrok URL as compile-time default
- [x] 2.2 Create `Services/Exceptions/ServiceAuthenticationException.cs` extending `Exception` with a `Content` string property
- [x] 2.3 Create `Services/Api/IRequestProvider.cs` interface with `GetAsync<TResult>`, `PostAsync<TRequest, TResponse>`, `PutAsync<TResult>`, `DeleteAsync` methods, all accepting optional `string token` parameter
- [x] 2.4 Create `Services/Api/RequestProvider.cs` implementing `IRequestProvider` using a shared `HttpClient`, with Bearer token injection per-call, JSON serialization via `SportowyHubJsonContext`, and centralized `HandleResponse` that throws `ServiceAuthenticationException` for 401/403 and `HttpRequestException` for other non-success codes

## 3. Auth service

- [x] 3.1 Create `Services/Auth/IAuthService.cs` interface with `LoginAsync`, `RegisterAsync`, `GetTokenAsync`, `GetCurrentUserAsync`, `IsLoggedInAsync`, `ClearAuthAsync` methods
- [x] 3.2 Create `Services/Auth/AuthService.cs` implementing `IAuthService`: `LoginAsync` POSTs to `/api/v1/login`, stores JWT in `SecureStorage["auth_token"]` and user JSON in `SecureStorage["auth_user"]`, returns `AuthResult<LoginResponse>`; catches HTTP exceptions and parses `ApiError` from response body
- [x] 3.3 Implement `RegisterAsync` in `AuthService`: POSTs to `/api/v1/register`, returns `AuthResult<RegisterResponse>`; handles 409 (email taken) and 422 (validation) errors by parsing `ApiError` response
- [x] 3.4 Implement `GetTokenAsync`, `GetCurrentUserAsync`, `IsLoggedInAsync`, `ClearAuthAsync` in `AuthService` using `SecureStorage` reads/writes

## 4. DI registration

- [x] 4.1 Register `HttpClient` as singleton, `IRequestProvider` as singleton (`RequestProvider`), and `IAuthService` as singleton (`AuthService`) in `MauiProgram.cs`

## 5. ViewModel integration

- [x] 5.1 Update `LoginViewModel`: add `IAuthService` constructor parameter, add `IsLoading`, `LoginError`, `EmailError` observable properties, add email validation matching RegisterViewModel pattern, update `LoginCommand` to call `IAuthService.LoginAsync` with loading state and error handling, navigate to `//` on success
- [x] 5.2 Update `RegisterViewModel`: add `IAuthService` constructor parameter, add `IsLoading`, `RegisterError` observable properties, update `CreateAccountCommand` to call `IAuthService.RegisterAsync` then `LoginAsync` on success with loading state and error handling, navigate to `//` on success, display server errors in `EmailError` or `RegisterError`

## 6. Auth screen UI updates

- [x] 6.1 Update `LoginPage.xaml`: add email validation error label below email field, add general error label above Login button bound to `LoginError`, add `ActivityIndicator` bound to `IsLoading`, bind Login button `IsEnabled` to negated `IsLoading`
- [x] 6.2 Update `RegisterPage.xaml`: add general error label above Create Account button bound to `RegisterError`, add `ActivityIndicator` bound to `IsLoading`, bind Create Account button `IsEnabled` to include negated `IsLoading`

## 7. Startup auth check

- [x] 7.1 Update `App.xaml.cs` to resolve `IAuthService` on startup, check `IsLoggedInAsync`, and navigate to Login page if not logged in or to main shell if logged in

## 8. Build verification

- [x] 8.1 Run `dotnet build` on `SportowyHub.App` and confirm it compiles with no errors
