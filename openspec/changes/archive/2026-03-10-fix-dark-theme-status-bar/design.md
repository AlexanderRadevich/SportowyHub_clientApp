## Context

The app uses `CommunityToolkit.Maui.StatusBarBehavior` in `AppShell.xaml` with `AppThemeBinding` for both `StatusBarColor` and `StatusBarStyle`. There are two theme-switching paths:

1. **HomeViewModel.ToggleTheme** — Sets `Application.Current.UserAppTheme` directly. Does NOT reconstruct AppShell.
2. **ProfileViewModel.OnSelectedThemeIndexChanged** — Sets `UserAppTheme`, then reconstructs AppShell via `Application.Current.Windows[0].Page = new AppShell()`.

The problem: when switching to dark theme via the home toggle, `AppThemeBinding` on `StatusBarBehavior` may not re-evaluate because the behavior is already attached and the Android platform status bar color is not re-applied. On Android 15+ (API 35), `StatusBarColor` is deprecated entirely — the status bar is always transparent in edge-to-edge mode, so only `StatusBarStyle` matters for icon visibility.

Additionally, iOS requires `UIViewControllerBasedStatusBarAppearance = false` in `Info.plist` for `StatusBarBehavior` to control the status bar style — this entry is currently missing.

## Goals / Non-Goals

**Goals:**
- Status bar icons/text are always visible regardless of theme (light or dark)
- Status bar updates immediately when theme changes via either toggle path
- Works on Android (including API 35+ edge-to-edge) and iOS

**Non-Goals:**
- Per-page status bar customization (all pages share the same status bar style)
- Android navigation bar styling (bottom gesture bar)
- Supporting Android versions below the current minimum SDK

## Decisions

### 1. Use imperative `StatusBar.SetColor`/`SetStyle` instead of XAML behavior binding

**Choice**: Replace the `AppThemeBinding`-based `StatusBarBehavior` in `AppShell.xaml` with imperative calls to `CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor()` and `StatusBar.SetStyle()` triggered from theme-switching code.

**Why**: `AppThemeBinding` on `StatusBarBehavior` is unreliable when `UserAppTheme` changes programmatically — the behavior may not re-evaluate its bindings. Imperative calls give deterministic control and work consistently across both toggle paths.

**Alternative considered**: Keeping XAML behavior and forcing re-evaluation by detaching/reattaching. Rejected because it's fragile and the AppShell reconstruction path already works — the problem is the non-reconstruction path.

**Implementation**: Create a static helper method (e.g., `StatusBarHelper.Apply(AppTheme)`) that calls the CommunityToolkit static APIs. Call it from both `HomeViewModel.ToggleTheme()` and `ProfileViewModel.OnSelectedThemeIndexChanged()`, plus once at startup in `App.xaml.cs`.

### 2. Keep StatusBarColor for pre-Android 15, accept transparency on Android 15+

**Choice**: Continue setting `StatusBarColor` for older Android versions. On Android 15+ where it's ignored, the transparent status bar will show the page background behind it, which is fine since `StatusBarStyle` ensures icon contrast.

**Why**: No extra effort needed. The page background already provides the right contrast (white for light, #121212 for dark). Setting the color is a no-op on API 35+ but harmless.

**Alternative considered**: Explicitly handling edge-to-edge safe area padding. Not needed because MAUI already respects safe areas by default (`Page.IgnoreSafeArea = false`).

### 3. Add iOS Info.plist entry

**Choice**: Add `UIViewControllerBasedStatusBarAppearance = false` to `Platforms/iOS/Info.plist`.

**Why**: Required by CommunityToolkit.Maui documentation for `StatusBarBehavior`/`StatusBar` static API to work on iOS. Currently missing.

### 4. Keep AppShell.xaml StatusBarBehavior as fallback

**Choice**: Keep the `StatusBarBehavior` in `AppShell.xaml` as a baseline that applies on Shell construction (covers the AppShell reconstruction path and initial load). The imperative calls handle the non-reconstruction toggle path.

**Why**: Belt-and-suspenders approach. The XAML behavior handles initial state and the ProfileViewModel reconstruction path correctly already. The imperative call fixes the HomeViewModel toggle path.

## Risks / Trade-offs

- **CommunityToolkit static API stability** → These are public APIs in `CommunityToolkit.Maui.Core.Platform`. Low risk but not the "preferred" approach per docs. Mitigated by keeping the XAML behavior as fallback.
- **Android 15 edge-to-edge transparency** → Status bar is transparent, so if a page has a non-standard background at the top, icons might lack contrast. Mitigated by all pages using theme-consistent backgrounds via `Styles.xaml`.
- **Calling static API at wrong lifecycle point** → Docs warn against calling in constructors. Mitigated by calling from command handlers (user-initiated) and `OnStart`/`OnResume` (lifecycle-safe).
