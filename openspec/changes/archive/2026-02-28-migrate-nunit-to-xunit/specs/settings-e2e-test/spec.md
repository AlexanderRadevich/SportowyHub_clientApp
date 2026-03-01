## MODIFIED Requirements

### Requirement: E2E test runs as a single ordered test fixture
The settings tests SHALL execute as a single xUnit test class implementing `IClassFixture<AppiumDriverFixture>` with `[TestPriority]`-ordered test methods. The execution order SHALL be: (1) navigate to profile, (2) change language and verify, (3) change theme and verify. Tests within the class SHALL share the same Appium driver session via the injected fixture.

#### Scenario: Tests execute in defined order
- **WHEN** the test class runs
- **THEN** the navigation test SHALL run first (TestPriority 1), followed by the language test (TestPriority 2), followed by the theme test (TestPriority 3), using `[TestPriority]` attributes with `PriorityOrderer`

#### Scenario: Test class shares driver session
- **WHEN** all tests in the class execute
- **THEN** they SHALL use the same Appium `AndroidDriver` instance from `AppiumDriverFixture` without restarting the app between tests

### Requirement: Settings E2E tests use xUnit assertions
The `SettingsTests` class SHALL use xUnit assertion methods instead of NUnit constraint-model assertions. Each test method SHALL be annotated with `[Fact]` and `[TestPriority(n)]`.

#### Scenario: Boolean visibility checks use Assert.True
- **WHEN** the test verifies the settings section is visible
- **THEN** it SHALL use `Assert.True(_profile.IsSettingsSectionVisible(), message)`

#### Scenario: String equality checks use Assert.Equal
- **WHEN** the test verifies a label displays expected text (e.g., "Settings", "English", "Dark")
- **THEN** it SHALL use `Assert.Equal(expected, actual)`

#### Scenario: Numeric comparison checks use Assert.True
- **WHEN** the test verifies dark theme RGB values are below a threshold
- **THEN** it SHALL use `Assert.True(value < threshold, message)` for each color component
