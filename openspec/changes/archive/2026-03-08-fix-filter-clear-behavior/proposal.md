## Why

Two UX issues with search filter management: (1) removing the last active filter chip leaves the user on an empty search page with cursor focused on the search bar instead of showing all listings, and (2) tapping "Clear Filters" in the filter popup resets the form but doesn't close the popup or refresh results — users expect it to behave like applying empty filters.

## What Changes

- When the last filter chip is removed via the close button, execute a search with no filters to show all available items instead of clearing results and focusing the search bar.
- When "Clear Filters" is tapped in the filter popup, reset the filter state, close the popup, and trigger a search refresh with default (no-filter) parameters.

## Capabilities

### New Capabilities

_(none)_

### Modified Capabilities

- `filter-chips-ui`: Clear Filters now closes popup and refreshes results; removing the last chip shows all items instead of empty state.

## Impact

- `SearchViewModel.RemoveFilter` — adjust behavior when removing last filter to keep showing results
- `SearchViewModel.OnSearchTextChanged` — ensure clearing search text with no filters still shows all items when coming from a filtered state
- `SearchFilterPopupViewModel.Reset` — change from local-only reset to triggering Applied event with empty state
- `SearchFilterPopup.xaml.cs` — handle the reset-as-apply flow to close the popup
