## Context

The test project (`SportowyHub.UITests/`) uses Appium + NUnit with a page object pattern. Existing tests cover auth screen navigation (open/close login and register pages) and input field acceptance, but no test performs an actual login against the backend or exercises the sign-out flow. The app now has error toast notifications (Snackbar via CommunityToolkit.Maui) that should not appear during successful flows.

Key constraints:
- Tests run on Android via UiAutomator2 — element lookup uses `MobileBy.Id()` (AutomationId) and `MobileBy.AccessibilityId()` (content descriptions)
- The app uses `noReset: true`, so state persists between test classes — tests must clean up after themselves
- The Login button currently has no `AutomationId`
- Sign-out shows a native `DisplayAlertAsync` confirmation dialog
- Tab navigation uses localized content descriptions (4 languages)

## Goals / Non-Goals

**Goals:**
- Verify the full login flow: enter credentials → submit → land on Home tab
- Verify the full sign-out flow: navigate to Account Profile → tap Sign Out → confirm → return to Profile in logged-out state
- Assert no error toasts (Snackbar) appear during either flow
- Keep test credentials configurable (not hard-coded in test methods)

**Non-Goals:**
- Testing invalid login (wrong credentials, validation errors) — separate change
- Testing login persistence across app restarts
- Testing the registration flow end-to-end
- Testing network failure scenarios

## Decisions

### 1. Test credentials in `TestConfig`

Add `TestEmail` and `TestPassword` constants to `TestConfig.cs`. This keeps credentials centralized and easy to change per environment.

**Alternative considered:** Environment variables or a JSON config file. Rejected — the project uses simple constants for all config, and env vars add complexity for a test project that runs locally.

### 2. Toast assertion via short-wait element check

Create an `AssertNoErrorToast()` helper that uses a short explicit wait (2 seconds) to check for Snackbar elements. On Android, CommunityToolkit.Maui Snackbar renders as a `com.google.android.material.snackbar.Snackbar` view inside a `SnackbarBaseLayout`. We can search for this by class name or by the text content.

Approach: Use `FindElements` (plural) with a short timeout — if the collection is empty, no toast is present. This avoids failing on `NoSuchElementException`.

**Alternative considered:** Screenshot-based color sampling. Rejected — fragile, position-dependent, and the existing `ScreenshotHelper` is meant for theme testing, not toast detection.

### 3. Single test class with ordered tests

Create `LoginSignOutTests` as one test class with `[Test, Order(n)]` attributes. Test 1 logs in (leaves the app on Home in logged-in state), Test 2 signs out (returns to logged-out state). This matches the existing test pattern and ensures the app state is clean at the end.

**Alternative considered:** Two separate test classes. Rejected — the sign-out test depends on being logged in, and using separate classes with `[OneTimeSetUp]` per class would require logging in again or managing shared state.

### 4. Native dialog interaction for sign-out confirmation

The sign-out confirmation uses `DisplayAlertAsync`, which renders as an Android `AlertDialog`. We interact with it by finding buttons by text using `MobileBy.XPath` with localized button text patterns (trying multiple languages), similar to how `NavigateToTabByNames` handles localized tabs.

### 5. Add `AutomationId="LoginButton"` to LoginPage.xaml

The Login button needs an AutomationId for reliable test targeting. This is a minimal app-side change.

## Risks / Trade-offs

- **[Backend dependency]** Tests require a running backend with the test account. → Mitigation: Document prerequisite in test class comments; credentials are configurable via `TestConfig`.
- **[Snackbar detection may be flaky]** The Snackbar auto-dismisses after 6 seconds and may not be found if it appeared and disappeared before the check. → Mitigation: Assert immediately after the action completes; use a 2-second window which is sufficient since we check right after navigation.
- **[Test ordering dependency]** Test 2 (sign-out) depends on Test 1 (login) having succeeded. → Mitigation: NUnit `[Order]` guarantees execution order within a class; if Test 1 fails, Test 2 will fail too, which is the expected behavior.
- **[Localized dialog buttons]** Sign-out confirmation button text varies by language. → Mitigation: Try multiple localized strings, same pattern as tab navigation.
