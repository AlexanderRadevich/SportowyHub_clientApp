# Add Service Input Validation -- Design

## Context

Service methods currently trust all input and forward it to the API. Null or empty strings cause confusing HTTP errors or silent failures rather than clear argument exceptions at the call site.

## Goals / Non-Goals

### Goals

- Every service method that accepts user-controlled string/id input has guard clauses
- Use `ArgumentException.ThrowIfNullOrWhiteSpace` for strings and `ArgumentOutOfRangeException.ThrowIfNegativeOrZero` for numeric IDs
- Fail fast before any network call

### Non-Goals

- Full input validation (length limits, format checks) -- that belongs in the ViewModel layer
- Changing API error handling or response parsing

## Decisions

- Use .NET built-in `ArgumentException.ThrowIfNullOrWhiteSpace` and `ArgumentOutOfRangeException.ThrowIfNegativeOrZero`
- Place guards as the first lines of each affected method
- Add corresponding unit tests that assert `ArgumentException` / `ArgumentOutOfRangeException` for invalid inputs

## Risks / Trade-offs

- **Behavioral change for callers passing empty strings:** Previously these would fail at the API level; now they fail at the service level. This is strictly better behavior.
- **No risk to valid-input paths:** Guards only trigger on null/empty/whitespace/negative values
