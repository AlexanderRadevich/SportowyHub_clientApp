## 1. Infrastructure (RequestProvider)

- [x] 1.1 Add `PutAsync<TRequest, TResponse>` overload to `IRequestProvider` interface
- [x] 1.2 Implement `PutAsync<TRequest, TResponse>` in `RequestProvider` (same pattern as `PostAsync`)

## 2. Models & Serialization

- [x] 2.1 Create `UpdateProfileAccountRequest` record in `Models/Api/UpdateProfileAccountRequest.cs` with `FirstName`, `LastName`, `NotificationsEnabled`, `QuietHoursStart`, `QuietHoursEnd`
- [x] 2.2 Create `UpdateProfileRequest` record in `Models/Api/UpdateProfileRequest.cs` with `Phone` and `Account` (UpdateProfileAccountRequest?)
- [x] 2.3 Register `UpdateProfileRequest` and `UpdateProfileAccountRequest` in `SportowyHubJsonContext`

## 3. Auth Service

- [x] 3.1 Add `Task<UserProfile?> UpdateProfileAsync(UpdateProfileRequest request)` to `IAuthService`
- [x] 3.2 Implement `UpdateProfileAsync` in `AuthService` — get token, call `PutAsync<UpdateProfileRequest, UserProfile>` on `/api/private/profile`

## 4. ViewModel

- [x] 4.1 Create `EditProfileViewModel` with observable properties: `FirstName`, `LastName`, `Phone`, `NotificationsEnabled`, `QuietHoursStart`, `QuietHoursEnd`, `IsLoading`, `GeneralError`, and field error properties
- [x] 4.2 Implement `IQueryAttributable` to receive current `UserProfile` and pre-populate form fields
- [x] 4.3 Add quiet hours HH:mm validation with localized error messages
- [x] 4.4 Implement `SaveCommand` — build `UpdateProfileRequest`, call `UpdateProfileAsync`, handle success (toast + navigate back) and errors (field-level + general)

## 5. View (XAML)

- [x] 5.1 Create `EditProfilePage.xaml` with form layout: Entry fields for first name, last name, phone; Switch for notifications; Entry fields for quiet hours start/end; Save button; ActivityIndicator; error labels
- [x] 5.2 Create `EditProfilePage.xaml.cs` code-behind with ViewModel injection

## 6. Navigation & DI

- [x] 6.1 Register `EditProfilePage` and `EditProfileViewModel` as transient in `MauiProgram.cs`
- [x] 6.2 Register `edit-profile` route in `AppShell.xaml.cs`
- [x] 6.3 Add Edit button to `AccountProfilePage.xaml` (toolbar or header) that navigates to `edit-profile` with current profile data
- [x] 6.4 Add static property or navigation helper to pass `UserProfile` to `EditProfileViewModel`

## 7. Localization

- [x] 7.1 Add localization keys to `AppResources.resx` (pl): `EditProfileTitle`, `EditProfileFirstName`, `EditProfileLastName`, `EditProfilePhone`, `EditProfileNotifications`, `EditProfileQuietHoursStart`, `EditProfileQuietHoursEnd`, `EditProfileSave`, `EditProfileSuccess`, `EditProfileError`, `EditProfileInvalidTime`
- [x] 7.2 Add same keys to `AppResources.en.resx`
- [x] 7.3 Add same keys to `AppResources.uk.resx`
- [x] 7.4 Add same keys to `AppResources.ru.resx`
- [x] 7.5 Add properties to `AppResources.Designer.cs`

## 8. Verification

- [x] 8.1 Build the project and fix any compilation errors
