## 1. RequestProvider Extensions

- [x] 1.1 Add `PatchAsync<TRequest, TResponse>` method to `IRequestProvider` and `RequestProvider` following the existing `PutAsync` pattern but using `HttpMethod.Patch`
- [x] 1.2 Add `PostMultipartAsync<TResponse>` method to `IRequestProvider` and `RequestProvider` that accepts `MultipartFormDataContent` and an auth token, returns deserialized response

## 2. Breaking Fix — Profile Update PUT → PATCH

- [x] 2.1 Update `AuthService.UpdateProfileAsync` to call `PatchAsync` instead of `PutAsync` for `PATCH /api/private/profile`
- [x] 2.2 Add `Locale` (string?) property to `UpdateProfileRequest` model
- [x] 2.3 Verify profile update works end-to-end with the backend

## 3. Breaking Fix — Search Parameters city → city_id

- [x] 3.1 Change `IListingsService.SearchAsync` signature: replace `string? city` with `int? cityId, int? voivodeshipId`
- [x] 3.2 Update `ListingsService.SearchAsync` to build query string with `city_id` and `voivodeship_id` integer params instead of `city` string
- [x] 3.3 Update `SearchViewModel` to pass `cityId`/`voivodeshipId` instead of city name string (use null for now until sections dictionary is available)
- [x] 3.4 Verify search works end-to-end with the backend

## 4. Breaking Fix — Favorites Remove Response

- [x] 4.1 Update `FavoritesService.RemoveAsync` to handle both `200 OK` with body and `204 No Content` as success (do not require a specific response body for cache update)
- [x] 4.2 Verify favorites remove works end-to-end with the backend

## 5. DTO Updates — Login & Register Response Fields

- [x] 5.1 Create `LoginUser` record with `Id` (int), `Email` (string), `TrustLevel` (string)
- [x] 5.2 Add `User` (LoginUser?) property to `LoginResponse`
- [x] 5.3 Add `VerificationUrl` (string?) property to `RegisterResponse`
- [x] 5.4 Register `LoginUser` in `SportowyHubJsonContext`
- [x] 5.5 Update `AuthService.LoginAsync` to store user info from `LoginResponse.User` in `SecureStorage["auth_user"]` when present

## 6. Sections Dictionary Service

- [x] 6.1 Create `Section` record (`Id`, `Slug`, `Name`) and `SectionsResponse` record (`Sections`, `Locale`)
- [x] 6.2 Create `Category` record (`Id`, `Slug`, `Name`, `SectionId`) and `CategoriesResponse` record (`Categories`, `Locale`)
- [x] 6.3 Register all section/category models in `SportowyHubJsonContext`
- [x] 6.4 Create `ISectionsService` interface with `GetSectionsAsync` and `GetCategoriesAsync`
- [x] 6.5 Implement `SectionsService` calling `GET /api/v1/sections` and `GET /api/v1/sections/{id}/categories`
- [x] 6.6 Register `ISectionsService` as singleton in `MauiProgram.cs`

## 7. Locale Endpoint Service

- [x] 7.1 Create `LocaleInfo` record (`Locale`, `AvailableLocales`, `DefaultLocale`)
- [x] 7.2 Register `LocaleInfo` in `SportowyHubJsonContext`
- [x] 7.3 Create `ILocaleService` interface with `GetLocaleInfoAsync`
- [x] 7.4 Implement `LocaleService` calling `GET /api/v1/locale`
- [x] 7.5 Register `ILocaleService` as singleton in `MauiProgram.cs`

## 8. Logout Endpoint

- [x] 8.1 Add `LogoutAsync()` method to `IAuthService`
- [x] 8.2 Implement `LogoutAsync` in `AuthService`: POST `/api/v1/logout` with Bearer token, then `ClearAuthAsync()` + `IFavoritesService.ClearCache()`, clear local data even on network failure
- [x] 8.3 Update existing logout UI flow to call `LogoutAsync()` instead of only `ClearAuthAsync()`

## 9. Avatar Management

- [x] 9.1 Create `AvatarResponse` record (`AvatarUrl`, `AvatarThumbnailUrl`) and register in `SportowyHubJsonContext`
- [x] 9.2 Add `UploadAvatarAsync(Stream, string, CancellationToken)` to `IAuthService` — POST multipart to `/api/private/profile/avatar`
- [x] 9.3 Add `DeleteAvatarAsync(CancellationToken)` to `IAuthService` — DELETE `/api/private/profile/avatar`
- [x] 9.4 Implement both methods in `AuthService` using `PostMultipartAsync` and `DeleteAsync`

## 10. Phone Verification Service

- [x] 10.1 Create request/response models: `PhoneVerificationRequestRequest`, `PhoneVerificationRequestResponse`, `PhoneVerificationVerifyRequest`, `PhoneVerificationResult`
- [x] 10.2 Register all phone verification models in `SportowyHubJsonContext`
- [x] 10.3 Create `IPhoneVerificationService` interface with `RequestVerificationAsync` and `VerifyCodeAsync`
- [x] 10.4 Implement `PhoneVerificationService` calling `POST /api/private/phone-verification/request` and `POST /api/private/phone-verification/verify`
- [x] 10.5 Register `IPhoneVerificationService` as singleton in `MauiProgram.cs`

## 11. Theme Sync Service

- [x] 11.1 Create `ThemePreferences` record (`ThemeMode`, `ColorScheme`) and register in `SportowyHubJsonContext`
- [x] 11.2 Create `IThemeSyncService` interface with `GetPreferencesAsync` and `UpdatePreferencesAsync`
- [x] 11.3 Implement `ThemeSyncService` calling `GET /api/private/theme/preferences` and `PATCH /api/private/theme/preferences`
- [x] 11.4 Register `IThemeSyncService` as singleton in `MauiProgram.cs`

## 12. OAuth Login

- [x] 12.1 Create `OAuthLoginRequest` record (`IdToken`, `AccessToken`) and register in `SportowyHubJsonContext`
- [x] 12.2 Add `OAuthLoginAsync(string provider, string? idToken, string? accessToken)` to `IAuthService`
- [x] 12.3 Implement `OAuthLoginAsync` in `AuthService`: POST to `/api/v1/auth/oauth/{provider}`, store tokens and user info on success
- [ ] 12.4 Implement `WebAuthenticator` integration for Google provider (platform URL scheme config, token extraction)

## 13. Listing Management Service

- [x] 13.1 Create request models: `CreateListingRequest`, `UpdateListingRequest` and register in `SportowyHubJsonContext`
- [x] 13.2 Create response models: `UpdateListingResponse`, `UpdateStatusResponse`, `ResubmitResponse` and register in `SportowyHubJsonContext`
- [x] 13.3 Create `IListingManagementService` interface with `GetMyListingsAsync`, `CreateListingAsync`, `UpdateListingAsync`, `DeleteListingAsync`, `UpdateStatusAsync`, `ResubmitForReviewAsync`
- [x] 13.4 Implement `ListingManagementService` calling the 6 private listing endpoints with Bearer auth
- [x] 13.5 Register `IListingManagementService` as singleton in `MauiProgram.cs`

## 14. Media Upload Service

- [x] 14.1 Create `MediaItem` record, `MediaUrls` record, and register in `SportowyHubJsonContext`
- [x] 14.2 Create `IMediaService` interface with `UploadAsync` and `DeleteAsync`
- [x] 14.3 Implement `MediaService`: `UploadAsync` uses `PostMultipartAsync` to POST `/api/private/media/` with `listing_id`, `file`, `sort_order`; `DeleteAsync` sends DELETE `/api/private/media/{id}`
- [x] 14.4 Register `IMediaService` as singleton in `MauiProgram.cs`

## 15. Messaging Service

- [x] 15.1 Create models: `Conversation`, `ConversationsResponse`, `Message`, `MessagesResponse`, `CreateConversationRequest`, `SendMessageRequest` and register in `SportowyHubJsonContext`
- [x] 15.2 Create `IMessagingService` interface with `CreateConversationAsync`, `GetConversationsAsync`, `GetConversationAsync`, `GetMessagesAsync`, `SendMessageAsync`
- [x] 15.3 Implement `MessagingService` calling the 5 private conversation/message endpoints with Bearer auth
- [x] 15.4 Register `IMessagingService` as singleton in `MauiProgram.cs`

## 16. Report Listing Service

- [x] 16.1 Create `ReportListingRequest` and `ReportResponse` models and register in `SportowyHubJsonContext`
- [x] 16.2 Create `IModerationService` interface with `ReportListingAsync`
- [x] 16.3 Implement `ModerationService` calling `POST /api/v1/moderation/report`
- [x] 16.4 Register `IModerationService` as singleton in `MauiProgram.cs`

## 17. DSAR Compliance Service

- [x] 17.1 Create models: `DsarRequestItem`, `DsarListResponse`, `DsarRequestResponse` and register in `SportowyHubJsonContext`
- [x] 17.2 Create `IDsarService` interface with `GetRequestsAsync`, `RequestDataExportAsync`, `RequestAccountDeletionAsync`
- [x] 17.3 Implement `DsarService` calling the 3 private DSAR endpoints with Bearer auth
- [x] 17.4 Register `IDsarService` as singleton in `MauiProgram.cs`

## 18. Verification & Cleanup

- [x] 18.1 Run `dotnet build` and fix any compilation errors
- [x] 18.2 Run `dotnet test` and fix any test failures
- [x] 18.3 Run `get_diagnostics` via MCP to check for warnings
- [x] 18.4 Verify all new models are registered in `SportowyHubJsonContext`
