## 1. Localization

- [x] 1.1 Add `AuthRegistrationSuccess` and `AuthRegistrationSuccessMessage` keys to `AppResources.resx` (en, default)
- [x] 1.2 Add translations to `AppResources.pl.resx`, `AppResources.uk.resx`, `AppResources.ru.resx`

## 2. ViewModel Logic

- [x] 2.1 In `RegisterViewModel.CreateAccount()`, branch on `registerResult.Data.TrustLevel == "TL0"`: show `DisplayAlert` with localized strings and navigate back via `..`; otherwise navigate to email verification as before
