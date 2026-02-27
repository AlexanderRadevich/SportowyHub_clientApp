## Why

The existing Appium UI test suite covers settings (language/theme) but has no tests for the authentication flow. We need basic smoke tests that verify the Login and Registration screens can be reached from the Profile tab, ensuring navigation routing and page rendering work end-to-end on a real device.

## What Changes

- Add a new `AuthScreenTests` test fixture in the UITests project with two ordered tests:
  1. Tap "Sign In" on the Profile tab → verify LoginPage opens (headline label visible)
  2. Tap "Create Account" on the Profile tab → verify RegisterPage opens (headline label visible)
- Add an `AuthPage` page object (or extend existing page objects) to locate auth screen elements by `AutomationId`
- Add `AutomationId` attributes to key elements on LoginPage.xaml and RegisterPage.xaml (headline labels, buttons) so Appium can find them

## Capabilities

### New Capabilities
- `auth-screen-e2e-test`: E2E integration tests that verify the Login and Registration screens open correctly from the Profile tab

### Modified Capabilities
- `auth-screens`: Add `AutomationId` attributes to key elements (headline labels) so they can be located by Appium tests

## Impact

- **Test project**: New test file `SportowyHub.UITests/Tests/AuthScreenTests.cs`, new page object(s)
- **XAML**: Minor additions of `AutomationId` to LoginPage.xaml and RegisterPage.xaml
- **No API/backend changes**: Tests only verify navigation and screen rendering, no auth API calls needed
- **Dependencies**: Uses existing Appium infrastructure (`AppiumSetup`, `AppShellPage`, `TestConfig`)
