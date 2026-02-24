## Why

The app has no automated test infrastructure. Manual verification of settings behavior (language switching, theme switching, icon color changes) is time-consuming and error-prone, especially across multiple languages and themes. Appium UI automation tests will provide repeatable regression coverage for these critical user-facing flows.

## What Changes

- Add an Appium UI test project alongside the main .NET MAUI app
- Add `AutomationId` attributes to key UI elements (pickers, labels, tab items) so Appium can locate them reliably
- Implement a first E2E test scenario that:
  1. Launches the app
  2. Navigates to the Profile tab (settings screen)
  3. Changes the language via the language picker and verifies UI text updates
  4. Changes the theme via the theme picker and verifies UI colors/icon tints update
- Add test configuration for running against an Android emulator via Appium

## Capabilities

### New Capabilities
- `appium-test-infrastructure`: Appium project setup, driver configuration, base test class, and build/run scripts for executing UI tests against the Android app
- `settings-e2e-test`: End-to-end test covering the language change and theme change flows on the Profile/Settings screen, including icon color verification

### Modified Capabilities
- `profile-hub`: Add `AutomationId` attributes to settings UI elements (language picker, theme picker, section labels) to enable test automation targeting
- `shell-navigation`: Add `AutomationId` attributes to tab bar items so tests can navigate between tabs

## Impact

- **New files**: Appium test project (`.csproj`, test classes, configuration, base helpers)
- **Modified XAML**: `ProfilePage.xaml` and `AppShell.xaml` gain `AutomationId` attributes (no behavioral change)
- **Dependencies**: New test project references (Appium.WebDriver, NUnit or xUnit, etc.) — isolated to the test project, no impact on the main app bundle
- **CI consideration**: Tests require an Android emulator and Appium server to run; not yet integrated into CI pipeline
