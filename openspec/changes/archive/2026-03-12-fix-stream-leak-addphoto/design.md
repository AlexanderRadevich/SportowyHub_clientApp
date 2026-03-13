# Design: Fix Stream Leak in AddPhoto

## Context

`CreateEditListingViewModel.cs` calls `file.OpenReadAsync()` and uses the returned stream without disposing it. This leaks native file handles on every photo addition.

## Goals / Non-Goals

### Goals
- Ensure the stream from `OpenReadAsync` is deterministically disposed
- Audit the file for any other undisposed streams

### Non-Goals
- Refactor the photo upload pipeline
- Change how photos are stored or processed

## Decisions

1. **`await using` declaration** — Wrap the stream with `await using var stream = await file.OpenReadAsync()`. This is the simplest and most idiomatic fix.
2. **Audit scope** — Check all `OpenReadAsync` and similar stream-returning calls in the same file for consistent disposal.

## Risks / Trade-offs

- **Stream lifetime** — Must ensure the stream is not used after the `await using` scope ends. If the stream is passed to an async operation that outlives the scope, the disposal would cause errors. Verify the stream is fully consumed within scope.
