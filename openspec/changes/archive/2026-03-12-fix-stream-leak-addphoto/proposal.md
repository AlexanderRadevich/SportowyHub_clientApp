# Fix Stream Leak in AddPhoto

## Why

`CreateEditListingViewModel.cs:260` calls `await file.OpenReadAsync()` but the returned stream is never disposed. This leaks file handles, which can exhaust OS resources on repeated photo additions.

## What Changes

Wrap the stream in an `await using` declaration to ensure deterministic disposal.

## Capabilities

### New
- None

### Modified
- `CreateEditListingViewModel.cs` — add `await using` to the stream returned by `OpenReadAsync`

## Impact

- **Reliability** — prevents file handle leaks during photo addition
- **Resources** — deterministic cleanup of native file handles
