## Why

The Account Profile page currently displays user data in read-only mode. Users need the ability to edit their personal information (name, phone, locale, notification preferences, quiet hours) via `PUT /api/private/profile`. The backend endpoint already exists — the client just needs the UI and service integration.

## What Changes

- Add `UpdateProfileRequest` and `UpdateProfileAccountRequest` request DTOs matching the API body structure
- Add `UpdateProfileAsync` method to `IAuthService`/`AuthService` calling `PUT /api/private/profile`
- Create `EditProfilePage` with form inputs for editable fields: first name, last name, phone, locale, notifications toggle, quiet hours start/end
- Create `EditProfileViewModel` with field validation, save/cancel commands, loading/error states
- Add "Edit" navigation from `AccountProfilePage` to `EditProfilePage`
- Register `edit-profile` Shell route and DI registrations
- Add localization strings for edit form labels, buttons, and validation messages (4 languages)

## Capabilities

### New Capabilities
- `edit-profile`: Edit profile page UI, ViewModel, navigation, and form validation

### Modified Capabilities
- `account-profile-placeholder`: Add "Edit" button/navigation to the read-only profile page
- `auth-api-models`: New `UpdateProfileRequest` and `UpdateProfileAccountRequest` DTOs, JSON context registrations
- `auth-service`: New `UpdateProfileAsync` method on `IAuthService`/`AuthService`

## Impact

- **New models**: `UpdateProfileRequest.cs`, `UpdateProfileAccountRequest.cs`
- **New view**: `Views/Profile/EditProfilePage.xaml` + code-behind
- **New ViewModel**: `ViewModels/EditProfileViewModel.cs`
- **Service**: `IAuthService.cs`, `AuthService.cs` — add `UpdateProfileAsync`
- **JSON context**: `SportowyHubJsonContext.cs` — register new request types
- **Navigation**: `AppShell.xaml.cs` — register `edit-profile` route
- **DI**: `MauiProgram.cs` — register new page and ViewModel
- **Existing view**: `AccountProfilePage.xaml` — add Edit button
- **Localization**: `AppResources.resx` (4 languages) — new string keys
- **Designer**: `AppResources.Designer.cs` — new properties
