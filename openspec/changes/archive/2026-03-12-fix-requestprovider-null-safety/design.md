# Design: Fix RequestProvider Null Safety

## Context

`RequestProvider` is the central HTTP client wrapper. It deserializes API responses using `System.Text.Json` with source generation. All deserialization results are treated as non-null via the `!` operator, creating 6 potential NRE sites.

## Goals / Non-Goals

### Goals
- Every deserialization result is explicitly null-checked
- Null results throw `InvalidOperationException` with the expected type name
- Remove all `!` null-forgiving operators from deserialization results

### Non-Goals
- Changing to Result pattern (separate architectural change)
- Adding retry logic for null responses

## Decisions

1. **`InvalidOperationException`** — appropriate for "the operation completed but produced an invalid state" semantics
2. **Include type name in message** — `$"API returned null response for {typeof(TResult).Name}"` aids debugging
3. **Extract to helper method** — if pattern repeats 6 times, a private `EnsureNotNull<T>(T? result)` method reduces duplication

## Risks / Trade-offs

- **Behavior change:** Previously threw NRE at call site; now throws InvalidOperationException at deserialization site. Callers catching `NullReferenceException` specifically (unlikely) would need updating.
