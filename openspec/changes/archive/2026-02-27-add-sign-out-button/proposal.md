## Why

Users who are logged in have no way to sign out from the app. The Account Profile page is currently a placeholder with no actions. A sign-out button is the most essential action on this screen, allowing users to end their session.

## What Changes

- Add a "Sign Out" button to the Account Profile page (`AccountProfilePage`)
- Add a "Sign Out" row on the Profile tab below the "Account Profile" row (visible when logged in), providing a quick sign-out without navigating into Account Profile
- Add a `LogoutAsync()` method to `IAuthService` / `AuthService` that calls `POST /api/v1/logout` on the backend (to revoke the refresh token server-side) and then clears local tokens via `ClearAuthAsync()`
- After sign-out, navigate the user back to the Profile tab (which will show the logged-out state with Sign In / Create Account rows)
- Add localized strings for the sign-out button label and a confirmation prompt (4 languages: pl, en, uk, ru)

## Capabilities

### New Capabilities
_(none — this extends existing capabilities)_

### Modified Capabilities
- `account-profile-placeholder`: Add sign-out button UI and interaction to the placeholder page
- `profile-hub`: Add "Sign Out" row in the logged-in account section
- `auth-screens`: Add `LogoutAsync` to the auth service contract and implementation

## Impact

- **Files modified**: `AccountProfilePage.xaml`, `AccountProfilePage.xaml.cs` (or new ViewModel), `IAuthService.cs`, `AuthService.cs`, `ProfileViewModel.cs`, `MauiProgram.cs` (if new ViewModel registered), `AppResources.resx` (4 language files)
- **Backend dependency**: `POST /api/v1/logout` endpoint (already specified in backend feature-073-refresh-token FR-9)
- **Navigation**: After sign-out, navigate back to Profile tab showing logged-out state
