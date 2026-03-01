## Why

Recent searches are hardcoded placeholders from the MVP phase. Users expect their actual search history to appear, allowing quick re-execution of past queries. Without persistence, every app launch shows the same dummy data, reducing the feature's utility.

## What Changes

- Replace the hardcoded `RecentSearches` collection in `SearchViewModel` with a persisted list stored via `Preferences`
- Record each executed search query into recent searches (after debounce completes successfully)
- Limit stored history to the 10 most recent unique queries
- Add the ability to clear all recent searches from the UI
- Show an empty state when no recent searches exist (hide the section entirely)

## Capabilities

### New Capabilities
- `recent-searches-persistence`: Local persistence of recent search queries using `Preferences`, including storage service, read/write operations, max-item cap, duplicate handling, and clear functionality

### Modified Capabilities
- `search-ui`: Update search page to support dynamic recent searches — hide section when empty, add a "clear" action next to the section header

## Impact

- `SearchViewModel.cs` — remove hardcoded data, inject persistence service, record searches on execution
- `SearchPage.xaml` — conditional visibility for recent searches section, add clear button
- `MauiProgram.cs` — register new service
- New `Services/RecentSearches/` — `IRecentSearchesService` + `RecentSearchesService`
- Localization `.resx` files — add string for "Clear" action
