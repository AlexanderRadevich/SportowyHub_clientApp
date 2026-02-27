## 1. Add AutomationId attributes to XAML pages

- [x] 1.1 Add `AutomationId="SignInRow"` to the Sign In Grid in `SportowyHub.App/Views/Profile/ProfilePage.xaml`
- [x] 1.2 Add `AutomationId="CreateAccountRow"` to the Create Account Grid in `SportowyHub.App/Views/Profile/ProfilePage.xaml`
- [x] 1.3 Add `AutomationId="LoginHeadline"` to the headline Label in `SportowyHub.App/Views/Auth/LoginPage.xaml`
- [x] 1.4 Add `AutomationId="RegisterHeadline"` to the headline Label in `SportowyHub.App/Views/Auth/RegisterPage.xaml`

## 2. Create page objects

- [x] 2.1 Create `SportowyHub.UITests/Pages/LoginPage.cs` with `IsHeadlineVisible()` method that locates `LoginHeadline` via `MobileBy.Id()`
- [x] 2.2 Create `SportowyHub.UITests/Pages/RegisterPage.cs` with `IsHeadlineVisible()` method that locates `RegisterHeadline` via `MobileBy.Id()`

## 3. Extend ProfilePage page object

- [x] 3.1 Add `TapSignIn()` method to `SportowyHub.UITests/Pages/ProfilePage.cs` that locates and taps `SignInRow`
- [x] 3.2 Add `TapCreateAccount()` method to `SportowyHub.UITests/Pages/ProfilePage.cs` that locates and taps `CreateAccountRow`

## 4. Create test fixture

- [x] 4.1 Create `SportowyHub.UITests/Tests/AuthScreenTests.cs` with `[TestFixture]` inheriting `AppiumSetup`
- [x] 4.2 Add `[Test, Order(1)]` test: navigate to Profile, tap Sign In, assert `LoginPage.IsHeadlineVisible()`, press back
- [x] 4.3 Add `[Test, Order(2)]` test: navigate to Profile, tap Create Account, assert `RegisterPage.IsHeadlineVisible()`, press back

## 5. Verify

- [x] 5.1 Build the UITests project (`dotnet build SportowyHub.UITests`)
