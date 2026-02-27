## Context

The Account Profile page (`AccountProfilePage`) is a placeholder with a centered label and no ViewModel. Users who are logged in navigate here from the Profile tab but have no way to sign out. The auth service (`AuthService`) has `ClearAuthAsync()` to remove local tokens but no method to call the backend logout endpoint. The backend `POST /api/v1/logout` endpoint exists (feature-073-refresh-token FR-9) and revokes the refresh token server-side.

## Goals / Non-Goals

**Goals:**
- Allow users to sign out from the Account Profile screen
- Revoke the refresh token server-side via `POST /api/v1/logout`
- Always clear local tokens, even if the server call fails
- Show a confirmation prompt before signing out

**Non-Goals:**
- Building out the full Account Profile screen (future change)
- Handling forced logout from expired/revoked tokens (separate concern)
- Adding sign-out to other screens

## Decisions

### 1. Create an `AccountProfileViewModel`

The page currently has no ViewModel. Rather than putting logic in code-behind (inconsistent with the rest of the app), create a dedicated `AccountProfileViewModel` with a `SignOutCommand`. Register it as Transient in DI, following the existing pattern for all ViewModels in `MauiProgram.cs`.

### 2. `LogoutAsync` — best-effort server revocation, guaranteed local clear

Add `LogoutAsync()` to `IAuthService` / `AuthService`:
1. Read the refresh token from SecureStorage
2. If a refresh token exists, attempt `POST /api/v1/logout` with the refresh token as Bearer auth (same pattern as `RefreshTokenAsync`)
3. **Always** call `ClearAuthAsync()` regardless of API success/failure — the user must never get stuck logged in
4. No return value needed; sign-out is fire-and-forget on the server side

This avoids blocking sign-out when offline or when the server is unreachable.

### 3. Navigation after sign-out

After clearing tokens, navigate back using `Shell.Current.GoToAsync("..")`. This returns to the Profile tab, where `OnAppearing` triggers `RefreshAuthStateCommand`, which calls `IsLoggedInAsync()` — now returning `false` — and the UI switches to the logged-out state (Sign In / Create Account rows).

No need to manipulate `Shell.Current.CurrentItem` since we're already on the Profile tab.

### 4. Confirmation dialog

Use `Application.Current.MainPage.DisplayAlert()` with localized title, message, and accept/cancel buttons. This is consistent with platform conventions and requires no custom UI.

### 5. Button placement on Account Profile page

Place the sign-out button at the bottom of the Account Profile page in a `VerticalStackLayout` below the existing placeholder content. Use the app's destructive-action styling: red text, no background fill, to visually distinguish it from primary actions. Keep the placeholder headline for now — the full Account Profile screen will be built separately.

### 6. Sign Out row on Profile tab

Add a "Sign Out" row in the logged-in account section of the Profile page, directly below the "Account Profile" row. It follows the same list-row pattern (Grid with label + chevron, tappable, with divider). The row uses red/Primary text color to visually mark it as a destructive action. It reuses the same `LogoutAsync` flow: confirmation dialog → logout → auth state refresh.

The `SignOutCommand` is added to `ProfileViewModel` since it already has `IAuthService`. After logout, instead of navigating, it just calls `RefreshAuthState()` to switch the UI to logged-out state (the user is already on the Profile tab).

## Risks / Trade-offs

- **[Server unreachable during logout]** → Mitigation: Local tokens are always cleared. The server-side refresh token will eventually expire on its own.
- **[No loading indicator during sign-out]** → The API call + local clear is fast enough that a spinner isn't needed. If the network call hangs, the local clear still runs and navigation proceeds. Keep it simple for now.
