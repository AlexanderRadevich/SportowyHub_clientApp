## Context

`AuthService.StoreTokens` already persists `access_token`, `refresh_token`, and `token_expiry` to SecureStorage on login. `IsLoggedInAsync()` checks for a stored token. However, nothing in the UI layer calls these methods — the Profile page always renders the logged-out Account section (Sign In + Create Account rows).

ProfileViewModel currently has no `IAuthService` dependency (parameterless constructor). It is registered as Transient in DI and injected into ProfilePage via constructor.

## Goals / Non-Goals

**Goals:**
- Profile page reflects auth state on every appearance (logged-in vs logged-out)
- Logged-in state hides Sign In / Create Account rows, shows Account Profile row
- Account Profile row navigates to a blank placeholder page
- Auth state refreshes each time Profile tab is selected (not just on first load)

**Non-Goals:**
- Token validation or refresh on app startup (handled by existing `RefreshTokenAsync`)
- Account Profile page content (blank placeholder for now)
- Logout functionality (separate change)
- Displaying user info (email, name) on Profile page

## Decisions

### 1. Auth state check: Page.Appearing event
Check `IAuthService.IsLoggedInAsync()` in a handler triggered by the page's `Appearing` event. This ensures the state refreshes every time the user navigates to Profile (including after login/logout).

**Alternative considered:** Check once in constructor — rejected because the ViewModel is Transient but the Shell may cache the tab page, so state could go stale.

**Implementation:** ProfilePage.xaml.cs subscribes to `Appearing` and calls a `RefreshAuthState` command on the ViewModel. The ViewModel calls `IsLoggedInAsync()` and sets `IsLoggedIn`.

### 2. Visibility toggle: IsLoggedIn boolean property
Add `[ObservableProperty] bool IsLoggedIn` to ProfileViewModel. In XAML, bind `IsVisible` on the logged-out rows to `{Binding IsLoggedIn, Converter={StaticResource InvertBoolConverter}}` and on the logged-in row to `{Binding IsLoggedIn}`.

**Alternative considered:** Two separate ContentViews swapped via DataTrigger — over-engineered for toggling 3 rows.

### 3. DI change: ProfileViewModel gets IAuthService
Change ProfileViewModel constructor to accept `IAuthService`. DI already registers `IAuthService` as Singleton and ProfileViewModel as Transient, so no registration changes needed beyond the constructor parameter.

### 4. AccountProfilePage: blank placeholder
Create `Views/Profile/AccountProfilePage.xaml` with a centered placeholder label. Register route `account-profile` in AppShell.xaml.cs. ProfileViewModel gets a `GoToAccountProfile` command that navigates to `account-profile`.

## Risks / Trade-offs

- **[Risk]** `IsLoggedInAsync` only checks token presence, not expiry → Acceptable for UI toggle; actual token validation happens on API calls via refresh flow.
- **[Risk]** Appearing fires on every tab switch, calling SecureStorage each time → Mitigated: SecureStorage reads are fast (in-memory cache on Android after first read).

## Open Questions

None — scope is deliberately minimal.
