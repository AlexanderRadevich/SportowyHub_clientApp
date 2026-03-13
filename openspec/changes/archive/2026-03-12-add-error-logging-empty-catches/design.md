# Design: Add Error Logging to Empty Catch Blocks

## Context

Empty catch blocks across 8+ files silently swallow exceptions. This is a widespread pattern that likely originated from rapid prototyping where error handling was deferred.

## Goals / Non-Goals

### Goals
- Every catch block logs the exception with structured logging
- Appropriate log levels: Warning for unexpected errors, Debug for intentional swallows
- ILogger<T> injected via constructor into all affected classes

### Non-Goals
- Changing error handling behavior (no new throws, no UI error messages)
- Refactoring catch blocks into Result pattern (separate change)

## Decisions

1. **Use `ILogger<T>` via DI** — consistent with project conventions and enables filtering per class
2. **Warning level for unexpected catches** — `_logger.LogWarning(ex, "Failed to {Action}", action)` with structured message templates
3. **Debug level for intentional swallows** — `TaskCanceledException` during debounce is expected; log at Debug to avoid noise
4. **No control flow changes** — catch blocks continue to swallow; we only add visibility

## Risks / Trade-offs

- **Log volume:** Adding logging to frequently-hit catch blocks (debounce cancellation) could produce noise — mitigated by using Debug level
- **Constructor changes:** Adding ILogger parameter to classes changes their DI registration signature, but all are already registered in MauiProgram.cs and ILogger is auto-resolved
