# Fix CancellationTokenSource Disposal Leaks

## Why

`SearchFilterPopupViewModel` and `SearchViewModel` create `CancellationTokenSource` fields (`_locationDebounceCts`, `_debounceCts`) for debounce logic. These CTS instances are never disposed, and neither ViewModel implements `IDisposable`. Each undisposed CTS leaks a timer handle and registered callbacks.

## What Changes

Implement `IDisposable` on both ViewModels. Dispose CTS fields in the `Dispose` method. Ensure corresponding pages trigger disposal when navigating away.

## Capabilities

### Modified

- `SearchFilterPopupViewModel` — implement `IDisposable`, dispose `_locationDebounceCts`
- `SearchViewModel` — implement `IDisposable`, dispose `_debounceCts`
- `SearchFilterPopupPage` — dispose ViewModel on close
- `SearchPage` — dispose ViewModel on `OnDisappearing`

## Impact

- **Memory:** Eliminates CTS timer handle leaks on repeated navigation
- **Risk:** Low — disposal only happens when the page is no longer in use
- **Dependencies:** None
