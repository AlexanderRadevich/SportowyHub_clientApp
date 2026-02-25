## Why

The backend API endpoints `/api/v1/register` and `/api/v1/login` have been updated with new response fields, a new error format, and a stricter password policy. The client models, error handling, and validation logic are now out of sync with the API contract and need to be aligned.

## What Changes

- **Add `locale` field to all response/error models**: The API now returns a top-level `locale` field (ru/pl/en/uk) in every JSON response (success and error). `LoginResponse`, `RegisterResponse`, and `ApiError` need this field.
- **Add `ValidationError` model**: The API returns 422 errors in a new format with `error.code`, `error.message`, and `error.violations` (object keyed by field name). This is distinct from `ApiError` and currently not handled.
- **Handle login 422 responses**: The login endpoint can now return 422 validation errors (previously only 401). `AuthService.LoginAsync` needs to handle this path.
- **Update password minimum length from 6 to 8**: The API spec defines `minLength: 8` for registration passwords. Client-side validation in `RegisterViewModel` currently enforces `>= 6`. **BREAKING** for users who had 6-7 character passwords.
- **Update password strength thresholds**: Adjust the strength indicator so "Medium" starts at 8 characters (the API minimum) instead of 6, keeping the UX aligned with what the server will accept.
- **Parse `violations` for field-level errors**: `AuthService.ParseErrorWithFields` needs to parse the new `ValidationError.error.violations` format to extract field-level errors for both register and login.
- **Wire up optional phone field for registration**: The `RegisterRequest` model already has `Phone` but it's never exposed in the UI or passed through the service layer. Add a phone input to the registration form, pass it through `IAuthService.RegisterAsync` and `AuthService` to the API.

## Capabilities

### New Capabilities
_(none)_

### Modified Capabilities
- `auth-api-models`: Add `locale` field to response/error models, add `ValidationError` model with violations support, register new types in JSON source generation context
- `auth-service`: Handle `ValidationError` (422) parsing for both login and register, update `ParseErrorWithFields` to extract violations, add optional `phone` parameter to `RegisterAsync`
- `auth-screens`: Update password minimum length validation from 6 to 8, adjust password strength thresholds, add optional phone input field to registration form

## Impact

- **Models**: `LoginResponse`, `RegisterResponse`, `ApiError` gain a `Locale` property; new `ValidationError` model added
- **JSON context**: `SportowyHubJsonContext` needs `ValidationError` registered for AOT serialization
- **AuthService**: Error parsing logic updated for both endpoints
- **RegisterViewModel**: `CanCreateAccount()` threshold changes from 6 to 8; `EvaluatePasswordStrength()` thresholds shift upward; new `Phone` property wired to registration form and service call
- **Existing specs**: Three specs updated (`auth-api-models`, `auth-service`, `auth-screens`)
