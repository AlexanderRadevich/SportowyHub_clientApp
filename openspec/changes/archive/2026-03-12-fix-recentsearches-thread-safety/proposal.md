# Fix RecentSearchesService Thread Safety

## Why

`RecentSearchesService` is registered as a singleton but uses a plain `List<string>` with mutating operations (`RemoveAll`, `Insert`, `RemoveRange`, `Clear`) without any synchronization. Concurrent calls from different threads can corrupt the list, causing `ArgumentOutOfRangeException` or lost data.

## What Changes

Add lock-based synchronization around all list operations in `RecentSearchesService` to prevent concurrent modification.

## Capabilities

### Modified

- `RecentSearchesService` — all list operations protected by a lock object

## Impact

- **Stability:** Eliminates potential list corruption from concurrent access
- **Performance:** Lock contention is negligible — recent searches is a low-frequency operation
- **Risk:** Low — behavioral semantics unchanged, only synchronization added
