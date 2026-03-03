## Context

The SportowyHub MAUI client currently integrates with 13 backend API endpoints across 3 services (`AuthService`, `ListingsService`, `FavoritesService`). The Symfony backend received a major update on 2026-03-02 that:

- Changed HTTP methods and query parameters on existing endpoints (breaking)
- Added new fields to existing response/request DTOs
- Introduced 30+ new endpoints across 12 feature areas

The app uses `RequestProvider` for all HTTP calls with `System.Text.Json` source generation via `SportowyHubJsonContext`. All services follow the interface + implementation + DI singleton pattern.

**Current state:**
- `RequestProvider` supports GET, POST, PUT, DELETE — no PATCH or multipart/form-data
- Search filters use free-text city name — backend now expects `city_id` and `voivodeship_id` integers
- No sections/categories dictionary exists in the app — needed for listing creation and search filters

## Goals / Non-Goals

**Goals:**
- Fix 3 breaking changes so existing features continue working
- Update DTOs for new response fields without losing backward compatibility
- Add `PatchAsync` and multipart upload support to `RequestProvider`
- Establish sections dictionary service as foundation for search and listing features
- Create service layer for all new backend capabilities following existing patterns
- Maintain phased delivery — breaking fixes first, then incremental feature additions

**Non-Goals:**
- Admin endpoints (not relevant for end users)
- SEO endpoint (`/api/v1/listings/{id}/seo`) — not needed in mobile app
- Real-time messaging via WebSocket — HTTP polling is sufficient for v1
- Full UI implementation for all new features in this change — specs define the service and model layer; UI pages will be separate changes
- Backend modifications — this change only adjusts the client

## Decisions

### 1. Phased delivery order

**Phase 1 — Breaking fixes** (must ship first):
- Profile update: PUT → PATCH (requires `PatchAsync` on `RequestProvider`)
- Search: `city` → `city_id`/`voivodeship_id` (requires sections dictionary to map names to IDs)
- Favorites remove: handle 200 OK with body instead of 204 No Content

**Phase 2 — DTO updates + foundation services:**
- Add `User` object to `LoginResponse`, `verification_url` to `RegisterResponse`
- Sections dictionary service (needed by search and listing management)
- Locale endpoint service

**Phase 3 — New authenticated features:**
- Logout endpoint
- Avatar management
- Phone verification
- Theme sync

**Phase 4 — Complex features:**
- Listing management (CRUD + status + resubmit)
- Media upload (requires multipart support)
- Messaging (conversations + messages)
- Report listing
- DSAR compliance

**Rationale:** Breaking fixes first to restore app functionality. Foundation services next because search and listing management depend on sections dictionary. Simple authenticated features before complex multi-screen ones.

### 2. RequestProvider extensions

Add `PatchAsync<TRequest, TResponse>` following the existing `PutAsync` pattern. Add `PostMultipartAsync<TResponse>` for avatar and media uploads using `MultipartFormDataContent`.

**Alternative considered:** Creating separate upload services with raw `HttpClient` — rejected because centralizing HTTP handling in `RequestProvider` keeps auth token injection and error handling consistent.

### 3. Search filter architecture

The search endpoint now requires `city_id` (int) and `voivodeship_id` (int) instead of `city` (string). The app needs a `ISectionsService` that loads sports sections and categories, plus a city/voivodeship lookup.

The `SearchAsync` method signature changes from `string? city` to `int? cityId, int? voivodeshipId`. The `SearchViewModel` must use the sections dictionary for filter selection.

**Alternative considered:** Keeping free-text city search and mapping client-side — rejected because the backend no longer accepts city strings.

### 4. New service organization

Each feature area gets its own service interface + implementation under `Services/`:

```
Services/
  Sections/ISectionsService.cs, SectionsService.cs
  Listings/IListingManagementService.cs, ListingManagementService.cs
  Media/IMediaService.cs, MediaService.cs
  Messaging/IMessagingService.cs, MessagingService.cs
  PhoneVerification/IPhoneVerificationService.cs, PhoneVerificationService.cs
  Moderation/IModerationService.cs, ModerationService.cs
  Dsar/IDsarService.cs, DsarService.cs
  Theme/IThemeService.cs, ThemeService.cs (server-side sync)
  Locale/ILocaleService.cs, LocaleService.cs
```

Listing CRUD goes into a separate `IListingManagementService` rather than extending `IListingsService` — the existing service handles public browsing, the new one handles authenticated CRUD.

**Alternative considered:** Merging into existing `IListingsService` — rejected because it would bloat a focused read-only interface with write operations and different auth requirements.

### 5. OAuth login approach

Use .NET MAUI `WebAuthenticator` for OAuth flows. The app opens the provider's auth page, receives the token callback, then calls `POST /api/v1/auth/oauth/{provider}` with the received `id_token` or `access_token`.

Add `OAuthLoginAsync(string provider, string idToken?, string accessToken?)` to `IAuthService`.

### 6. Logout with server-side revocation

Add `LogoutAsync()` to `IAuthService` that calls `POST /api/v1/logout` before clearing local tokens. This ensures server-side refresh token revocation. Falls back to local-only clear if the network call fails.

## Risks / Trade-offs

**[Search breaking change depends on sections dictionary]** → Phase 1 search fix and Phase 2 sections dictionary must ship together. If sections service is not ready, search with city filter will be broken.

**[OAuth requires platform-specific setup]** → Google Sign-In needs SHA-1 fingerprint in Firebase console; Apple Sign-In needs Apple Developer entitlements; Facebook needs App ID. These are configuration-only but must be done per platform. Mitigation: implement OAuth service layer first, enable providers incrementally.

**[Multipart upload size limits]** → Media uploads allow up to 12MB files. On slow mobile connections this may timeout. Mitigation: use `HttpCompletionOption.ResponseHeadersRead` and consider progress reporting in a future change.

**[Messaging without real-time]** → HTTP polling for messages means users won't see new messages instantly. Mitigation: acceptable for v1; plan WebSocket/SignalR integration as a separate change.

**[18 new DTO types in JSON context]** → Adding many types to `SportowyHubJsonContext` increases build time for source generation. Mitigation: impact is minimal for this scale; monitor build times.
