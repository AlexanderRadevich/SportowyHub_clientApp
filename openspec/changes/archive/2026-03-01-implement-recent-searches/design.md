## Context

The search page currently displays a hardcoded list of 5 recent search strings directly in `SearchViewModel`. The app already uses `Preferences` for simple key-value storage (theme, language) and `SecureStorage` for tokens. Recent searches are non-sensitive, low-volume string data — a natural fit for `Preferences`.

## Goals / Non-Goals

**Goals:**
- Persist recent searches locally across app restarts
- Record searches automatically when a query executes successfully
- Cap history at 10 unique entries (most recent first)
- Allow users to clear all recent searches
- Hide the recent searches section when the list is empty

**Non-Goals:**
- Syncing recent searches across devices or to a backend
- Per-item delete (swipe to remove individual entries)
- Search analytics or tracking
- Persisting popular searches (remains hardcoded for now)

## Decisions

### 1. Storage mechanism: `Preferences` with JSON serialization

Store the list as a single JSON string under a `Preferences` key.

**Why over alternatives:**
- SQLite: overkill for a list of max 10 strings
- File-based: adds file I/O complexity for trivial data
- `Preferences` is already used in the app for settings, consistent pattern

**Key:** `"recent_searches"`
**Format:** JSON array of strings, e.g. `["yoga mat","basketball","running shoes"]`
**Serialization:** `System.Text.Json` with the existing `SportowyHubJsonContext` source generator

### 2. Service abstraction: `IRecentSearchesService`

Encapsulate all persistence logic behind a service interface injected into `SearchViewModel`. This keeps the ViewModel testable and follows the existing service pattern.

**Lifetime:** Singleton — the list is small and shared app-wide.

**API:**
- `IReadOnlyList<string> GetAll()` — synchronous, reads from Preferences
- `void Add(string query)` — adds to front, deduplicates, trims to max 10, persists
- `void Clear()` — removes all entries and the Preferences key

Synchronous API because `Preferences.Get`/`Set` is synchronous and the data is trivial.

### 3. Recording trigger: after successful search execution

Record the query in `SearchViewModel.ExecuteSearch` only when `offset == 0` (initial search, not pagination) and results return without error. This avoids storing partial/cancelled queries.

### 4. UI: conditional section visibility + clear button

- Bind `IsVisible` on the recent searches `VerticalStackLayout` to `RecentSearches.Count > 0`
- Add a "Clear" label/button next to the "Recent Searches" header, bound to a `ClearRecentSearchesCommand`

## Risks / Trade-offs

- **`Preferences` size limit on some platforms** → Mitigated by capping at 10 short strings (~500 bytes max). Well within platform limits.
- **Synchronous Preferences access on UI thread** → Acceptable for this data volume. If it ever becomes a concern, the service interface allows swapping to async without ViewModel changes.
