## Context

The app supports light and dark themes via `AppThemeBinding`. Two controls were missed during theming: an ActivityIndicator on AccountProfilePage and a Switch on EditProfilePage. Both hard-code `{StaticResource Primary}` which only resolves correctly in light mode.

## Goals / Non-Goals

**Goals:**
- Fix the two identified controls to use `AppThemeBinding`
- Audit nearby controls for the same issue

**Non-Goals:**
- Redesigning the theme system
- Adding new color resources

## Decisions

- Use the same `AppThemeBinding Light={StaticResource Primary}, Dark={StaticResource PrimaryDark}` pattern already established in the codebase
- Audit other ActivityIndicator and Switch controls in the same pages for consistency

## Risks / Trade-offs

- Minimal risk — pattern is well-established in the project
- If `PrimaryDark` resource doesn't exist, it must be added to `Colors.xaml` first
