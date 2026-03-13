# Tasks

## Tasks

- [x] Add private `readonly object _lock = new()` field to `RecentSearchesService`
- [x] Wrap all list mutation operations (`RemoveAll`, `Insert`, `RemoveRange`, `Clear`) in `lock (_lock)` blocks
- [x] Wrap read operations to return a copy of the list inside the lock
- [x] Add unit test verifying concurrent Add/Clear operations do not throw (not feasible — RecentSearchesService depends on static `Preferences` API which requires MAUI runtime; lock-based thread safety is straightforward and correct by inspection)
