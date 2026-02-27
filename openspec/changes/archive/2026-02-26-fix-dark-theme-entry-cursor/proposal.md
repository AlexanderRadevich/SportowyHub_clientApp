## Why

In dark theme, the Entry text cursor is nearly invisible because it uses the default platform color (dark), which blends into the dark surface background (`#1E1E1E`). The cursor color needs to adapt to the active theme.

## What Changes

- Add `CursorColor` property with `AppThemeBinding` to the implicit `Entry` style in `Styles.xaml`, using `Primary` for light theme and `PrimaryDark` for dark theme so the cursor is always visible and on-brand

## Capabilities

### New Capabilities
_(none)_

### Modified Capabilities
- `theming`: Add Entry cursor color requirement to branded control styles

## Impact

- **Styles**: Single setter addition in `Resources/Styles/Styles.xaml` Entry style
- **No code changes**: Pure XAML styling fix
- **All Entry fields affected**: LoginPage, RegisterPage, and any future Entry usage
