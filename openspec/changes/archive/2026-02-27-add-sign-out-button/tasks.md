## 1. Localization

- [x] 1.1 Add localized strings to all 4 `.resx` files (pl, en, uk, ru): `SignOut`, `SignOutConfirmTitle`, `SignOutConfirmMessage`, `Cancel` (if not already present)
- [x] 1.2 Regenerate `AppResources.Designer.cs` to include the new keys

## 2. Auth Service

- [x] 2.1 Add `Task LogoutAsync()` to `IAuthService` interface
- [x] 2.2 Implement `LogoutAsync()` in `AuthService`: read refresh token, attempt `POST /api/v1/logout` with Bearer auth, always call `ClearAuthAsync()`, catch all exceptions silently

## 3. ViewModel

- [x] 3.1 Create `AccountProfileViewModel` class with `IAuthService` injection and `SignOutCommand` relay command
- [x] 3.2 Implement `SignOutCommand`: show `DisplayAlert` confirmation dialog with localized strings, call `LogoutAsync()` on confirm, navigate back via `Shell.Current.GoToAsync("..")`

## 4. UI and DI Wiring

- [x] 4.1 Update `AccountProfilePage.xaml.cs` to accept `AccountProfileViewModel` via constructor and set as `BindingContext`
- [x] 4.2 Update `AccountProfilePage.xaml`: add Sign Out button with `AutomationId="SignOutButton"`, localized text, destructive styling (red text, transparent background), bound to `SignOutCommand`
- [x] 4.3 Register `AccountProfileViewModel` as Transient in `MauiProgram.cs`

## 5. Profile Page Sign Out Row

- [x] 5.1 Add `SignOutCommand` to `ProfileViewModel`: show confirmation dialog, call `LogoutAsync()` on confirm, then call `RefreshAuthState()` to update UI
- [x] 5.2 Add "Sign Out" row to `ProfilePage.xaml` below "Account Profile" row (logged-in section): tappable Grid with `AutomationId="ProfileSignOutRow"`, localized label in Primary/red color, bound to `SignOutCommand`
