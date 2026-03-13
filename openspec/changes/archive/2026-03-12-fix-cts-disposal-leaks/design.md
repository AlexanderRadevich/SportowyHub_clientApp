# Design: Fix CancellationTokenSource Disposal Leaks

## Context

Debounce patterns in search-related ViewModels create new `CancellationTokenSource` instances to cancel previous debounce timers. The old CTS is cancelled but never disposed. Over time, this leaks native timer handles.

## Goals / Non-Goals

### Goals
- Both ViewModels implement `IDisposable`
- All CTS fields are disposed in `Dispose()`
- Pages trigger ViewModel disposal on navigation away

### Non-Goals
- Implementing a reusable debounce abstraction (potential future improvement)
- Changing the debounce mechanism itself

## Decisions

1. **Standard `IDisposable` pattern** — implement `Dispose(bool disposing)` with `_disposed` guard
2. **Cancel then dispose** — in `Dispose`, call `_cts?.Cancel()` then `_cts?.Dispose()` to ensure pending work is cancelled
3. **Page triggers disposal** — override `OnDisappearing` in the page to call `(BindingContext as IDisposable)?.Dispose()`. For the popup, dispose on `OnClosed`.
4. **Dispose old CTS in debounce method** — when creating a new CTS, dispose the previous one after cancelling it (not just cancel)

## Risks / Trade-offs

- **Double disposal:** If both the debounce method and `Dispose()` try to dispose the same CTS, the second call is a no-op (CTS.Dispose is idempotent).
- **OnDisappearing timing:** `OnDisappearing` fires for both navigation away and page overlay. Use a flag or only dispose on final navigation if this becomes an issue.
