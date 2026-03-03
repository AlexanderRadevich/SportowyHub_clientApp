## Why

E2E verification of the `sync-api-endpoints` change revealed 4 mismatches in the profile update integration that will cause silent data loss or runtime errors. These must be fixed before the profile editing feature is reliable against the current backend.

## What Changes

- **BREAKING** `NotificationsEnabled` in `UpdateProfileAccountRequest` changes from `bool` to `bool?` to support partial PATCH semantics (omit field = no-op)
- Remove top-level `Locale` from `UserProfile` record (never deserializes — backend nests it inside `account`)
- Create `UpdateProfileResponse` record matching the actual PATCH response shape (`{ account: {...} }` only)
- Change `AuthService.UpdateProfileAsync` return type to use `UpdateProfileResponse` instead of `UserProfile`
- Change `UserProfile.ReputationScore` from `int` to `decimal` to match backend's `getReputationScoreDecimal()` return type
- Apply `FlexibleDecimalConverter` to `ReputationScore` for safety (same string-vs-number inconsistency pattern)

## Capabilities

### New Capabilities

(none)

### Modified Capabilities

- `edit-profile`: PATCH request/response models change to match actual backend contract
- `auth-api-models`: `UserProfile` field corrections (`Locale` removal, `ReputationScore` type change)

## Impact

- **Models**: `UserProfile.cs`, `UpdateProfileAccountRequest.cs` — field type changes
- **New model**: `UpdateProfileResponse.cs` — dedicated PATCH response record
- **Services**: `AuthService.UpdateProfileAsync` — return type change
- **ViewModels**: `EditProfileViewModel` — adapt to new return type (currently discards it, minimal impact)
- **JSON context**: Register `UpdateProfileResponse` in `SportowyHubJsonContext`
