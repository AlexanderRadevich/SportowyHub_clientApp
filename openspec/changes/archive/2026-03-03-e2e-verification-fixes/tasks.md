## 1. UserProfile Model Fixes

- [x] 1.1 Remove top-level `Locale` parameter from `UserProfile` record in `UserProfile.cs`
- [x] 1.2 Change `ReputationScore` from `int` to `decimal` with `[property: JsonConverter(typeof(FlexibleDecimalConverter))]` in `UserProfile.cs`
- [x] 1.3 Update any code that reads `UserProfile.Locale` to use `UserProfile.Account?.Locale` instead

## 2. UpdateProfileAccountRequest Fix

- [x] 2.1 Change `NotificationsEnabled` from `bool` to `bool?` in `UpdateProfileAccountRequest.cs`
- [x] 2.2 Update `EditProfileViewModel` to pass `bool?` for `NotificationsEnabled` when constructing `UpdateProfileAccountRequest`

## 3. UpdateProfileResponse Model

- [x] 3.1 Create `UpdateProfileResponse` record with `Account` (`UserAccount?`) property in `Models/Api/UpdateProfileResponse.cs`
- [x] 3.2 Register `UpdateProfileResponse` in `SportowyHubJsonContext`
- [x] 3.3 Update `AuthService.UpdateProfileAsync` to deserialize PATCH response as `UpdateProfileResponse` instead of `UserProfile`
- [x] 3.4 Update `IAuthService.UpdateProfileAsync` return type signature to match

## 4. Verification

- [x] 4.1 Run `dotnet build` with 0 errors, 0 warnings
- [x] 4.2 Mark tasks 2.3, 3.4, 4.2 as complete in `sync-api-endpoints/tasks.md`
