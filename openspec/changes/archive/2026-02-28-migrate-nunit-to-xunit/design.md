## Context

The `SportowyHub.UITests` project uses NUnit 4.3.2 with Appium WebDriver for E2E UI testing. The project's CLAUDE.md mandates xUnit v3 as the standard test framework. The test suite is small (4 test classes, 14 tests total) with a shared `AppiumSetup` base class managing the Appium driver lifecycle via `[OneTimeSetUp]`/`[OneTimeTearDown]`. All tests use `[Order(n)]` for deterministic execution order — critical for E2E tests that depend on app state from prior steps.

**Current NUnit patterns in use:**
- `[TestFixture]` on all test classes
- `[OneTimeSetUp]` in `AppiumSetup` (driver init) and in each test class (page object init)
- `[OneTimeTearDown]` in `AppiumSetup` (driver quit)
- `[Test, Order(n)]` on all test methods
- `Assert.That()` with constraint model (`Is.True`, `Is.EqualTo`, `Is.LessThan`, `Has.Length.EqualTo`)

## Goals / Non-Goals

**Goals:**
- Replace NUnit with xUnit v3 across the entire test project
- Preserve test ordering behavior (critical for sequential E2E flows)
- Preserve one-driver-per-fixture lifecycle (driver created once, shared across all tests in a class)
- Keep the Page Object Model and helper classes unchanged (they have no NUnit dependency)

**Non-Goals:**
- Adding unit tests or changing test coverage
- Changing the Appium WebDriver version or configuration
- Refactoring Page Objects or helper classes beyond what the migration requires
- Migrating to a different assertion library (e.g., FluentAssertions)

## Decisions

### 1. Driver lifecycle: `IClassFixture<T>` with `IAsyncLifetime`

**Choice:** Create an `AppiumDriverFixture` class implementing `IAsyncLifetime` that manages the driver session. Test classes receive it via `IClassFixture<AppiumDriverFixture>`.

**Why over alternatives:**
- xUnit has no `[OneTimeSetUp]`/`[OneTimeTearDown]`. The idiomatic replacement is class fixtures.
- `IClassFixture<T>` gives one shared instance per test class — matching NUnit's `[OneTimeSetUp]` behavior.
- `IAsyncLifetime` allows async setup/teardown (future-proofing for async Appium operations).

**Alternative considered:** Using constructor/`IDisposable` directly on each test class. Rejected because it would create a new driver per test method, not per class.

### 2. Test ordering: Custom `ITestCaseOrderer`

**Choice:** Implement a custom `PriorityOrderer` that reads a `[TestPriority(n)]` attribute to order tests within each class.

**Why:** xUnit intentionally randomizes test order by default. E2E Appium tests are stateful — e.g., login must happen before sign-out. A custom orderer with an explicit attribute is the standard xUnit approach and makes ordering intent visible.

**Implementation:**
- `TestPriorityAttribute` — simple attribute storing an `int Priority`
- `PriorityOrderer` — implements `ITestCaseOrderer`, sorts by priority value
- Assembly-level `[TestCaseOrderer]` attribute to apply globally

### 3. Per-class `[OneTimeSetUp]` replacement: Constructor injection

**Choice:** Each test class's `[OneTimeSetUp]` (which initializes page objects) moves to the constructor, receiving the `AppiumDriverFixture` via `IClassFixture<T>`.

**Why:** xUnit uses constructors for per-class setup. Since page object initialization is synchronous and lightweight, the constructor is the natural fit.

### 4. Assertion mapping

Direct 1:1 mapping from NUnit constraint model to xUnit assertions:

| NUnit | xUnit |
|-------|-------|
| `Assert.That(x, Is.True, msg)` | `Assert.True(x, msg)` |
| `Assert.That(x, Is.EqualTo(y), msg)` | `Assert.Equal(y, x)` |
| `Assert.That(x, Is.LessThan(y), msg)` | `Assert.True(x < y, msg)` |
| `Assert.That(x, Has.Length.EqualTo(y), msg)` | `Assert.Equal(y, x.Length)` |
| `Assert.Fail(msg)` | `Assert.Fail(msg)` |

**Note:** `Assert.Equal` in xUnit does not accept a message parameter. For assertions where the message is important for debugging, use `Assert.True(condition, message)` as the fallback pattern.

### 5. Package changes

| Remove | Add |
|--------|-----|
| `NUnit` 4.3.2 | `xunit` (latest v3) |
| `NUnit3TestAdapter` 4.6.0 | `xunit.runner.visualstudio` (latest) |
| | `xunit.analyzers` (latest, for best practices) |

`Microsoft.NET.Test.Sdk`, `Appium.WebDriver`, and `SkiaSharp` remain unchanged.

## Risks / Trade-offs

**[Test ordering fragility]** → xUnit discourages ordered tests. Mitigation: The custom orderer is simple and well-documented. E2E tests inherently need ordering; this is an accepted trade-off.

**[Assert.Equal has no message overload]** → Some NUnit assertions include diagnostic messages. Mitigation: Use `Assert.True(condition, message)` where the message is critical for debugging test failures. For simple equality checks, the default xUnit failure output is sufficient.

**[Driver lifecycle timing]** → `IClassFixture<T>` creates the fixture when the first test in the class runs and disposes after the last. This matches NUnit's `[OneTimeSetUp]`/`[OneTimeTearDown]` behavior, so no timing risk.

## Migration Plan

1. Update `.csproj` — swap packages
2. Create `AppiumDriverFixture` with `IAsyncLifetime`
3. Create `TestPriorityAttribute` and `PriorityOrderer`
4. Migrate `AppiumSetup` → delete or repurpose as the fixture
5. Migrate each test class (4 files) — attributes, assertions, constructor injection
6. Update `ToastHelper.AssertNoErrorToast` if it uses any NUnit assertions internally
7. Run `dotnet build` to verify compilation
8. Run `dotnet test` against live Appium session to verify all 14 tests pass
9. Update the 5 affected OpenSpec specs to reflect xUnit terminology
