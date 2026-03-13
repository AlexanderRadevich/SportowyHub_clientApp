# Design: Fix Favorites Cache Atomicity

## Context

`FavoritesService` is a singleton with a `ConcurrentDictionary<int, byte> _favoriteIds` field. The dictionary itself is thread-safe, but the field is reassigned to a new instance during load and clear operations. Any code holding a reference to the old instance will not see updates.

## Goals / Non-Goals

### Goals
- Single `ConcurrentDictionary` instance for the lifetime of the service
- All mutations happen in-place on that instance

### Non-Goals
- Making the load operation itself atomic (brief window during Clear+TryAdd is acceptable)
- Changing the caching strategy

## Decisions

1. **Mark field as `readonly`** — prevents accidental reassignment after construction
2. **`Clear()` + loop `TryAdd()`** — replaces `_favoriteIds = new ConcurrentDictionary<>(data)` pattern
3. **Accept brief inconsistency during reload** — between `Clear()` and the last `TryAdd()`, a reader may see partial data. This is acceptable for a favorites cache that is eventually consistent.

## Risks / Trade-offs

- **Brief partial state during reload:** A reader between Clear and final TryAdd sees incomplete favorites. Acceptable — UI will refresh on completion. If this becomes a problem, a snapshot-swap with `Interlocked.Exchange` on a volatile field is the alternative.
