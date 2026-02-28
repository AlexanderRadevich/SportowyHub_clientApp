## Why

The `/api/private/profile` endpoint has been restructured — personal fields (`first_name`, `last_name`, `avatar_url`, etc.) are now nested inside an `account` object, and several new fields have been added (`reputation_score`, `oauth_linked`, `full_name`, `avatar_thumbnail_url`, `balance_grosze`, verification timestamps). The current `UserProfile` flat record no longer matches the API response, breaking deserialization, and the UI doesn't display the new data.

## What Changes

- **BREAKING**: Restructure `UserProfile` record to match the new nested API response — extract `account` fields into a separate `UserAccount` record
- Add new fields: `EmailVerifiedAt`, `PhoneVerifiedAt`, `ReputationScore`, `OauthLinked`, `LastLoginAt`, `LastActivityAt` (top-level) and `FullName`, `AvatarThumbnailUrl`, `BalanceGrosze`, `BalanceUpdatedAt` (in account)
- Add `UserAccount` and `OauthLinked` DTOs, register them in `SportowyHubJsonContext`
- Update `AccountProfileViewModel` to access nested `Profile.Account.FirstName` etc.
- Update `AccountProfilePage.xaml` UI to:
  - Show `ReputationScore` in the Account section
  - Show `FullName` (from account) in the header when available
  - Show linked OAuth providers in Account section
  - Show `BalanceGrosze` formatted as currency in Account section
- Add localization strings for new UI labels (pl, en, uk, ru)

## Capabilities

### New Capabilities
_(none — all changes fit within existing capabilities)_

### Modified Capabilities
- `account-profile-placeholder`: Updated `UserProfile` model structure (nested `account` object), new fields, new UI sections for reputation, balance, and OAuth status
- `auth-api-models`: New `UserAccount` and `OauthLinked` DTOs, updated `UserProfile` record, new `SportowyHubJsonContext` registrations

## Impact

- **Model**: `UserProfile.cs` — breaking change from flat record to nested structure
- **New models**: `UserAccount.cs`, `OauthLinked.cs`
- **JSON context**: `SportowyHubJsonContext.cs` — register new types
- **ViewModel**: `AccountProfileViewModel.cs` — update property access paths
- **View**: `AccountProfilePage.xaml` — new UI rows and updated bindings
- **Localization**: `AppResources.resx` (4 languages) — new string keys
- **No API service changes**: `GetProfileAsync()` still calls the same endpoint, deserialization handles the rest
