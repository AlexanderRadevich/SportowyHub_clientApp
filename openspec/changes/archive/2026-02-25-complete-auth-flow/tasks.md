## 1. API Models

- [x] 1.1 Update `LoginResponse` record: replace `Token` (string) and `User` (UserInfo) with `AccessToken` (string), `ExpiresIn` (int), `TokenType` (string), `RefreshToken` (string?, nullable), keep `Locale` (string?, nullable)
- [x] 1.2 Update `RegisterRequest` record: add `PasswordConfirm` (string) parameter between `Password` and `Phone`
- [x] 1.3 Create `ResendVerificationRequest` record with `Email` (string) in `Models/Api/`
- [x] 1.4 Create `ResendVerificationResponse` record with `Message` (string) and `Locale` (string?, nullable) in `Models/Api/`
- [x] 1.5 Add `ErrorCode` (string?, nullable) property to `AuthResult<T>`, update `Failure` factory to accept optional `errorCode` parameter, update `Success` factory to set `ErrorCode = null`
- [x] 1.6 Register `ResendVerificationRequest` and `ResendVerificationResponse` in `SportowyHubJsonContext`

## 2. Infrastructure

- [x] 2.1 Add `HttpStatusCode StatusCode` property to `ServiceAuthenticationException`, pass status code from `HandleResponse`
- [x] 2.2 Add optional `Dictionary<string, string>? headers` parameter to `RequestProvider.PostAsync<TRequest, TResponse>` and apply headers to request
- [x] 2.3 Update `IRequestProvider` interface to match new `PostAsync` signature

## 3. Auth Service

- [x] 3.1 Update `IAuthService`: change `RegisterAsync` signature to `(string email, string password, string passwordConfirm, string? phone = null)`, add `ResendVerificationAsync(string email)` and `RefreshTokenAsync()` methods
- [x] 3.2 Update `AuthService.LoginAsync`: use `response.AccessToken` instead of `response.Token`, remove `response.User` SecureStorage write, store refresh token and expiry when present, send `X-Include-Refresh-Token: true` header, populate `ErrorCode` from parsed `ApiError.Error.Code`
- [x] 3.3 Update `AuthService.RegisterAsync`: accept `passwordConfirm` parameter, pass it to `RegisterRequest`, populate `ErrorCode` from parsed error
- [x] 3.4 Implement `AuthService.ResendVerificationAsync`: POST to `/api/v1/resend-verification` with `ResendVerificationRequest`, return `AuthResult<ResendVerificationResponse>`
- [x] 3.5 Implement `AuthService.RefreshTokenAsync`: read refresh token from SecureStorage, POST to `/api/v1/refresh` with Bearer auth and `X-Include-Refresh-Token: true` header, store new tokens on success, clear auth on failure
- [x] 3.6 Update `AuthService.ClearAuthAsync`: also remove `auth_refresh_token` and `auth_token_expiry` from SecureStorage
- [x] 3.7 Update `ParseErrorWithFields` to return error code as a third tuple element from `ApiError.Error.Code`

## 4. Email Verification Screen

- [x] 4.1 Add localization strings to all 4 `.resx` files and `AppResources.Designer.cs`: `EmailVerificationTitle`, `EmailVerificationDescription`, `EmailVerificationResend`, `EmailVerificationResent`, `EmailVerificationBackToLogin`, `EmailVerificationError`
- [x] 4.2 Create `EmailVerificationViewModel` with: `Email`, `IsLoading`, `StatusMessage`, `IsStatusError`, `CooldownSeconds` observable properties; `ResendCommand` (disabled during loading/cooldown); `BackToLoginCommand`; `IQueryAttributable` for email param; 60-second cooldown timer after resend
- [x] 4.3 Create `EmailVerificationPage.xaml` and code-behind: mail icon, title, description with email, resend button, status message, back to login link
- [x] 4.4 Register `email-verification` route in `AppShell.xaml.cs`
- [x] 4.5 Register `EmailVerificationPage` and `EmailVerificationViewModel` as transient in `MauiProgram.cs`

## 5. ViewModel Updates

- [x] 5.1 Update `RegisterViewModel.CreateAccount`: pass `ConfirmPassword` to `RegisterAsync`, on success navigate to `email-verification?email={Email}` instead of auto-login
- [x] 5.2 Update `LoginViewModel.Login`: check `result.ErrorCode` for `EMAIL_NOT_VERIFIED` → navigate to `email-verification?email={Email}`, check for `USER_BANNED` → show in `LoginError`

## 6. Build Verification

- [x] 6.1 Build the solution and fix any compilation errors
