## 1. Add AutomationId to XAML Entry fields

- [x] 1.1 Add `AutomationId="LoginEmailEntry"` and `AutomationId="LoginPasswordEntry"` to Entry fields in `Views/Auth/LoginPage.xaml`
- [x] 1.2 Add `AutomationId="RegisterEmailEntry"`, `AutomationId="RegisterPhoneEntry"`, `AutomationId="RegisterPasswordEntry"`, `AutomationId="RegisterConfirmPasswordEntry"` to Entry fields in `Views/Auth/RegisterPage.xaml`
- [x] 1.3 Add `AutomationId="SearchEntry"` to the Entry field in `Views/Search/SearchPage.xaml`

## 2. Extend existing page objects with text entry methods

- [x] 2.1 Add `TypeEmail()`, `TypePassword()`, `GetEmailText()`, `GetPasswordText()`, `ClearEmail()`, `ClearPassword()` methods to `UITests/Pages/LoginPage.cs`
- [x] 2.2 Add `TypeEmail()`, `TypePhone()`, `TypePassword()`, `TypeConfirmPassword()`, `GetEmailText()`, `GetPhoneText()`, `GetPasswordText()`, `GetConfirmPasswordText()`, `ClearEmail()`, `ClearPhone()`, `ClearPassword()`, `ClearConfirmPassword()` methods to `UITests/Pages/RegisterPage.cs`

## 3. Create SearchPage page object

- [x] 3.1 Create `UITests/Pages/SearchPage.cs` with `TypeSearch()`, `GetSearchText()`, `ClearSearch()` methods using `MobileBy.Id("SearchEntry")`

## 4. Create EntryInputTests fixture

- [x] 4.1 Create `UITests/Tests/EntryInputTests.cs` with ordered tests: SearchEntry (Order 1), LoginEmailEntry (Order 2), LoginPasswordEntry (Order 3), RegisterEmailEntry (Order 4), RegisterPhoneEntry (Order 5), RegisterPasswordEntry (Order 6), RegisterConfirmPasswordEntry (Order 7). Each test types a known string, asserts the value, and clears the field.
