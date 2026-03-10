## Why

The current filter chips on the search page use a single tappable area for both the label and the close button. The close icon is only 10×10px, making it difficult to tap accurately — users often accidentally remove filters or fail to dismiss them. A split-pill design with a visually distinct, always-visible remove zone improves usability and matches the updated visual language.

## What Changes

- Redesign active filter chips from a single-region pill to a split-pill layout with two distinct zones:
  - **Label zone** (left): colored dot indicator + filter label text
  - **Remove zone** (right): visually separated "×" button with its own tap target
- Add a subtle vertical divider between the label and remove zones
- Increase the remove button tap target to a comfortable size (the entire right zone)
- Update chip styling: rounded pill shape with border stroke, matching the dark/light theme

## Capabilities

### New Capabilities

_(none)_

### Modified Capabilities

- `filter-chips-ui`: Redesign search page filter chip layout from single-region to split-pill with separate label and remove zones, colored dot indicator, and enlarged tap target for removal

## Impact

- `SportowyHub.App/Views/Search/SearchPage.xaml` — filter chip `DataTemplate` rewritten
- `SportowyHub.App/Resources/Styles/Styles.xaml` — new or updated styles for split-pill chip
- No model changes needed — `ActiveFilterChip` record stays the same
- No ViewModel changes — same `RemoveFilterCommand` binding, same `ActiveFilterChips` collection
