## MODIFIED Requirements

### Requirement: Login E2E test
The `LoginSignOutTests` class SHALL implement `IClassFixture<AppiumDriverFixture>` and contain a test (`[Fact]`, `[TestPriority(1)]`) that performs a full login flow: navigate to Profile tab, tap Sign In, enter `TestConfig.TestEmail` and `TestConfig.TestPassword`, tap Login, wait for navigation, assert the Home tab is active, and assert no error toast appeared. Assertions SHALL use xUnit `Assert.True()`.

#### Scenario: Successful login navigates to Home
- **WHEN** the test enters valid credentials and taps Login
- **THEN** `Assert.True` SHALL verify the Home tab is active
- **AND** `ToastHelper.AssertNoErrorToast` SHALL verify no error toast is displayed

#### Scenario: Login test starts from logged-out state
- **WHEN** the test begins
- **THEN** `Assert.True(_profile.IsSignInRowVisible())` SHALL confirm the Profile page shows the Sign In row

### Requirement: Sign-out E2E test
The `LoginSignOutTests` class SHALL contain a test (`[Fact]`, `[TestPriority(2)]`) that performs a full sign-out flow: navigate to Profile tab, tap Account Profile, tap Sign Out, confirm the dialog, wait for navigation back to Profile, assert the Sign In row is visible, and assert no error toast appeared. Assertions SHALL use xUnit `Assert.True()`.

#### Scenario: Successful sign-out returns to logged-out profile
- **WHEN** the test taps Sign Out and confirms the dialog
- **THEN** `Assert.True(_profile.IsSignInRowVisible())` SHALL verify the Sign In row is visible
- **AND** `ToastHelper.AssertNoErrorToast` SHALL verify no error toast is displayed

#### Scenario: Sign-out test runs after login
- **WHEN** the sign-out test begins
- **THEN** the app SHALL be in logged-in state (from the preceding login test via `[TestPriority]` ordering)

### Requirement: Toast assertion helper uses xUnit
The `ToastHelper.AssertNoErrorToast` method SHALL use xUnit `Assert.Fail()` instead of NUnit `Assert.Fail()` when a Snackbar error toast is detected.

#### Scenario: Error toast present — xUnit assertion fails
- **WHEN** `AssertNoErrorToast()` is called and a Snackbar with error text is visible
- **THEN** `Assert.Fail(message)` from `Xunit` namespace SHALL be called with a descriptive message
