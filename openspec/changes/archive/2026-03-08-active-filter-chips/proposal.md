## Why

When a user applies search filters (via the filter popup or by tapping a sport category on the home screen), there is no visible indication of which filters are active. The only feedback is a small badge count on the filter icon. Users cannot see or quickly remove individual filters without reopening the full filter popup.

## What Changes

- Add a horizontal row of dismissible filter chips below the search bar on the Search page
- Each active filter (sport, category, location, price range, condition, sort) renders as a separate chip showing a human-readable label and an "X" close button
- Tapping the close button on a chip removes that specific filter from `SearchFilterState` and re-executes the search
- The chip row is only visible when at least one filter is active
- Chips scroll horizontally if they overflow the screen width

## Capabilities

### New Capabilities
- `filter-chips-ui`: Horizontal scrollable row of dismissible filter chips displayed below the search bar, each representing one active filter with a close button to remove it

### Modified Capabilities

## Impact

- `SearchPage.xaml` — new chip row UI between search bar and results
- `SearchViewModel` — new observable collection of active filter chip descriptors, commands to remove individual filters
- `SearchFilterState` — may need individual property clear methods
- Localization — chip labels need localized filter type prefixes (e.g., "Sport: Hockey", "Condition: New")
- All 4 .resx files — new strings for chip label prefixes
