## MODIFIED Requirements

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
