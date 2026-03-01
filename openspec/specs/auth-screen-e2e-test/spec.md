### Requirement: E2E test navigates to Login screen from Profile
The test SHALL navigate to the Profile tab, tap the "Sign In" row (located by `AutomationId` `SignInRow`), and verify that the Login page opens by asserting the headline label (`AutomationId` `LoginHeadline`) is visible. After verification the test SHALL navigate back to the Profile tab using the Android back button.

#### Scenario: Login screen opens from Profile Sign In row
- **WHEN** the test navigates to the Profile tab and taps the element with `AutomationId` `SignInRow`
- **THEN** the Login page SHALL be displayed with the headline label (`AutomationId` `LoginHeadline`) visible

#### Scenario: Test navigates back to Profile after verifying Login screen
- **WHEN** the Login page headline has been verified
- **THEN** the test SHALL press the Android back button and the Profile tab content SHALL be visible again

### Requirement: E2E test navigates to Registration screen from Profile
The test SHALL navigate to the Profile tab, tap the "Create Account" row (located by `AutomationId` `CreateAccountRow`), and verify that the Registration page opens by asserting the headline label (`AutomationId` `RegisterHeadline`) is visible. After verification the test SHALL navigate back to the Profile tab using the Android back button.

#### Scenario: Registration screen opens from Profile Create Account row
- **WHEN** the test navigates to the Profile tab and taps the element with `AutomationId` `CreateAccountRow`
- **THEN** the Registration page SHALL be displayed with the headline label (`AutomationId` `RegisterHeadline`) visible

#### Scenario: Test navigates back to Profile after verifying Registration screen
- **WHEN** the Registration page headline has been verified
- **THEN** the test SHALL press the Android back button and the Profile tab content SHALL be visible again

### Requirement: Auth screen tests use Page Object Model
The test project SHALL contain `LoginPage` and `RegisterPage` page object classes following the existing POM pattern. Each page object SHALL accept an `AndroidDriver` in its constructor, use `WebDriverWait` with `TestConfig.DefaultWaitSeconds`, and locate elements via `MobileBy.Id()` with `AutomationId` values. Each page object SHALL expose an `IsHeadlineVisible()` method that returns `true` if the headline element is found within the wait timeout.

#### Scenario: LoginPage page object locates headline
- **WHEN** `LoginPage.IsHeadlineVisible()` is called while the Login page is displayed
- **THEN** it SHALL return `true` by finding the element with `AutomationId` `LoginHeadline`

#### Scenario: RegisterPage page object locates headline
- **WHEN** `RegisterPage.IsHeadlineVisible()` is called while the Registration page is displayed
- **THEN** it SHALL return `true` by finding the element with `AutomationId` `RegisterHeadline`

### Requirement: Auth screen tests run as a single ordered test fixture
The `AuthScreenTests` class SHALL implement `IClassFixture<AppiumDriverFixture>` and use `[TestPriority]` attributes for ordering. The execution order SHALL be: (1) Login screen opens, (2) Registration screen opens. Tests within the class SHALL share the same Appium driver session via the injected fixture.

#### Scenario: Tests execute in defined order
- **WHEN** the `AuthScreenTests` class runs
- **THEN** the Login screen test SHALL run first (TestPriority 1), followed by the Registration screen test (TestPriority 2)

#### Scenario: Test class shares driver session
- **WHEN** all tests in the class execute
- **THEN** they SHALL use the same Appium `AndroidDriver` instance from `AppiumDriverFixture` without restarting the app between tests

### Requirement: Auth screen tests use xUnit assertions
The `AuthScreenTests` class SHALL use xUnit `Assert.True()` for boolean visibility checks instead of NUnit `Assert.That(..., Is.True)`. Each test method SHALL be annotated with `[Fact]` and `[TestPriority(n)]`.

#### Scenario: Login screen visibility assertion uses xUnit
- **WHEN** the test verifies the Login page headline is visible
- **THEN** it SHALL use `Assert.True(_login.IsHeadlineVisible(), message)`

#### Scenario: Registration screen visibility assertion uses xUnit
- **WHEN** the test verifies the Registration page headline is visible
- **THEN** it SHALL use `Assert.True(_register.IsHeadlineVisible(), message)`

### Requirement: ProfilePage page object exposes auth row tap methods
The existing `ProfilePage` page object SHALL be extended with `TapSignIn()` and `TapCreateAccount()` methods that locate and tap the Sign In and Create Account rows by their `AutomationId` values (`SignInRow`, `CreateAccountRow`).

#### Scenario: TapSignIn locates and taps the Sign In row
- **WHEN** `ProfilePage.TapSignIn()` is called while the Profile page is displayed
- **THEN** it SHALL find the element with `AutomationId` `SignInRow` and tap it

#### Scenario: TapCreateAccount locates and taps the Create Account row
- **WHEN** `ProfilePage.TapCreateAccount()` is called while the Profile page is displayed
- **THEN** it SHALL find the element with `AutomationId` `CreateAccountRow` and tap it
