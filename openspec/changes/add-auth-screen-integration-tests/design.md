## Context

The UITests project already has a working Appium infrastructure (`AppiumSetup`, `TestConfig`, `AppShellPage`, `ProfilePage`) and one test fixture (`SettingsTests`) that validates language/theme changes. The auth screens (LoginPage, RegisterPage) are registered as Shell routes (`"login"`, `"register"`) and navigated to from the Profile tab's "Sign In" and "Create Account" rows via `TapGestureRecognizer` on Grid elements. Currently neither the Profile account rows nor the auth page elements carry `AutomationId` attributes, so Appium cannot reliably locate them.

## Goals / Non-Goals

**Goals:**
- Two Appium tests that verify Login and Register screens open from the Profile tab
- Add `AutomationId` attributes to the Profile account rows and auth page headline labels so tests can locate them
- Follow the existing Page Object Model pattern

**Non-Goals:**
- Testing actual login/registration API flows (form submission, validation, error handling)
- Testing email verification flow
- Adding AutomationId to every element on auth pages (only what's needed for these two tests)

## Decisions

### 1. Element location strategy: AutomationId on container Grids

Add `AutomationId` to the Sign In and Create Account Grid rows in ProfilePage.xaml (`SignInRow`, `CreateAccountRow`) rather than trying to locate the inner Label by its localized text. This avoids fragile text-based lookups that break across languages.

**Alternative considered:** Using `MobileBy.AccessibilityId` with localized text (like `AppShellPage.NavigateToTabByNames`). Rejected because auth row labels are localized and we'd need to maintain multiple language variants.

### 2. Verify page opened via headline label AutomationId

Add `AutomationId="LoginHeadline"` to the headline Label on LoginPage.xaml and `AutomationId="RegisterHeadline"` to RegisterPage.xaml. Tests assert the headline is visible after navigation, confirming the correct page rendered.

**Alternative considered:** Checking Shell `CurrentPage` type via a custom Appium command. Rejected because Appium operates at the UI layer — checking a visible element is the standard approach and more robust.

### 3. New page objects: `LoginPage` and `RegisterPage`

Create minimal page objects with only the methods needed now (find headline, check visibility). They follow the existing pattern (`AndroidDriver` + `WebDriverWait` in constructor, `MobileBy.Id()` for locators).

**Alternative considered:** Extending `ProfilePage` with auth navigation methods. Rejected because auth pages are separate screens with their own elements — they deserve their own page objects per the existing POM convention.

### 4. Back navigation after each test

After verifying each auth screen opened, navigate back to Profile using the Android back button (`driver.Navigate().Back()`). This keeps tests independent — test 2 doesn't depend on the state left by test 1.

### 5. Single test fixture, ordered tests

Use one `AuthScreenTests` fixture inheriting `AppiumSetup`, with `[Order]` attributes matching the existing `SettingsTests` pattern. Order: (1) Login screen opens, (2) Register screen opens.

## Risks / Trade-offs

- **TapGestureRecognizer on Grid may not be reachable by Appium** → The `AutomationId` on the Grid container should make it tappable via `MobileBy.Id()`. If the tap doesn't register, fallback is to use coordinate-based tap on the element's center.
- **Shell navigation timing** → After tapping a row, the page push animation takes time. Mitigated by using `WebDriverWait` to wait for the headline element on the target page.
