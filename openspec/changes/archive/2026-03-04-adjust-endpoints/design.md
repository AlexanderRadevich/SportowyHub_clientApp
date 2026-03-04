## Context

The Symfony backend underwent REFACTOR-037, migrating all private endpoints from `/api/v1/` to `/api/private/`. The MAUI client was updated accordingly, but several subtle mismatches remain:

1. **Trailing slashes**: `POST /api/private/listings/`, `POST /api/private/conversations/`, and `POST /api/private/media/` in the client have a trailing slash that does not appear in the backend route definitions. On a production server behind nginx or Apache, an extra trailing slash can trigger a 308 redirect with method preservation, or in some configurations silently drop the request body. Symfony's router does not auto-strip the trailing slash for non-`GET` methods in strict mode.

2. **Missing DTO fields**: The backend `FavoritesController::add` response includes `favorites_count`, which is absent from the client `FavoriteActionResponse` record. The backend `FavoritesController::list` returns `serial_id` per item, which is absent from the client `FavoriteItem` record. These fields are silently ignored by `System.Text.Json` source-generated deserialization, meaning features relying on them (e.g. showing current favorites count after an add, or listing serial IDs for navigation) silently receive `0`/`null`.

All fixes are client-side only. No backend changes are required.

## Goals / Non-Goals

**Goals:**
- Remove trailing slash from all non-`GET` request URIs in `ListingManagementService`, `MessagingService`, and `MediaService`
- Add `FavoritesCount` to `FavoriteActionResponse`
- Add `SerialId` to `FavoriteItem`
- Confirm no other DTO/endpoint mismatches exist (auth, sections, locale, DSAR, phone verification, theme)

**Non-Goals:**
- Changing any backend code
- Refactoring `RequestProvider` or the HTTP client pipeline
- Adding retry/resilience policies (separate concern)
- Changing pagination strategy for any endpoint

## Decisions

### Decision 1: Fix trailing slash at the call site, not in `RequestProvider`

**Chosen**: Remove the trailing slash directly from each service's URI string literal.

**Alternative considered**: Strip trailing slashes inside `RequestProvider.PostAsync` before sending.

**Rationale**: Centralising the strip in `RequestProvider` would mask future bugs where a developer accidentally includes a trailing slash. Fixing at the call site makes the correct URI explicit and reviewable at a glance. There are only three affected call sites.

### Decision 2: Add `FavoritesCount` as a nullable `int?` in `FavoriteActionResponse`

**Chosen**: `int? FavoritesCount` — nullable because if the backend ever omits the field, deserialization should not fail.

**Alternative considered**: `int FavoritesCount` (non-nullable, default 0).

**Rationale**: The backend currently always returns this field, but defensive nullability costs nothing with `System.Text.Json` source generation and avoids a future breaking deserialization if the field is dropped.

### Decision 3: Add `SerialId` as `int` in `FavoriteItem`

**Chosen**: `int SerialId` — the backend always serialises this field.

**Alternative considered**: `int?` nullable.

**Rationale**: `serial_id` is a non-nullable database sequence column in the backend entity. Making it non-nullable in the client DTO makes intent clear and avoids null-checks at usage sites.

## Risks / Trade-offs

- **[Risk] Trailing slash fix causes regression on local dev if a proxy normalises URLs** → The fix is always correct per HTTP spec; no mitigation needed beyond running existing tests.
- **[Risk] `FavoritesCount` field name collides with property if `FavoriteActionResponse` is extended** → Low risk; the record is a simple DTO. Adding it as the last positional parameter in the record maintains backward compatibility with any existing callers that construct the record by position.

## Migration Plan

1. Update three service files (remove trailing slashes).
2. Update two model files (add fields).
3. `SportowyHubJsonContext` does not need to change — types are already registered.
4. Run `dotnet build` to catch any compilation issues from record positional parameter changes.
5. Run `dotnet test` to verify no unit test regressions.
6. No data migration, no rollback concern — all changes are additive or corrective string literals.
