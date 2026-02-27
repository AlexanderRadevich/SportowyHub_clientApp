## Why

The app has E2E tests for auth screen navigation and input fields, but no tests that actually perform a real login against the backend or verify the sign-out flow. We need integration-level E2E tests that confirm the full login-to-home and profile-sign-out journeys work end-to-end, including verifying no error toasts appear during the flows.

## What Changes

- Add configurable test credentials (`TestEmail`, `TestPassword`) to `TestConfig` so they are not hard-coded in test methods
- Add an `AutomationId="LoginButton"` to the Login page button so tests can tap it reliably
- Add helper methods to `LoginPage` page object: `TapLogin()` to submit the form
- Add helper methods to `ProfilePage` page object: `TapAccountProfile()` to open the account profile, `IsSignInRowVisible()` to verify logged-out state
- Create a new `AccountProfilePage` page object with `TapSignOut()` and helpers for the native confirmation dialog
- Add a toast/snackbar assertion helper to verify no error toasts are displayed
- Create a new `LoginSignOutTests` test class with two ordered tests:
  1. **Login and navigate to Home**: Profile → Sign In → enter credentials → tap Login → assert Home tab is active and no error toast appeared
  2. **Sign out from Account Profile**: Home → Profile → Account Profile → Sign Out → confirm dialog → assert signed-out state (SignInRow visible) and no error toast appeared

## Capabilities

### New Capabilities
- `login-signout-e2e-test`: E2E test scenarios for login and sign-out flows, including test config, page object additions, toast assertion helper, and the test class

### Modified Capabilities
_(none — existing specs are not changing, only new test code is added)_

## Impact

- **Test project**: `SportowyHub.UITests/` — new test class, new page object, updated page objects and config
- **App project**: `SportowyHub.App/Views/Auth/LoginPage.xaml` — add `AutomationId` to Login button
- **Dependencies**: No new packages required
- **Backend**: Tests require a running backend with a valid test account (`alex10@gmail.com` / `qwerty12345`)
