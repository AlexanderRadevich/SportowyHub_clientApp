## MODIFIED Requirements

### Requirement: Appium test project structure
The solution SHALL contain a separate test project `SportowyHub.UITests/SportowyHub.UITests.csproj` targeting `net10.0`. The project SHALL reference `Appium.WebDriver` (8.x), `xunit` (v3), `xunit.runner.visualstudio`, `xunit.analyzers`, and `Microsoft.NET.Test.Sdk`. The project SHALL NOT reference or depend on the main MAUI app project.

#### Scenario: Test project builds independently
- **WHEN** the developer runs `dotnet build` on the test project
- **THEN** the project SHALL compile successfully without requiring the MAUI app to be built first

#### Scenario: Test project targets plain .NET
- **WHEN** the test project `.csproj` is inspected
- **THEN** the `TargetFramework` SHALL be `net10.0` (not a MAUI workload target)

#### Scenario: Test project uses xUnit packages
- **WHEN** the test project `.csproj` is inspected
- **THEN** it SHALL reference `xunit`, `xunit.runner.visualstudio`, and `xunit.analyzers` and SHALL NOT reference `NUnit` or `NUnit3TestAdapter`

### Requirement: Base test class with driver lifecycle
The test project SHALL contain an `AppiumDriverFixture` class implementing `IAsyncLifetime` that manages the Appium driver session lifecycle. The class SHALL create an `AndroidDriver` instance in `InitializeAsync` and dispose of it in `DisposeAsync`. Test classes SHALL receive the fixture via `IClassFixture<AppiumDriverFixture>` to share a single driver session across all tests in the class.

#### Scenario: Driver session created once per test class
- **WHEN** a test class using `IClassFixture<AppiumDriverFixture>` begins execution
- **THEN** a single Appium `AndroidDriver` session SHALL be created before any tests run and disposed after all tests in the class complete

#### Scenario: Driver session handles connection failure gracefully
- **WHEN** the Appium server is not running at the configured URL
- **THEN** the setup SHALL fail with a clear error message indicating the server is unreachable

## ADDED Requirements

### Requirement: Test ordering infrastructure
The test project SHALL provide a custom `ITestCaseOrderer` implementation (`PriorityOrderer`) and a `TestPriorityAttribute` that accepts an `int` priority value. Tests SHALL be ordered by ascending priority value. The orderer SHALL be applied globally via an assembly-level `[TestCaseOrderer]` attribute.

#### Scenario: Tests with priority attributes execute in order
- **WHEN** a test class contains methods annotated with `[TestPriority(1)]`, `[TestPriority(2)]`, `[TestPriority(3)]`
- **THEN** xUnit SHALL execute them in ascending priority order: 1, 2, 3

#### Scenario: Tests without priority attribute run after prioritized tests
- **WHEN** a test class contains both prioritized and non-prioritized test methods
- **THEN** prioritized tests SHALL run first in order, followed by non-prioritized tests
