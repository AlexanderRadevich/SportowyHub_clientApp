## Why

When the user switches to dark theme, the phone's status bar (battery, clock, signal icons) becomes invisible. The status bar text/icons blend into the dark background, making system information unreadable. This is a usability bug that affects every dark-mode user on every session.

## What Changes

- Investigate and fix the `StatusBarBehavior` in `AppShell.xaml` so that status bar icon style (`LightContent`/`DarkContent`) reliably updates when the app theme changes programmatically
- Handle Android 15+ edge-to-edge enforcement where `StatusBarColor` is deprecated and the status bar is always transparent — ensure proper contrast via `StatusBarStyle` and page background
- Ensure the status bar remains correct after `AppShell` reconstruction (triggered by theme/language changes in `ProfileViewModel`)
- Verify iOS `Info.plist` has `UIViewControllerBasedStatusBarAppearance` set to `false` for `StatusBarBehavior` to work reliably

## Capabilities

### New Capabilities

_(none)_

### Modified Capabilities

- `theming`: Fix status bar visibility when switching to dark theme; ensure `StatusBarBehavior` style and color update reliably across theme changes and Shell reconstruction

## Impact

- **AppShell.xaml** — May need to adjust or reinforce `StatusBarBehavior` bindings
- **Android platform** — `values/styles.xml` and `values-night/colors.xml` may need updates for edge-to-edge compatibility; potentially add safe-area padding handling
- **iOS platform** — `Info.plist` may need `UIViewControllerBasedStatusBarAppearance` entry
- **ProfileViewModel / HomeViewModel** — Theme toggle logic may need to explicitly re-apply status bar style after changing `UserAppTheme`
- **No new dependencies** — uses existing CommunityToolkit.Maui `StatusBarBehavior`
