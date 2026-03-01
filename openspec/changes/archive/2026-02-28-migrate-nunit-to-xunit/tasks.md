## 1. Package Migration

- [x] 1.1 Replace NUnit packages with xUnit in `SportowyHub.UITests.csproj`: remove `NUnit` and `NUnit3TestAdapter`, add `xunit`, `xunit.runner.visualstudio`, and `xunit.analyzers`
- [x] 1.2 Run `dotnet restore` and `dotnet build` to verify packages resolve

## 2. Test Ordering Infrastructure

- [x] 2.1 Create `TestPriorityAttribute` class in `Helpers/` — an attribute accepting an `int` priority value
- [x] 2.2 Create `PriorityOrderer` class in `Helpers/` — implements `ITestCaseOrderer`, sorts test cases by ascending `TestPriority` value
- [x] 2.3 Add assembly-level `[TestCaseOrderer]` attribute in a `Properties/AssemblyInfo.cs` or test project root

## 3. Driver Lifecycle Migration

- [x] 3.1 Create `AppiumDriverFixture` class in `Helpers/` — implements `IAsyncLifetime`, moves driver creation from `AppiumSetup.OneTimeSetUp` to `InitializeAsync` and driver quit to `DisposeAsync`
- [x] 3.2 Delete or remove the old `AppiumSetup` base class

## 4. Test Class Migration

- [x] 4.1 Migrate `AuthScreenTests` — remove `[TestFixture]` and `AppiumSetup` inheritance, add `IClassFixture<AppiumDriverFixture>`, replace `[OneTimeSetUp]` with constructor, replace `[Test, Order(n)]` with `[Fact, TestPriority(n)]`, convert `Assert.That(..., Is.True)` to `Assert.True()`
- [x] 4.2 Migrate `EntryInputTests` — same pattern as 4.1, convert `Assert.That(..., Is.EqualTo())` to `Assert.Equal()`, convert `Assert.That(..., Has.Length.EqualTo())` to `Assert.Equal(expected, actual.Length)`
- [x] 4.3 Migrate `LoginSignOutTests` — same pattern as 4.1, convert assertions to `Assert.True()`
- [x] 4.4 Migrate `SettingsTests` — same pattern as 4.1, convert `Assert.That(..., Is.EqualTo())` to `Assert.Equal()`, convert `Assert.That(..., Is.LessThan())` to `Assert.True(x < y, msg)`

## 5. Helper Migration

- [x] 5.1 Update `ToastHelper.AssertNoErrorToast` — replace `using NUnit.Framework` with `using Xunit`, replace `Assert.Fail()` with xUnit `Assert.Fail()`

## 6. Verification

- [x] 6.1 Run `dotnet build` on the test project and verify zero errors
- [ ] 6.2 Run `dotnet test` against a live Appium session and verify all 14 tests pass
- [x] 6.3 Run `get_diagnostics` via MCP to check for warnings
