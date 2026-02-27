## 1. Create AccountProfilePage placeholder

- [x] 1.1 Create `Views/Profile/AccountProfilePage.xaml` with centered placeholder label and `Shell.TabBarIsVisible="False"`
- [x] 1.2 Create `Views/Profile/AccountProfilePage.xaml.cs` code-behind
- [x] 1.3 Register `account-profile` route in `AppShell.xaml.cs`
- [x] 1.4 Register `AccountProfilePage` as Transient in `MauiProgram.cs`

## 2. Add localization resource for Account Profile

- [x] 2.1 Add `ProfileAccountProfile` string to all 4 .resx files (pl, en, uk, ru)

## 3. Update ProfileViewModel with auth state

- [x] 3.1 Add `IAuthService` constructor parameter to `ProfileViewModel`
- [x] 3.2 Add `[ObservableProperty] bool IsLoggedIn` property
- [x] 3.3 Add `RefreshAuthStateCommand` that calls `IsLoggedInAsync()` and sets `IsLoggedIn`
- [x] 3.4 Add `GoToAccountProfileCommand` that navigates to `account-profile`

## 4. Update ProfilePage XAML for conditional auth rows

- [x] 4.1 Wrap Sign In row + separator in a container with `IsVisible="{Binding IsLoggedIn, Converter={StaticResource InvertBoolConverter}}"`
- [x] 4.2 Wrap Create Account row + separator with the same inverted visibility binding
- [x] 4.3 Add Account Profile row + separator with `IsVisible="{Binding IsLoggedIn}"` and chevron, bound to `GoToAccountProfileCommand`

## 5. Wire up Appearing event

- [x] 5.1 In `ProfilePage.xaml.cs`, subscribe to `Appearing` event and call `RefreshAuthStateCommand.Execute(null)` on the ViewModel
