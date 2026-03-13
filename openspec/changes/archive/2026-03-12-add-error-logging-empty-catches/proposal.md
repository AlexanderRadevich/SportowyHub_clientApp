# Add Error Logging to Empty Catch Blocks

## Why

8+ files contain empty catch blocks that silently swallow exceptions, making debugging impossible. When errors occur in production, there is no trace of what went wrong. This affects `ApiErrorParser`, `AuthService`, `FavoritesService`, and multiple ViewModels.

## What Changes

Add structured logging to every empty catch block across the codebase. Inject `ILogger<T>` into classes that lack it. Use appropriate log levels: `LogWarning` for unexpected errors, `LogDebug` for intentional swallows like `TaskCanceledException` during debounce.

## Capabilities

### Modified

- `ApiErrorParser` — log parsing failures instead of silently returning defaults
- `AuthService` — log logout/cleanup errors
- `FavoritesService` — log HTTP 409 Conflict handling
- `HomeViewModel` — log section loading and favorite toggle failures
- `LoginViewModel` — log authentication error handling failures
- `RegisterViewModel` — log registration error handling failures
- `SearchFilterPopupViewModel` — log location/filter operation failures
- `SearchViewModel` — log search and debounce failures

## Impact

- **Debugging:** Errors become visible in logs, drastically reducing investigation time
- **Risk:** Minimal — logging does not change control flow
- **Dependencies:** `Microsoft.Extensions.Logging` (already referenced)
