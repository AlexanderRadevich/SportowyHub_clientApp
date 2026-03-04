## Why

The "Create Listing" floating action button (FAB) on the home screen is currently hidden for non-authenticated users (`IsVisible="{Binding IsLoggedIn}"`). This means new users never see the primary call-to-action, reducing discoverability and conversion. When the FAB was visible and an unauthenticated user tapped it, the app showed a toast error instead of guiding the user toward authentication.

Additionally, after any login triggered by an auth guard (FAB, Favorites, profile actions), the app always navigates to `//home`, losing the user's context. On the web this is solved with a "return URL" pattern, and the same approach works for Shell navigation.

## What Changes

- **FAB always visible**: Remove `IsVisible="{Binding IsLoggedIn}"` from the home page FAB so all users see it
- **Auth redirect instead of toast**: When an unauthenticated user taps the FAB, redirect to the login screen instead of showing a toast error
- **Return URL service**: Introduce `IReturnUrlService` / `ReturnUrlService` (singleton) that stores a single return route string
- **Login return navigation**: After successful login (email/password or Google OAuth), check the return URL service. If a return route is set, navigate there instead of `//home`
- **Auth guards with return URL**: Update Favorites sign-in button and Profile auth-gated actions (My Listings, Account Profile) to store the return URL before navigating to login

## Capabilities

### New Capabilities
- `return-url-service`: A singleton service that stores and retrieves a return route for post-login navigation. Stores one string, clears after consumption.

### Modified Capabilities
- `auth-screens`: LoginViewModel checks return URL after successful login and navigates accordingly. Home FAB is always visible.
- `shell-navigation`: INavigationService gains a `NavigateToLoginWithReturnUrlAsync` helper method.

## Impact

- **Services**: New `IReturnUrlService` / `ReturnUrlService`, minor addition to `INavigationService` / `ShellNavigationService`
- **ViewModels**: `HomeViewModel` (auth redirect), `LoginViewModel` (return URL navigation), `FavoritesViewModel` (return URL on sign-in), `ProfileViewModel` (return URL on auth-gated actions)
- **Views**: `HomePage.xaml` (remove IsVisible binding from FAB)
- **DI**: Register `IReturnUrlService` as singleton in `MauiProgram.cs`
