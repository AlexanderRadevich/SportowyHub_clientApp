# Design: Fix RecentSearchesService Thread Safety

## Context

`RecentSearchesService` is a singleton managing recent search terms as a `List<string>`. Multiple ViewModels can call Add/Get/Clear concurrently from different threads, but `List<T>` is not thread-safe for concurrent writes.

## Goals / Non-Goals

### Goals
- All list operations are thread-safe
- No behavioral changes to the public API

### Non-Goals
- Changing from singleton to scoped lifetime
- Persisting recent searches to disk (separate concern)

## Decisions

1. **Use `lock` over `ConcurrentQueue`** — the service needs index-based operations (`Insert(0, ...)`, `RemoveRange`) that `ConcurrentQueue` doesn't support. A lock around the existing `List<string>` is the simplest correct approach.
2. **Private `readonly object _lock = new()`** — standard lock pattern, avoids locking on `this` or the list itself.
3. **Return copies from read operations** — `GetRecentSearches()` returns `new List<string>(_recentSearches)` to prevent external mutation.

## Risks / Trade-offs

- **Lock granularity:** Single lock for all operations is simple but could block reads during writes. Acceptable given the low frequency of recent search operations.
- **Alternative considered:** `ReaderWriterLockSlim` would allow concurrent reads, but adds complexity for minimal gain here.
