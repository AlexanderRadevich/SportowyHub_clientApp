## Why

The backend (Symfony) received a major update on 2026-03-02 introducing new endpoints, changing HTTP methods, and modifying request/response structures. The MAUI client currently consumes 13 endpoints but the backend now exposes 45+. Three existing integrations are broken (wrong HTTP method, changed query parameters, different response codes), several response DTOs are missing new fields, and the app lacks support for entire feature areas the backend now provides (OAuth, messaging, listing CRUD, phone verification, etc.).

## What Changes

### Breaking fixes (existing endpoints)
- **BREAKING** Profile update: change HTTP method from `PUT` to `PATCH` on `/api/private/profile`
- **BREAKING** Search: replace `city` (string) parameter with `city_id` (int), add `voivodeship_id` (int) parameter
- **BREAKING** Favorites remove: handle `200 OK` with body instead of `204 No Content` from `DELETE /api/private/favorites/{id}`

### Field updates (existing endpoints)
- Login response: add `User` object (`id`, `email`, `trust_level`) to `LoginResponse`
- Register response: add optional `verification_url` to `RegisterResponse`
- Profile update request: support `locale` at root level in `UpdateProfileRequest`
- Search: add `test` parameter support

### New endpoint integrations
- OAuth login via `/api/v1/auth/oauth/{provider}` (Google, Facebook, Apple)
- Logout via `POST /api/v1/logout`
- Avatar upload/delete via `/api/private/profile/avatar`
- Listing CRUD: my listings, create, update, delete, status change, resubmit for review
- Media upload/delete for listing images
- Conversations and messaging (create conversation, list, get, send/read messages)
- Phone verification (request code, verify)
- Sections/categories dictionary (sports list, categories per sport)
- Report listing via `/api/v1/moderation/report`
- DSAR: data export request, account deletion request
- Theme preferences sync via `/api/private/theme/preferences`
- Locale endpoint via `GET /api/v1/locale`

## Capabilities

### New Capabilities
- `oauth-login`: OAuth authentication flow for Google, Facebook, and Apple providers
- `logout-endpoint`: Server-side logout with token revocation
- `avatar-management`: Upload and delete profile avatar images
- `listing-management`: Full listing CRUD — create, update, delete, status transitions, resubmit for review
- `media-upload`: Image upload and deletion for listings (JPEG, PNG, HEIC/HEIF, max 12MB)
- `messaging`: Conversations and messages between buyers and sellers
- `phone-verification`: SMS code request and verification for trust level progression
- `sections-dictionary`: Sports sections and category tree for listing creation and filtering
- `report-listing`: User-submitted moderation reports on listings
- `dsar-compliance`: Data export and account deletion requests (GDPR/DSAR)
- `theme-sync`: Server-side theme preference storage and retrieval
- `locale-endpoint`: Available locales discovery from backend

### Modified Capabilities
- `auth-api-models`: Login response adds `user` object; register response adds `verification_url`; profile update changes from PUT to PATCH and adds root-level `locale` field
- `auth-service`: Must use PATCH for profile update; must handle new login/register response fields
- `listings-service`: Search parameters change from `city` string to `city_id`/`voivodeship_id` integers
- `listings-api-models`: Search request model needs `city_id`, `voivodeship_id`; login/register response DTOs need new fields
- `favorites-service`: Remove endpoint now returns 200 with body instead of 204 No Content
- `search-ui`: Search filters must use city/voivodeship IDs instead of city name strings (requires sections dictionary for lookup)

## Impact

- **Services**: `AuthService`, `ListingsService`, `FavoritesService` need fixes; 12 new service interfaces/implementations
- **Models**: 3 existing DTOs need field additions; 30+ new request/response DTOs required
- **JSON context**: `SportowyHubJsonContext` must register all new model types
- **DI registration**: New services, view models, and pages in `MauiProgram.cs`
- **Shell navigation**: New routes for listing management, messaging, phone verification, DSAR pages
- **Localization**: New `.resx` entries for all new feature UI strings (pl, en, uk, ru)
- **Risk**: Breaking changes in auth and search affect all users immediately — these must be fixed first before adding new features
