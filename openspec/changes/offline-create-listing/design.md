## Context

The MAUI client app uses Shell navigation with `INavigationService` wrapping `Shell.Current.GoToAsync()`. Authentication is handled by `IAuthService` with tokens in SecureStorage. The home page FAB is currently gated behind `IsLoggedIn`, and after login the app always navigates to `//home`. Several screens require authentication (Favorites, My Listings, Account Profile) but none preserve the user's intended destination across the login flow.

Current navigation patterns:
- Login success: `await nav.GoToAsync("..")` then `await nav.GoToAsync("//home")`
- Google OAuth success: same pattern
- Favorites sign-in: `await nav.GoToAsync("login")` (no return context)
- Profile sign-in: `await nav.GoToAsync("login")` (no return context)
- Home FAB: checks auth + trust level, shows toast if unauth

## Goals / Non-Goals

**Goals:**
- Make the Create Listing FAB visible to all users regardless of auth state
- Redirect unauthenticated users to login when they tap the FAB
- Preserve the user's intended destination across the login flow using a return URL
- Apply the return URL pattern to all auth-gated entry points (FAB, Favorites, Profile actions)

**Non-Goals:**
- Deep return URL with query parameters (just store the Shell route string)
- Persisting return URL across app restarts (in-memory only)
- Automatic auth interception middleware (each ViewModel explicitly guards its actions)
- Handling return URL for registration flow (registration has its own email verification flow)
- Return URL stack (only one return URL at a time)

## Decisions

### 1. ReturnUrlService as a simple singleton

**Decision:** Create `IReturnUrlService` with `SetReturnUrl(string route)`, `string? ConsumeReturnUrl()`, and `bool HasReturnUrl` members. Implement as a singleton holding a single `string?` field. `ConsumeReturnUrl` returns the stored value and clears it atomically.

**Rationale:** The return URL is a transient, in-memory concept. Only one auth redirect can be in flight at a time. A singleton with consume-on-read semantics prevents stale return URLs from being used on subsequent logins.

**File:** `SportowyHub.App/Services/Navigation/IReturnUrlService.cs` and `SportowyHub.App/Services/Navigation/ReturnUrlService.cs`

### 2. NavigateToLoginWithReturnUrlAsync helper

**Decision:** Add `NavigateToLoginWithReturnUrlAsync(string returnUrl)` to `INavigationService`. The implementation stores the return URL via `IReturnUrlService`, then calls `GoToAsync("login")`.

**Rationale:** Avoids scattering `IReturnUrlService` injection across every ViewModel that needs auth guards. The navigation service already handles route navigation, so this is a natural extension.

**Alternative considered:** Having each ViewModel inject `IReturnUrlService` directly. Rejected because it duplicates the set-then-navigate pattern in multiple places.

**File:** Modify `SportowyHub.App/Services/Navigation/INavigationService.cs` and `SportowyHub.App/Services/Navigation/ShellNavigationService.cs`

### 3. LoginViewModel consumes return URL after login

**Decision:** After successful login (both email/password and Google OAuth), call `IReturnUrlService.ConsumeReturnUrl()`. If a return URL is present, navigate with `await nav.GoToAsync("..")` then `await nav.GoToAsync(returnUrl)`. If no return URL, keep the current behavior: `await nav.GoToAsync("..")` then `await nav.GoToAsync("//home")`.

**Rationale:** The LoginViewModel is the single point where login success is handled. Consuming the return URL here covers all entry points that set it.

**File:** Modify `SportowyHub.App/ViewModels/LoginViewModel.cs`

### 4. HomeViewModel auth redirect

**Decision:** In `GoToCreateListing`, check `IsLoggedInAsync()` first. If not logged in, call `nav.NavigateToLoginWithReturnUrlAsync("//home")` and return. The return URL is `//home` because after login the user should land on the home page, which is where the FAB lives, and they can tap it again now that they are authenticated.

**Rationale:** After login the user returns to the home page. The FAB is still visible and now the auth check passes. We do not navigate directly to `create-edit-listing` as the return URL because the user also needs the trust level check (phone verification), which happens in `GoToCreateListing`.

**Alternative considered:** Setting return URL to `create-edit-listing` directly. Rejected because the trust level check in `GoToCreateListing` would be bypassed if we navigated directly to the create listing route.

**File:** Modify `SportowyHub.App/ViewModels/HomeViewModel.cs`

### 5. FAB always visible

**Decision:** Remove `IsVisible="{Binding IsLoggedIn}"` from the FAB `Button` in `HomePage.xaml`. The `IsLoggedIn` property and `CheckAuthCommand` can remain on `HomeViewModel` if other parts of the page use them; otherwise they become candidates for removal.

**File:** Modify `SportowyHub.App/Views/Home/HomePage.xaml`

### 6. FavoritesViewModel auth redirect

**Decision:** In the `SignIn` command, use `nav.NavigateToLoginWithReturnUrlAsync("//favorites")` instead of `nav.GoToAsync("login")`. This way, after login the user returns to the Favorites tab.

**File:** Modify `SportowyHub.App/ViewModels/FavoritesViewModel.cs`

### 7. ProfileViewModel auth redirect

**Decision:** In `SignInAsync`, use `nav.NavigateToLoginWithReturnUrlAsync("//profile")`. The auth-gated actions (`GoToAccountProfile`, `GoToMyListings`) already require being on the profile page, and the profile page checks `IsLoggedIn` on appearing, so returning to `//profile` is sufficient.

**File:** Modify `SportowyHub.App/ViewModels/ProfileViewModel.cs`

### 8. DI registration

**Decision:** Register `IReturnUrlService` as singleton in `MauiProgram.cs`. Update `ShellNavigationService` registration to inject `IReturnUrlService`.

**File:** Modify `SportowyHub.App/MauiProgram.cs`

## Risks / Trade-offs

- **Single return URL** means if the user somehow triggers two auth guards before logging in, only the last one is preserved. This is acceptable because the UI flow is sequential: the user can only be on one screen at a time.
- **Return URL is `//home` for FAB, not `create-edit-listing`** means the user has to tap the FAB again after login. This is intentional because the trust level check must still run, and it provides clearer UX (user sees the home page, then taps FAB).
- **`ShellNavigationService` gains a dependency on `IReturnUrlService`** which slightly increases its responsibility. Acceptable because it is a single method that combines two related operations.

## Open Questions

None.
