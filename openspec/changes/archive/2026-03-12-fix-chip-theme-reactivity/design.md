# Design: Fix Chip Theme Reactivity

## Context

`HomePage.xaml.cs` creates section chips programmatically with hardcoded `Color` values read from `Application.Current.RequestedTheme` at creation time. When the theme changes, chips retain stale colors.

## Goals / Non-Goals

### Goals
- Chips update colors immediately when the app theme changes
- Maintain the current chip appearance in both light and dark themes

### Non-Goals
- Redesign the chip component
- Add custom theme support beyond light/dark

## Decisions

1. **AppThemeBinding** — Use `AppThemeBinding` for chip `BackgroundColor`, `TextColor`, and `BorderColor` properties. This lets MAUI handle theme change reactivity automatically.
2. **Style resources** — Define chip styles (selected/unselected variants) in `Styles.xaml` as named styles, then apply them in code-behind. This centralizes theme colors and makes them DynamicResource-friendly.
3. **Consider XAML DataTemplate** — If feasible, move chip creation from code-behind to XAML using a `BindableLayout` with `DataTemplate`. This would eliminate the code-behind color logic entirely.

## Risks / Trade-offs

- **AppThemeBinding in code-behind** — Setting `AppThemeBinding` programmatically requires using `SetAppThemeColor` extension or `OnPlatform`/`OnIdiom` approaches. Slightly more verbose than XAML.
- **Migration complexity** — Moving from programmatic chip creation to XAML DataTemplate is a larger refactor but yields a cleaner result.
