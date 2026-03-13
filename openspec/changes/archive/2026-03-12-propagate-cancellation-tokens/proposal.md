# Propagate Cancellation Tokens

## Why

Multiple ViewModel async methods either pass `CancellationToken.None` or omit the token entirely. This means navigating away from a page does not cancel in-flight HTTP requests, wasting bandwidth and potentially updating UI on a page that is no longer visible.

## What Changes

Add `CancellationToken` parameters to all `[RelayCommand]` async methods that lack them. The CommunityToolkit.Mvvm source generator automatically provides a cancellation token to async relay commands. Propagate these tokens through to all service and HTTP calls.

## Capabilities

### Modified

- `AccountProfileViewModel.SignOutAsync` — accept and propagate token
- `HomeViewModel.LoadSectionsAsync`, `ToggleFavoriteAsync` — accept and propagate token
- `ProfileViewModel.RefreshAuthStateAsync` — accept and propagate token
- `SearchViewModel.AppearingAsync` — accept and propagate token
- `MyListingsViewModel` — accept and propagate token in async commands
- `FavoritesViewModel` — accept and propagate token in async commands

## Impact

- **Responsiveness:** Navigating away cancels pending work instead of letting it complete in the background
- **Resource usage:** Cancelled HTTP requests free up connections sooner
- **Risk:** Low — `OperationCanceledException` is expected and handled by the MVVM toolkit
