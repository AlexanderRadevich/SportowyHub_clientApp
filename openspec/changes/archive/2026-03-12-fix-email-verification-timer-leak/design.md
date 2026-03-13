# Design: Fix Email Verification Timer Leak

## Context

`EmailVerificationViewModel` uses `IDispatcherTimer` for a resend-code cooldown countdown. The timer callback captures `this` via a lambda, creating a strong reference from the timer to the ViewModel. If the page is navigated away from without explicitly stopping the timer, the ViewModel cannot be garbage collected.

## Goals / Non-Goals

### Goals
- ViewModel implements `IDisposable` and stops the timer in `Dispose`
- Page disposes ViewModel when navigating away
- Timer cannot keep ViewModel alive after page is popped

### Non-Goals
- Replacing the timer mechanism with a different approach
- Adding a resend cooldown to other pages

## Decisions

1. **`IDisposable` on ViewModel** — `Dispose` calls `StopCooldownTimer()` (method already exists)
2. **Page `OnDisappearing` triggers dispose** — `(BindingContext as IDisposable)?.Dispose()` in the page's `OnDisappearing` override
3. **WeakReference as defense-in-depth** — store a `WeakReference<EmailVerificationViewModel>` and use it in the timer callback. If the ViewModel is collected despite the timer running, the callback becomes a no-op. This is a safety net, not a replacement for proper disposal.

## Risks / Trade-offs

- **OnDisappearing false positives:** `OnDisappearing` fires when a modal is pushed over the page, not just on back navigation. Mitigate by checking `Navigation.NavigationStack` or using a disposed flag to make Dispose idempotent.
- **WeakReference complexity:** Adds a small amount of indirection. Worth it as a GC safety net for timer scenarios.
