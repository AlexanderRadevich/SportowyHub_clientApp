## Why

The close (remove) button on active filter chips in the search screen feels oversized relative to the chip itself, making the chips look cluttered. Reducing the button size will improve visual balance and match typical chip design conventions.

## What Changes

- Reduce the `ImageButton` close button dimensions on active filter chips from 14×14 to 10×10
- Adjust spacing between the label and close button to maintain visual harmony at the smaller size

## Capabilities

### New Capabilities

_(none)_

### Modified Capabilities

- `filter-chips-ui`: Close button size requirement changes from 14×14 to 10×10

## Impact

- `SportowyHub.App/Views/Search/SearchPage.xaml` — filter chip `DataTemplate` (lines ~120-130)
- No API, model, or service changes
- Visual-only change; no behavioral impact
