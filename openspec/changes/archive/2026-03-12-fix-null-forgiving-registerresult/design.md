## Context

The RegisterViewModel processes registration results and accesses `Data.TrustLevel` after checking `IsSuccess`. The null-forgiving operator (`!`) suppresses the compiler warning but does not prevent runtime NREs if the API returns a success status with a null payload.

## Goals / Non-Goals

**Goals:**
- Add a null guard before accessing `registerResult.Data`
- Show a meaningful error if Data is unexpectedly null

**Non-Goals:**
- Refactoring the Result pattern
- Changing API contract validation

## Decisions

- Check `registerResult.Data is null` after the success check
- If null, show an error toast and return early
- Remove the null-forgiving operator

## Risks / Trade-offs

- Minimal risk — adds defensive code without altering the success path
- The null case should be rare but is now handled gracefully instead of crashing
