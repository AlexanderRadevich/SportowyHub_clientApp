## Why

API errors (network failures, server errors, auth expiry) are currently either swallowed silently (`GetProfileAsync`, `LogoutAsync`) or shown only via inline observable properties that require the user to notice a text change. There is no centralized, prominent notification to inform the user when something goes wrong. A top-positioned toast that auto-dismisses after 6 seconds provides consistent, non-blocking feedback across all screens.

## What Changes

- Create a reusable `IToastService` with a `ShowError(string message)` method that displays an error toast at the top of the screen for 6 seconds
- Implement toast as a custom overlay view (top-anchored banner) since CommunityToolkit.Maui's built-in `Toast`/`Snackbar` don't support reliable top positioning cross-platform
- Wrap all ViewModel-level API calls in try-catch and show the exception message via `IToastService` on failure
- Surface error details from currently-silent failures (`GetProfileAsync`, `LogoutAsync`) to users via toast
- Register `IToastService` in DI so it's injectable into ViewModels

## Capabilities

### New Capabilities
- `toast-notifications`: A reusable toast notification service and UI overlay that displays error messages at the top of the screen with 6-second auto-dismiss

### Modified Capabilities
_(none — existing specs describe requirements, not error handling implementation details)_

## Impact

- **New files**: `Services/Toast/IToastService.cs`, `Services/Toast/ToastService.cs`, toast overlay view/control
- **Modified files**: All ViewModels that make API calls (`LoginViewModel`, `RegisterViewModel`, `EmailVerificationViewModel`, `AccountProfileViewModel`, `ProfileViewModel`) — add try-catch with toast on error
- **Modified files**: `MauiProgram.cs` — register `IToastService`
- **Dependencies**: None new — uses existing MAUI controls for the overlay
- **No breaking changes**: Existing inline error display remains; toast is additive
