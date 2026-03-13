# Fix Chip Theme Reactivity

## Why

`HomePage.xaml.cs` methods `CreateSectionChip` and `StyleChip` bake concrete `Color` values from `RequestedTheme` at creation time. When the user toggles the theme without navigating away, chips retain stale colors from the previous theme.

## What Changes

Replace hardcoded Color values with `AppThemeBinding` or `DynamicResource` references so chips react to theme changes in real-time. Consider moving chip creation to XAML `DataTemplate` with style bindings.

## Capabilities

### New
- Theme-reactive chip styling

### Modified
- `HomePage.xaml.cs` — replace hardcoded colors with AppThemeBinding in CreateSectionChip and StyleChip/UpdateChipStyles
- Potentially `HomePage.xaml` and `Styles.xaml` — chip styles as resources or DataTemplate

## Impact

- **UX** — chips update immediately on theme toggle without requiring navigation
- **Consistency** — chip colors always match the active theme
