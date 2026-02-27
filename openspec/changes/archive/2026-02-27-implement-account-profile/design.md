## Context

The Account Profile page is a placeholder with a title label and Sign Out button. The `AccountProfileViewModel` currently only has `IAuthService` for sign-out. The backend `GET /api/private/profile` endpoint returns a flat JSON payload with user info, verification status, and preferences. The app uses `System.Text.Json` source generation (`SportowyHubJsonContext`) with `SnakeCaseLower` naming policy, and `IRequestProvider.GetAsync<T>()` for typed GET requests.

## Goals / Non-Goals

**Goals:**
- Display all relevant user profile fields from `GET /api/private/profile` in a read-only grouped layout
- Handle loading, error, and empty/null states gracefully
- Preserve the existing Sign Out button
- Follow existing patterns: MVVM, source-gen JSON, DI, localized strings

**Non-Goals:**
- Editing profile fields (deferred to a later change)
- Avatar upload or image loading (show initials/icon placeholder)
- Caching profile data locally
- Pull-to-refresh (can be added later)

## Decisions

### 1. `UserProfile` response model — flat record

Create a single `UserProfile` record matching the flat `/api/private/profile` JSON structure. No nested sub-objects needed. All nullable fields use `?` types. Register in `SportowyHubJsonContext`.

```
UserProfile: Id, Email, FirstName, LastName, Locale, AvatarUrl,
             NotificationsEnabled, QuietHoursStart, QuietHoursEnd,
             Phone, PhoneVerified, EmailVerified, TrustLevel
```

Using a record (not class) for immutability — this is read-only data.

Note: The response has no `full_name` field. Display name is computed from `FirstName`/`LastName` in the ViewModel, falling back to email.

### 2. `GetProfileAsync()` on `IAuthService`

Add `Task<UserProfile?> GetProfileAsync()` to `IAuthService`. Implementation reads the access token from SecureStorage and calls `_requestProvider.GetAsync<UserProfile>("/api/private/profile", token)`. Returns `null` on any error (auth failure, network, etc.) — the ViewModel handles null as an error state.

Alternatives considered:
- A separate `IProfileService` — overkill for one method; `IAuthService` already owns the token and user-related concerns.
- Returning `AuthResult<UserProfile>` — unnecessary complexity for a read-only GET; null is sufficient to indicate failure.

### 3. ViewModel loads on page appearance

`AccountProfileViewModel` gains a `LoadProfileCommand` executed from code-behind's `OnAppearing`. The command sets `IsLoading = true`, calls `GetProfileAsync()`, populates observable properties, and sets `IsLoading = false`. On failure, sets `HasError = true`.

Observable properties exposed: `IsLoading`, `HasError`, `Profile` (the `UserProfile` object). Computed properties: `DisplayName` (from `FirstName`/`LastName` or email fallback). The XAML binds directly to `Profile.Email`, `Profile.TrustLevel`, etc. using compiled bindings with `x:DataType`.

### 4. Page layout: header + grouped sections + Sign Out

Replace the placeholder with a `ScrollView` containing:

1. **Header area**: Avatar circle placeholder (person icon), display name (`FirstName LastName`, falling back to email), trust level badge
2. **Contact section**: Email (with verified indicator), Phone (with verified indicator)
3. **Account section**: Trust level row
4. **Sign Out button** at the bottom (preserved from current implementation)

Each section uses the existing `SectionHeader` style and list-row pattern (Grid, 48px height, 16px padding, border separator) matching the Profile tab's styling.

Null fields display a localized "Not set" placeholder in `TextSecondary` color.

### 5. Verification status display

Show a simple text indicator next to email/phone: "Verified" in green or "Not verified" in `TextSecondary`. No icons for now — keeps it simple and avoids adding new image assets.

## Risks / Trade-offs

- **[Token expired when loading profile]** → `GetProfileAsync` returns null, ViewModel shows error state with a retry option. The user can navigate back and re-authenticate.
- **[Many null fields on new accounts]** → "Not set" placeholder text keeps the layout consistent. All profile rows are always visible regardless of null values.
- **[No caching]** → Profile is fetched every time the page appears. This is acceptable for now since it's a single lightweight GET call.
