## 1. Update API Models

- [x] 1.1 Add `Locale` property to `LoginResponse` record (`Models/Api/LoginResponse.cs`)
- [x] 1.2 Add `Locale` property to `RegisterResponse` record (`Models/Api/RegisterResponse.cs`)
- [x] 1.3 Add `Locale` property to `ApiError` record and add `Violations` (Dictionary<string, string>?, nullable) property to `ErrorDetail` record (`Models/Api/ApiError.cs`)
- [x] 1.4 Register updated types in `SportowyHubJsonContext` — add `[JsonSerializable(typeof(Dictionary<string, string>))]` for violations deserialization (`Services/Api/SportowyHubJsonContext.cs`)

## 2. Update AuthService Error Handling

- [x] 2.1 Update `ParseErrorWithFields` to extract `Violations` from `ApiError.Error.Violations` into the returned `FieldErrors` dictionary (`Services/Auth/AuthService.cs`)
- [x] 2.2 Update `LoginAsync` to use `ParseErrorWithFields` instead of `ParseErrorMessage` so that 422 field errors are surfaced (`Services/Auth/AuthService.cs`)

## 3. Update Registration Validation

- [x] 3.1 Change `CanCreateAccount()` password minimum length check from `< 6` to `< 8` (`ViewModels/RegisterViewModel.cs`)
- [x] 3.2 Update `EvaluatePasswordStrength()` thresholds — Weak: < 8, Medium: 8+ with letters and digits, Strong: 10+ with letters, digits, and special characters (`ViewModels/RegisterViewModel.cs`)

## 4. Update LoginViewModel Field Error Handling

- [x] 4.1 Update `LoginCommand` handler to check `FieldErrors` from the result and display email field errors in `EmailError` property (`ViewModels/LoginViewModel.cs`)

## 5. Wire Up Phone Field for Registration

- [x] 5.1 Add localization strings for phone field — `AuthPhone` and `AuthEnterPhone` — to all `.resx` files and `AppResources.Designer.cs`
- [x] 5.2 Add optional `phone` parameter to `IAuthService.RegisterAsync` signature (`Services/Auth/IAuthService.cs`)
- [x] 5.3 Update `AuthService.RegisterAsync` to accept and pass `phone` to `RegisterRequest` (`Services/Auth/AuthService.cs`)
- [x] 5.4 Add `Phone` and `PhoneError` observable properties to `RegisterViewModel`; make Phone required in `CanCreateAccount`; pass `Phone` to `RegisterAsync` call; handle "phone" field errors from server (`ViewModels/RegisterViewModel.cs`)
- [x] 5.5 Add Phone input field to `RegisterPage.xaml` between Email and Password fields with `Keyboard="Telephone"` (`Views/Auth/RegisterPage.xaml`)

## 6. Verify

- [x] 6.1 Build the solution and confirm no compilation errors
