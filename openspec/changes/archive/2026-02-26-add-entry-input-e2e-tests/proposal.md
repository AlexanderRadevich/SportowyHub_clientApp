## Why

There are no E2E tests verifying that users can actually type into text fields. All 7 Entry fields across the app lack `AutomationId` attributes, making them untestable via Appium. We need basic smoke tests confirming every Entry accepts input.

## What Changes

- Add `AutomationId` to all 7 Entry fields across LoginPage, RegisterPage, and SearchPage
- Add Appium E2E tests that navigate to each screen, tap each Entry, type text, and assert the entered value is present
- Extend existing page objects (`LoginPage`, `RegisterPage`) and create a `SearchPage` page object with text entry methods

## Capabilities

### New Capabilities
- `entry-input-e2e-test`: E2E tests verifying text input works on all Entry fields across Login, Registration, and Search screens

### Modified Capabilities
- `auth-screens`: Add `AutomationId` attributes to Entry fields on LoginPage and RegisterPage
- `search-ui`: Add `AutomationId` to the SearchEntry field on SearchPage

## Impact

- **XAML**: Add `AutomationId` to 7 Entry fields (LoginPage: 2, RegisterPage: 4, SearchPage: 1)
- **Test project**: New test fixture `EntryInputTests.cs`, new `SearchPage` page object, extend `LoginPage`/`RegisterPage` page objects
- **No behavioral changes**: Pure test infrastructure addition
