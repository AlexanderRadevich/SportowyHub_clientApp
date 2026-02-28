## 1. Models & Serialization

- [x] 1.1 Create `UserAccount` record in `Models/Api/UserAccount.cs` with all properties: `FirstName`, `LastName`, `FullName`, `AvatarUrl`, `AvatarThumbnailUrl`, `NotificationsEnabled`, `QuietHoursStart`, `QuietHoursEnd`, `Locale`, `BalanceGrosze`, `BalanceUpdatedAt`
- [x] 1.2 Create `OauthLinked` record in `Models/Api/OauthLinked.cs` with `Google` (bool) property
- [x] 1.3 Update `UserProfile` record to new nested structure: replace flat fields with `Account` (UserAccount?), add `EmailVerifiedAt`, `PhoneVerifiedAt`, `ReputationScore`, `OauthLinked`, `LastLoginAt`, `LastActivityAt`
- [x] 1.4 Register `UserAccount` and `OauthLinked` in `SportowyHubJsonContext` with `[JsonSerializable]` attributes

## 2. ViewModel

- [x] 2.1 Update `DisplayName` in `AccountProfileViewModel` to use cascade: `Profile.Account?.FullName` → `FirstName`+`LastName` → `Email`
- [x] 2.2 Update `DisplayEmail` to access `Profile?.Email` (unchanged, but verify)
- [x] 2.3 Add `FormattedBalance` computed property: format `Profile.Account?.BalanceGrosze` as `"X.XX zł"`
- [x] 2.4 Add `TrustInfo` computed property: format as `"TL1 · 5 rep"` from `TrustLevel` and `ReputationScore`
- [x] 2.5 Raise `OnPropertyChanged` for new computed properties after profile load

## 3. XAML UI

- [x] 3.1 Update header: bind display name subtitle to `TrustInfo` instead of raw `TrustLevel`
- [x] 3.2 Update header: bind email visibility to `Profile.Account.FirstName` → `Profile.Account` (check for non-null Account with any name)
- [x] 3.3 Add Reputation Score row to Account section with localized label and `Profile.ReputationScore` value
- [x] 3.4 Add Balance row to Account section with localized label and `FormattedBalance` binding
- [x] 3.5 Add Google Account row to Account section with linked/not-linked status using DataTriggers (same pattern as email/phone verification badges)

## 4. Localization

- [x] 4.1 Add `ProfileReputationScore`, `ProfileBalance`, `ProfileGoogleLinked`, `ProfileLinked`, `ProfileNotLinked` keys to `AppResources.resx` (en)
- [x] 4.2 Add same keys to `AppResources.pl.resx`
- [x] 4.3 Add same keys to `AppResources.uk.resx`
- [x] 4.4 Add same keys to `AppResources.ru.resx`

## 5. Verification

- [x] 5.1 Build the project and fix any compilation errors
- [x] 5.2 Verify JSON deserialization works with the new nested API response shape
