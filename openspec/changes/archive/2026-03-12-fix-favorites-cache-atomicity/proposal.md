# Fix Favorites Cache Atomicity

## Why

`FavoritesService` uses a `ConcurrentDictionary` for `_favoriteIds` but reassigns the entire field in `LoadFavoriteIdsAsync` and `ClearCache`. This creates a race condition: concurrent readers may hold a reference to the old dictionary while new data is being written to a different instance.

## What Changes

Replace field reassignment with in-place `Clear()` + bulk `TryAdd()` operations on the existing `ConcurrentDictionary` instance, ensuring all readers and writers operate on the same object.

## Capabilities

### Modified

- `FavoritesService.LoadFavoriteIdsAsync` — use `Clear()` + `TryAdd()` instead of `new ConcurrentDictionary<>()`
- `FavoritesService.ClearCache` — use `Clear()` instead of field reassignment

## Impact

- **Correctness:** Eliminates stale reference race condition
- **Risk:** Low — `ConcurrentDictionary.Clear()` is atomic with respect to individual operations
- **Performance:** Negligible difference
