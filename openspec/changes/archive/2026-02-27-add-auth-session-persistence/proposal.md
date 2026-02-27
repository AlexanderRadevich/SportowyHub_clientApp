## Why

After successful login the token is stored in SecureStorage, but nothing checks it on app launch or when the Profile tab appears. The user always sees "Sign In" and "Create Account" even when authenticated. We need the Profile page to reflect auth state: hide auth rows when logged in and show an "Account Profile" link instead.

## What Changes

- ProfileViewModel gains an `IAuthService` dependency and checks `IsLoggedInAsync` when the page appears
- ProfileViewModel exposes an `IsLoggedIn` observable property that drives visibility of auth rows vs. account profile row
- Profile XAML conditionally shows Sign In / Create Account rows (logged out) or an Account Profile row (logged in)
- The Account Profile row navigates to a blank placeholder page for now
- On app startup, no token validation is performed beyond checking SecureStorage presence (token refresh is already handled separately)

## Capabilities

### New Capabilities
- `account-profile-placeholder`: Blank Account Profile page registered as a shell route, navigable from Profile hub when authenticated

### Modified Capabilities
- `profile-hub`: Account section switches between logged-out rows (Sign In, Create Account) and logged-in row (Account Profile) based on auth state
- `auth-screens`: ProfileViewModel requires IAuthService injection and exposes IsLoggedIn property

## Impact

- **ViewModels**: ProfileViewModel — add IAuthService dependency, IsLoggedIn property, Appearing refresh logic
- **XAML**: ProfilePage.xaml — conditional visibility on auth rows, new Account Profile row
- **New page**: AccountProfilePage.xaml (blank placeholder)
- **DI**: ProfileViewModel registration changes from parameterless to injected
- **Shell routing**: Register `account-profile` route
