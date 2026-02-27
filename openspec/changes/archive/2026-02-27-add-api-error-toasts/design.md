## Context

The app has 5 ViewModels that call `IAuthService` methods which can fail. Currently, auth operations (login, register, resend) use `AuthResult<T>` and show errors via observable properties bound to inline UI labels. Profile loading and logout either swallow exceptions silently or show a generic `HasError` flag with no message. There is no centralized notification mechanism. `CommunityToolkit.Maui` v13.0.0 is installed and configured (`.UseMauiCommunityToolkit()`), providing `Toast` and `Snackbar` APIs.

## Goals / Non-Goals

**Goals:**
- Show a top-positioned error toast (auto-dismiss after 6 seconds) whenever an API call fails in a ViewModel
- Provide an `IToastService` injectable via DI for consistent usage across all ViewModels
- Surface errors from currently-silent failures (`GetProfileAsync`, `LogoutAsync`)

**Non-Goals:**
- Replacing existing inline validation errors (login/register field errors remain as-is)
- Success toasts (only error notifications for now)
- Toast queueing or stacking (latest toast replaces any visible one)
- Custom toast styling beyond basic error appearance

## Decisions

### 1. Use CommunityToolkit.Maui `Toast` API

Use `CommunityToolkit.Maui.Alerts.Toast.Make(message, ToastDuration.Long).Show()` for displaying error messages. This is the simplest approach — the toolkit is already installed, and `Toast` shows natively on each platform (top on iOS, bottom on Android — this is standard platform behavior).

Alternatives considered:
- **Custom overlay view**: Would allow exact top positioning on all platforms, but requires managing a view layer above Shell, handling safe areas, animations, and lifecycle. Heavy for the current need.
- **Snackbar**: Supports custom duration but always anchors to the bottom. No top positioning support.

Trade-off: Android toasts appear at the bottom (platform convention). Accepting platform-native positioning is simpler and more familiar to users than fighting the platform.

For the 6-second duration: `ToastDuration.Long` is ~3.5s natively. We'll use `Snackbar` with `Duration = TimeSpan.FromSeconds(6)` and no action button to get the exact 6-second auto-dismiss behavior. Snackbar appears at the bottom but supports custom duration.

**Final decision**: Use `Snackbar` with 6-second duration, no action button, and error styling. It appears at the bottom (platform standard) but gives us exact duration control.

### 2. `IToastService` as a thin DI-injectable wrapper

Create `IToastService` with a single method `ShowError(string message)`. The implementation creates a `Snackbar` with 6-second duration and error styling, then calls `Show()` on the main thread. Registered as singleton in DI.

```
IToastService
  └── ShowError(string message): Task
```

Implementation dispatches to `MainThread.InvokeOnMainThreadAsync` to ensure it works when called from background threads (common in async service calls).

### 3. ViewModel-level try-catch with toast

Add try-catch around API calls at the ViewModel level (not the service level). This keeps services clean and lets ViewModels decide when/whether to show toasts.

Pattern:
```csharp
try
{
    // existing API call logic
}
catch (Exception ex)
{
    await _toastService.ShowError(ex.Message);
}
```

For methods that already use `AuthResult<T>` (login, register, resend): the outer catch-all in the ViewModel command should show a toast instead of silently swallowing.

For `AccountProfileViewModel.LoadProfile`: wrap the call in try-catch, show toast on failure while keeping `HasError = true` for the UI state.

For `ProfileViewModel.SignOut` and `AccountProfileViewModel.SignOut`: wrap logout in try-catch and toast on failure (logout is best-effort, so still proceed with local sign-out).

### 4. Inject `IToastService` into ViewModels via constructor

Each ViewModel that makes API calls gets `IToastService` added to its constructor. This follows the existing DI pattern used for `IAuthService`.

Affected ViewModels: `LoginViewModel`, `RegisterViewModel`, `EmailVerificationViewModel`, `AccountProfileViewModel`, `ProfileViewModel`.

## Risks / Trade-offs

- **[Snackbar at bottom, not top]** → Accepted trade-off. Platform-native positioning is consistent with user expectations. Custom top positioning would require a complex overlay system.
- **[Exception message may be technical]** → For now, we show `ex.Message` which may contain HTTP details. Future improvement: map exceptions to user-friendly localized messages. For `AuthResult` failures, we already have parsed error messages.
- **[Toast may overlap with keyboard on auth screens]** → Low risk since errors typically dismiss the keyboard or appear after submission completes.
