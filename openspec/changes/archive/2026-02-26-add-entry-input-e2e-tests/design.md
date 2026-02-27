## Context

The UITests project has Appium infrastructure (`AppiumSetup`, `TestConfig`, page objects) and existing test fixtures for settings and auth screen navigation. The `LoginPage` and `RegisterPage` page objects exist but only expose `IsHeadlineVisible()`. The `SearchPage` has no page object yet. All 7 Entry fields lack `AutomationId`.

## Goals / Non-Goals

**Goals:**
- Verify every Entry field in the app accepts typed text via Appium
- Add `AutomationId` to all Entry fields for reliable element location

**Non-Goals:**
- Testing form validation logic (covered by other tests)
- Testing form submission or API calls
- Testing password masking behavior

## Decisions

### 1. AutomationId naming convention

Use descriptive IDs following the pattern `<Page><Field>Entry`:
- LoginPage: `LoginEmailEntry`, `LoginPasswordEntry`
- RegisterPage: `RegisterEmailEntry`, `RegisterPhoneEntry`, `RegisterPasswordEntry`, `RegisterConfirmPasswordEntry`
- SearchPage: `SearchEntry` (already has `x:Name="SearchEntry"`, use same name for AutomationId)

### 2. Test structure: one fixture, ordered by screen

Single `EntryInputTests` fixture inheriting `AppiumSetup`. Tests grouped by screen in order:
1. Search screen entries (navigated from Home tab)
2. Login screen entries (navigated from Profile > Sign In)
3. Register screen entries (navigated from Profile > Create Account)

Each test types a known string, reads the Entry value back, and asserts equality. After each screen's tests, navigate back.

### 3. Clear fields after typing

After asserting the typed value, clear the Entry field to avoid side effects on subsequent tests (e.g., triggering validation on auth forms).

### 4. Extend existing page objects, add SearchPage

- Extend `LoginPage` with `TypeEmail()`, `TypePassword()`, `GetEmailText()`, `GetPasswordText()`, `ClearEmail()`, `ClearPassword()`
- Extend `RegisterPage` (UITests) with similar methods for all 4 fields
- Create new `SearchPage` page object with `TypeSearch()`, `GetSearchText()`, `ClearSearch()`

## Risks / Trade-offs

- **Password fields return masked text on Android** → Use `SendKeys()` and read back via element `.Text` property. On Android with UiAutomator2, `.Text` returns the actual text even for password fields.
- **Search page navigation** → SearchPage is a pushed route, need to navigate via Home tab search bar tap. The existing `HomePage.xaml` has a tappable Border that navigates to SearchPage.
