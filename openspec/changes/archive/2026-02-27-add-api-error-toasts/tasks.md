## 1. Toast Service

- [x] 1.1 Create `Services/Toast/IToastService.cs` with `Task ShowError(string message)` method
- [x] 1.2 Create `Services/Toast/ToastService.cs` — implementation using `CommunityToolkit.Maui.Alerts.Snackbar` with 6-second duration, no action button, dispatched to main thread
- [x] 1.3 Register `IToastService` → `ToastService` as singleton in `MauiProgram.cs`

## 2. ViewModel Integration — Auth Screens

- [x] 2.1 Inject `IToastService` into `LoginViewModel` constructor; in `Login()` catch-all, replace silent swallow with `await _toastService.ShowError(ex.Message)`
- [x] 2.2 Inject `IToastService` into `RegisterViewModel` constructor; in `CreateAccount()` catch-all, replace silent swallow with `await _toastService.ShowError(ex.Message)`
- [x] 2.3 Inject `IToastService` into `EmailVerificationViewModel` constructor; in `Resend()` catch-all, replace silent swallow with `await _toastService.ShowError(ex.Message)`

## 3. ViewModel Integration — Profile Screens

- [x] 3.1 Inject `IToastService` into `AccountProfileViewModel` constructor; in `LoadProfile()`, wrap `GetProfileAsync()` call in try-catch and show toast on exception (keep `HasError = true`)
- [x] 3.2 In `AccountProfileViewModel.SignOut()`, wrap `LogoutAsync()` call in try-catch and show toast on exception (still proceed with local sign-out)
- [x] 3.3 Inject `IToastService` into `ProfileViewModel` constructor; in `SignOut()`, wrap `LogoutAsync()` call in try-catch and show toast on exception (still proceed with local sign-out)

## 4. Build Verification

- [x] 4.1 Verify project builds with 0 errors and 0 warnings
