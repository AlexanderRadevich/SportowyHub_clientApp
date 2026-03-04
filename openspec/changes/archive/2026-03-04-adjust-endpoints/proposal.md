## Why

The MAUI client and the Symfony backend have drifted in several areas after the REFACTOR-037 backend migration: response shape mismatches in listings and favorites, a missing `serial_id` field on `FavoriteItem`, a trailing-slash inconsistency on the create-listing and create-conversation endpoints, and a missing `total` field on the conversations list response. Aligning the client now prevents silent data-loss bugs and makes integration tests reliable.

## What Changes

- Fix `FavoriteItem` DTO: add `serial_id` field — backend returns it, client drops it silently
- Fix `FavoritesListResponse` shape: backend wraps items under `items`, client DTO uses `Items` (snake_case: `items`) — already OK via JSON context, but `FavoriteActionResponse` is missing `favorites_count`
- Fix `ConversationsResponse`: backend returns `items` (no `total`), client DTO has `Limit` and `Offset` but no `Total` — matches; verify `total` is absent so no mismatch
- Fix `CreateConversationRequest` endpoint trailing slash: client uses `POST /api/private/conversations/` (trailing slash), backend route is `/api/private/conversations` — trailing slash causes 308 redirect on some servers; remove it
- Fix `CreateListingRequest` endpoint trailing slash: client uses `POST /api/private/listings/` (trailing slash), backend route is `/api/private/listings` — same redirect risk; remove it
- Fix `MediaService` upload endpoint trailing slash: client uses `POST /api/private/media/` (trailing slash), backend route is `/api/private/media` — remove trailing slash
- Fix `ListingDetail` model: backend returns `last_moderator_comment` (conditionally), client DTO has `LastModeratorComment` — already present, no change needed (confirm)
- Fix `FavoriteActionResponse`: backend returns `{ status, favorites_count }`, client model is missing `FavoritesCount` field
- Fix `ThemeSyncService.UpdatePreferencesAsync`: backend accepts both `PUT` and `PATCH`, client uses `PATCH` — OK, no change needed; but response shape from backend is `{ success, theme_mode, color_scheme }`, client deserializes into `ThemePreferences(ThemeMode, ColorScheme)` — `success` field is ignored, which is fine
- Add `serial_id` to `FavoriteItem` DTO so it is available for navigation/display
- Verify `DsarService` endpoint alignment: client uses `GET /api/private/dsar`, `POST /api/private/dsar/export`, `POST /api/private/dsar/delete` — backend matches exactly, no change needed

## Capabilities

### New Capabilities

- `endpoint-alignment`: Corrects HTTP endpoint paths (trailing slash removal) and DTO field gaps so the MAUI client matches the Symfony `/api/private` and `/api/v1` contracts exactly

### Modified Capabilities

- `auth-api-models`: `FavoriteActionResponse` gains `FavoritesCount`; `FavoriteItem` gains `SerialId`

## Impact

- `SportowyHub.App/Services/ListingManagement/ListingManagementService.cs` — remove trailing slash from create URL
- `SportowyHub.App/Services/Messaging/MessagingService.cs` — remove trailing slash from create-conversation URL
- `SportowyHub.App/Services/Media/MediaService.cs` — remove trailing slash from upload URL
- `SportowyHub.App/Models/Api/FavoriteItem.cs` — add `SerialId` field
- `SportowyHub.App/Models/Api/FavoriteActionResponse.cs` — add `FavoritesCount` field
- `SportowyHub.App/Services/Api/SportowyHubJsonContext.cs` — no change needed (types already registered)
- No backend changes required; all fixes are client-side only
