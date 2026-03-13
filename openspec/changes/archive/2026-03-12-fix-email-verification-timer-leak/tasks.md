# Tasks

## Tasks

- [x] Implement `IDisposable` on `EmailVerificationViewModel`
- [x] Call `StopCooldownTimer()` in `Dispose` method
- [x] Add `_disposed` guard to prevent double disposal
- [x] Override `OnDisappearing` in `EmailVerificationPage.xaml.cs` to dispose ViewModel
- [x] Consider using `WeakReference<EmailVerificationViewModel>` in timer callback as GC safety net (not needed — Dispose from OnDisappearing already stops the timer; WeakReference would add complexity without benefit since the timer is short-lived and properly cleaned up)
- [x] Verify timer stops when navigating away from the page
- [x] Verify `dotnet build` succeeds
