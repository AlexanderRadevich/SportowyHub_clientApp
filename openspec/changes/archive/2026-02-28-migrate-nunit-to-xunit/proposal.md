## Why

The project's CLAUDE.md specifies **xUnit v3** as the testing standard, but the existing UI test project (`SportowyHub.UITests`) uses **NUnit 4.3.2**. Aligning the test framework eliminates the mismatch between documented conventions and actual code, and positions the project for future unit test additions that follow the same framework.

## What Changes

- **BREAKING**: Replace all NUnit packages (`NUnit`, `NUnit3TestAdapter`) with xUnit equivalents (`xunit`, `xunit.runner.visualstudio`)
- Replace NUnit attributes with xUnit equivalents across all test files:
  - `[TestFixture]` → removed (xUnit doesn't require it)
  - `[Test]` → `[Fact]`
  - `[Test, Order(n)]` → custom `[Fact]` with `ITestCaseOrderer` or `[Theory]` as needed
  - `[OneTimeSetUp]` → `IAsyncLifetime.InitializeAsync` or constructor
  - `[OneTimeTearDown]` → `IAsyncLifetime.DisposeAsync` or `IDisposable`
- Replace `Assert.That()` constraint-model assertions with xUnit `Assert.*` methods
- Update `AppiumSetup` base class to use xUnit lifecycle (`IAsyncLifetime` or class fixture)
- Preserve test ordering capability (NUnit `[Order]` → xUnit test case orderer)

## Capabilities

### New Capabilities

_None — this is a framework migration, not a new capability._

### Modified Capabilities

- `appium-test-infrastructure`: Base class lifecycle changes from NUnit `[OneTimeSetUp]`/`[OneTimeTearDown]` to xUnit `IAsyncLifetime`. Package references change from NUnit to xUnit.
- `auth-screen-e2e-test`: Test attributes and assertions migrate from NUnit to xUnit syntax.
- `entry-input-e2e-test`: Test attributes and assertions migrate from NUnit to xUnit syntax.
- `login-signout-e2e-test`: Test attributes and assertions migrate from NUnit to xUnit syntax.
- `settings-e2e-test`: Test attributes and assertions migrate from NUnit to xUnit syntax.

## Impact

- **Project file**: `SportowyHub.UITests.csproj` — swap NUnit packages for xUnit packages
- **Base class**: `Helpers/AppiumSetup.cs` — rewrite lifecycle management
- **All test files** (4 files): `Tests/AuthScreenTests.cs`, `Tests/EntryInputTests.cs`, `Tests/LoginSignOutTests.cs`, `Tests/SettingsTests.cs` — attribute and assertion changes
- **Test execution**: `dotnet test` command remains the same; no CI/CD pipeline changes needed
- **Test ordering**: Requires a custom `ITestCaseOrderer` implementation to replace NUnit `[Order(n)]`
