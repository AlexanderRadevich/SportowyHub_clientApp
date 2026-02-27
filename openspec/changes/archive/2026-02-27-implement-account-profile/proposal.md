## Why

The Account Profile page is currently a placeholder with just a title label and a Sign Out button. Logged-in users need to see their account details (email, phone, verification status, trust level, name, avatar). The backend `GET /api/private/profile` endpoint already exists and returns all necessary data in a flat JSON structure.

## What Changes

- Create a `UserProfile` response model matching the `GET /api/private/profile` flat JSON structure
- Add `GetProfileAsync()` to `IAuthService` / `AuthService` to fetch user profile with the stored access token
- Expand `AccountProfileViewModel` to load and expose profile data on page appearance
- Replace the placeholder UI with a read-only profile display: avatar placeholder, name/email header, and grouped info sections (contact, account status)
- Add localized labels for all displayed fields (4 languages)
- Register the `UserProfile` model in `SportowyHubJsonContext` for source-generated serialization
- Handle loading, error, and empty states (null fields show as "Not set")

## Capabilities

### New Capabilities
_(none)_

### Modified Capabilities
- `account-profile-placeholder`: Replace placeholder with full read-only profile display, data loading from API, and error/loading states

## Impact

- **New files**: `Models/Api/UserProfile.cs` (response model)
- **Modified files**: `IAuthService.cs`, `AuthService.cs` (new method), `AccountProfileViewModel.cs` (profile loading), `AccountProfilePage.xaml` (full UI), `SportowyHubJsonContext.cs` (register new type), `AppResources.resx` (4 language files), `AppResources.Designer.cs`
- **Backend dependency**: `GET /api/private/profile` (authenticated, returns user profile JSON)
- **No breaking changes**: Sign Out button is preserved in the new layout
