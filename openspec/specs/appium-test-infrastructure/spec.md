# Appium Test Infrastructure

## Purpose

Defines the Appium UI test project structure, driver lifecycle, page object model, test ordering, and screenshot utilities.

## Requirements

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

### Requirement: Appium driver configuration
The test project SHALL contain a `TestConfig` class that defines Appium desired capabilities for connecting to an Android emulator. The configuration SHALL specify `UiAutomator2` as the automation name, the app package name, and the Appium server URL (`http://127.0.0.1:4723`).

#### Scenario: Configuration targets Android with UIAutomator2
- **WHEN** the `TestConfig` class is used to create an Appium session
- **THEN** the capabilities SHALL include `platformName: Android`, `automationName: UiAutomator2`, and the correct `appPackage` and `appActivity` for the SportowyHub app

### Requirement: Base test class with driver lifecycle
The test project SHALL contain an `AppiumDriverFixture` class implementing `IAsyncLifetime` that manages the Appium driver session lifecycle. The class SHALL create an `AndroidDriver` instance in `InitializeAsync` and dispose of it in `DisposeAsync`. Test classes SHALL receive the fixture via `IClassFixture<AppiumDriverFixture>` to share a single driver session across all tests in the class.

#### Scenario: Driver session created once per test class
- **WHEN** a test class using `IClassFixture<AppiumDriverFixture>` begins execution
- **THEN** a single Appium `AndroidDriver` session SHALL be created before any tests run and disposed after all tests in the class complete

#### Scenario: Driver session handles connection failure gracefully
- **WHEN** the Appium server is not running at the configured URL
- **THEN** the setup SHALL fail with a clear error message indicating the server is unreachable

### Requirement: Test ordering infrastructure
The test project SHALL provide a custom `ITestCaseOrderer` implementation (`PriorityOrderer`) and a `TestPriorityAttribute` that accepts an `int` priority value. Tests SHALL be ordered by ascending priority value. The orderer SHALL be applied globally via an assembly-level `[TestCaseOrderer]` attribute.

#### Scenario: Tests with priority attributes execute in order
- **WHEN** a test class contains methods annotated with `[TestPriority(1)]`, `[TestPriority(2)]`, `[TestPriority(3)]`
- **THEN** xUnit SHALL execute them in ascending priority order: 1, 2, 3

#### Scenario: Tests without priority attribute run after prioritized tests
- **WHEN** a test class contains both prioritized and non-prioritized test methods
- **THEN** prioritized tests SHALL run first in order, followed by non-prioritized tests

### Requirement: Page Object Model for test interactions
The test project SHALL implement page object classes that encapsulate UI element locators and interaction methods. Each page object SHALL locate elements using `MobileBy.Id()` with `AutomationId` values. Page objects SHALL expose high-level action methods (e.g., `SelectLanguage(int index)`, `SelectTheme(int index)`, `NavigateToProfile()`).

#### Scenario: Page objects abstract element locators
- **WHEN** a test calls `profilePage.SelectLanguage(2)`
- **THEN** the page object SHALL locate the language picker by its `AutomationId` and perform the selection interaction

#### Scenario: Page objects use explicit waits
- **WHEN** a page object method interacts with an element
- **THEN** it SHALL use `WebDriverWait` to wait for the element to be present before interacting, with a configurable timeout

### Requirement: Screenshot helper for visual verification
The test project SHALL contain a `ScreenshotHelper` class that captures screenshots via the Appium driver and provides pixel color sampling. The helper SHALL decode the screenshot to a bitmap and return the RGB color value at a given coordinate or region.

#### Scenario: Screenshot captures current screen state
- **WHEN** `ScreenshotHelper.CaptureScreenshot(driver)` is called
- **THEN** it SHALL return a bitmap representation of the current device screen

#### Scenario: Pixel color sampling returns RGB values
- **WHEN** `ScreenshotHelper.GetPixelColor(bitmap, x, y)` is called with valid coordinates
- **THEN** it SHALL return the RGB color value of the pixel at that position
