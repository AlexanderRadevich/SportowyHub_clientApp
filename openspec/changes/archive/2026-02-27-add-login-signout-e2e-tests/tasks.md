## 1. App-side changes

- [x] 1.1 Add `AutomationId="LoginButton"` to the Login button in `SportowyHub.App/Views/Auth/LoginPage.xaml`

## 2. Test configuration

- [x] 2.1 Add `TestEmail` and `TestPassword` constants to `SportowyHub.UITests/Config/TestConfig.cs` with values `"alex10@gmail.com"` and `"qwerty12345"`

## 3. Page object updates

- [x] 3.1 Add `TapLogin()` method to `SportowyHub.UITests/Pages/LoginPage.cs` that taps the element with `MobileBy.Id("LoginButton")`
- [x] 3.2 Add `TapAccountProfile()` method to `SportowyHub.UITests/Pages/ProfilePage.cs` that taps the Account Profile row
- [x] 3.3 Add `IsSignInRowVisible()` method to `SportowyHub.UITests/Pages/ProfilePage.cs` that returns bool using short-wait element check on `SignInRow`

## 4. New page object

- [x] 4.1 Create `SportowyHub.UITests/Pages/AccountProfilePage.cs` with constructor taking `AndroidDriver`, a `TapSignOut()` method targeting `SignOutButton`, and a `ConfirmSignOut()` method that taps the confirm button on the native AlertDialog using localized text (pl/en/uk/ru)

## 5. Toast assertion helper

- [x] 5.1 Create `AssertNoErrorToast()` static helper method (in `SportowyHub.UITests/Helpers/ToastHelper.cs`) that uses a 2-second explicit wait with `FindElements` to check for Snackbar elements; fails with descriptive message if found

## 6. Test class

- [x] 6.1 Create `SportowyHub.UITests/Tests/LoginSignOutTests.cs` with test Order(1): navigate to Profile → tap Sign In → enter TestConfig credentials → tap Login → assert Home tab is active → assert no error toast
- [x] 6.2 Add test Order(2) to `LoginSignOutTests`: navigate to Profile → tap Account Profile → tap Sign Out → confirm dialog → assert SignInRow visible → assert no error toast

## 7. Build verification

- [x] 7.1 Verify `dotnet build` of both `SportowyHub.App` and `SportowyHub.UITests` projects succeeds with 0 errors
