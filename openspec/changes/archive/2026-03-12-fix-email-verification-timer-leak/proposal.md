# Fix Email Verification Timer Leak

## Why

`EmailVerificationViewModel.StartCooldown` (line 88-104) creates a timer whose lambda captures `this`, preventing garbage collection. `StopCooldownTimer()` exists but relies on code-behind to call it. If the page is popped without calling stop, the timer keeps ticking indefinitely, holding the ViewModel in memory and executing UI updates on a defunct page.

## What Changes

Implement `IDisposable` on `EmailVerificationViewModel` and stop the timer in `Dispose`. Ensure the page disposes the ViewModel when navigating away. Optionally use `WeakReference` in the timer callback to allow GC even if disposal is missed.

## Capabilities

### Modified

- `EmailVerificationViewModel` — implement `IDisposable`, stop timer in Dispose
- `EmailVerificationPage.xaml.cs` — dispose ViewModel on navigation away

## Impact

- **Memory:** Prevents ViewModel from being held alive by timer after page is popped
- **Correctness:** Prevents timer callbacks from updating properties on a detached ViewModel
- **Risk:** Low — adds cleanup that should have been there from the start
