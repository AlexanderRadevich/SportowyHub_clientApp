## 1. API Model — adjust to flat structure

- [x] 1.1 Replace `UserProfile`, `UserAccount`, `OAuthLinked` records in `Models/Api/UserProfile.cs` with a single flat `UserProfile` record matching `/api/private/profile` response: `Id`, `Email`, `FirstName`, `LastName`, `Locale`, `AvatarUrl`, `NotificationsEnabled`, `QuietHoursStart`, `QuietHoursEnd`, `Phone`, `PhoneVerified`, `EmailVerified`, `TrustLevel`
- [x] 1.2 Update `SportowyHubJsonContext` — remove `UserAccount` and `OAuthLinked` registrations, keep only `UserProfile`

## 2. Auth Service — change endpoint

- [x] 2.1 Update `AuthService.GetProfileAsync()` to call `GET /api/private/profile` instead of `/api/v1/me`

## 3. Localization — remove unused keys

- [x] 3.1 Remove unused localization keys from all 4 `.resx` files and `AppResources.Designer.cs`: `ProfileReputation`, `ProfileBalance`, `ProfileGoogle`, `ProfileNotLinked`, `ProfileLinked`

## 4. ViewModel — adjust computed properties

- [x] 4.1 Update `AccountProfileViewModel.DisplayName` to compute from `Profile.FirstName`/`Profile.LastName` (concatenate non-null parts, trim, fall back to email)
- [x] 4.2 Remove `BalanceFormatted` computed property (balance is no longer in the response)

## 5. Page UI — simplify layout

- [x] 5.1 Update `AccountProfilePage.xaml`: remove Reputation row, Balance row, and Google OAuth row from the Account section
- [x] 5.2 Update header bindings: change `Profile.Account.FullName` references to use `DisplayName`, remove `DisplayEmail` sub-label's `IsVisible` binding (show email whenever `FirstName` or `LastName` is set)
- [x] 5.3 Update contact section: change `Profile.Email` to `Profile.Email`, `Profile.Phone` to `Profile.Phone` (already flat, but remove any `Profile.Account.*` references)

## 6. Build Verification

- [x] 6.1 Verify project builds with 0 errors and 0 warnings
