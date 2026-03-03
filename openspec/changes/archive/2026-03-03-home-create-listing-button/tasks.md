## 1. ViewModel Changes

- [x] 1.1 Add `IAuthService` to `HomeViewModel` primary constructor
- [x] 1.2 Add `IsLoggedIn` observable property to `HomeViewModel`
- [x] 1.3 Add `CheckAuthCommand` that calls `IAuthService.IsLoggedInAsync()` and sets `IsLoggedIn`
- [x] 1.4 Add `GoToCreateListingCommand` that checks trust level via `IAuthService.GetCurrentUserAsync()` — navigate to `create-edit-listing` for TL1+, show toast for TL0

## 2. Page Layout

- [x] 2.1 Wrap the existing Row 1 content (`RefreshView`) and a new FAB `Button` in a `Grid` so the FAB overlays the feed
- [x] 2.2 Add the FAB `Button` (56x56, `+`, bottom-right, 16dp margin) with `IsVisible="{Binding IsLoggedIn}"` and `Command="{Binding GoToCreateListingCommand}"`

## 3. Page Lifecycle

- [x] 3.1 Call `CheckAuthCommand` in `HomePage.xaml.cs` `OnAppearing` alongside the existing `LoadListingsCommand` call

## 4. Localization

- [x] 4.1 Add localized string for TL0 toast message (phone verification required) to all 4 `.resx` files (pl, en, uk, ru)

## 5. Verification

- [x] 5.1 Build passes (`dotnet build`)
- [ ] 5.2 Manual test: anonymous user does not see FAB on home page
- [ ] 5.3 Manual test: TL1+ user sees FAB and can navigate to create listing
- [ ] 5.4 Manual test: TL0 user sees FAB and gets toast on tap
