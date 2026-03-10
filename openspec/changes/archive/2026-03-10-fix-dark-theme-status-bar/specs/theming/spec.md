## ADDED Requirements

### Requirement: Status bar visibility across themes
The system SHALL ensure the phone status bar (battery, clock, signal icons) remains visible in both light and dark themes. The status bar style SHALL be set to `DarkContent` (dark icons) in light theme and `LightContent` (light icons) in dark theme. The status bar color SHALL match the theme's `Background` color (`#FFFFFF` for light, `#121212` for dark). On platforms where the status bar is transparent (Android 15+ edge-to-edge), the page background SHALL provide sufficient contrast for the status bar icons.

#### Scenario: Status bar icons visible in light theme
- **WHEN** the app is in light theme
- **THEN** the status bar style SHALL be `DarkContent` and the status bar color SHALL be `#FFFFFF`

#### Scenario: Status bar icons visible in dark theme
- **WHEN** the app is in dark theme
- **THEN** the status bar style SHALL be `LightContent` and the status bar color SHALL be `#121212`

#### Scenario: Status bar updates after programmatic theme toggle
- **WHEN** the user toggles the theme via the home screen toggle button (without AppShell reconstruction)
- **THEN** the status bar style and color SHALL update immediately to match the new theme

#### Scenario: Status bar correct after AppShell reconstruction
- **WHEN** the theme changes via the profile settings picker (which reconstructs AppShell)
- **THEN** the status bar style and color SHALL be correct for the newly selected theme

#### Scenario: Status bar correct on app startup
- **WHEN** the app launches with a persisted theme preference
- **THEN** the status bar style and color SHALL match the persisted theme before any user interaction

#### Scenario: Status bar on Android 15+ edge-to-edge
- **WHEN** running on Android 15+ where the status bar is transparent
- **THEN** the status bar style SHALL still be set correctly and the page background SHALL provide contrast for icon visibility

### Requirement: iOS status bar control configuration
The iOS `Info.plist` SHALL include `UIViewControllerBasedStatusBarAppearance` set to `false` to allow CommunityToolkit.Maui `StatusBarBehavior` and static `StatusBar` APIs to control the status bar appearance.

#### Scenario: iOS Info.plist contains status bar entry
- **WHEN** the iOS app is built
- **THEN** `Info.plist` SHALL contain `UIViewControllerBasedStatusBarAppearance` with value `false`

### Requirement: Centralized status bar application
A helper method SHALL exist to apply the correct status bar color and style for a given theme. Both theme-switching paths (home toggle and profile picker) and app startup SHALL use this helper to ensure consistent behavior.

#### Scenario: Home toggle uses centralized helper
- **WHEN** the user toggles theme from the home screen
- **THEN** the centralized status bar helper SHALL be invoked with the new theme

#### Scenario: Profile picker uses centralized helper
- **WHEN** the user changes theme from the profile settings
- **THEN** the centralized status bar helper SHALL be invoked with the new theme

#### Scenario: App startup uses centralized helper
- **WHEN** the app starts and applies the persisted theme
- **THEN** the centralized status bar helper SHALL be invoked with the loaded theme
