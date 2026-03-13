## Why

`AccountProfilePage.xaml:17` ActivityIndicator uses `{StaticResource Primary}` without `AppThemeBinding`, rendering light-theme color in dark mode. `EditProfilePage.xaml:99` Switch.OnColor has the same issue. These controls break visual consistency when the user switches to dark theme.

## What Changes

Add `AppThemeBinding` with light/dark color variants to the affected controls, matching the pattern already used by all other themed ActivityIndicators and controls in the project.

## Capabilities

### Modified

- AccountProfilePage ActivityIndicator respects dark theme
- EditProfilePage Switch.OnColor respects dark theme

## Impact

- Visual-only fix; no logic or data changes
- Improves dark mode consistency across profile-related pages
- Low risk — follows existing theming pattern used elsewhere in the app
