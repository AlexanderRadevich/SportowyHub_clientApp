## Context

The app has a complete service layer for listing management (`IListingManagementService`, `IMediaService`, `ISectionsService`) but no UI. Users access the Profile tab to manage their account; "My Listings" will live here as a new navigation entry point. The backend supports a full listing lifecycle (draft → pending_review → published → hidden → archived → deleted) with trust-level-based moderation.

## Goals / Non-Goals

**Goals:**
- Let users view, create, edit, and delete their own listings
- Support status transitions with appropriate confirmation dialogs
- Integrate photo upload/delete into the create/edit flow
- Display rejection reasons from moderators
- Follow existing app patterns (MVVM, Shell navigation, theming, localization)

**Non-Goals:**
- Location/GPS picker UI (use simple numeric entry for city_id/voivodeship_id for now; full location picker is a separate change)
- Category attribute schema UI (dynamic form based on category attributes — defer to a later change)
- Drag-to-reorder photos (just add/delete for now)
- Real-time moderation status updates (pull-to-refresh is sufficient)

## Decisions

**Decision 1: Separate `MyListingSummary` model instead of reusing `ListingSummary`**
- Rationale: The `/api/private/listings/my` endpoint returns `status` and `created_at` fields that the public endpoint doesn't. Reusing `ListingSummary` would require making it a superset with nullable fields, confusing the public listing contract.
- Alternative: Add optional fields to `ListingSummary` — rejected to keep public/private response models cleanly separated.

**Decision 2: Tab-based status filtering on My Listings page**
- Rationale: The backend filters server-side via `?status=` param. Status tabs are cleaner than a dropdown picker and match common marketplace app patterns. Tabs: All, Draft, Published, Pending, Rejected, Hidden.
- Alternative: Single list with status badges — rejected because users typically want to see specific status groups.

**Decision 3: Single Create/Edit page with mode parameter**
- Rationale: Create and Edit share 95% of the same form. A single `CreateEditListingPage` with an `IsEditMode` flag avoids duplicated XAML. The listing ID query parameter determines the mode.
- Alternative: Two separate pages — rejected as unnecessary duplication.

**Decision 4: Photos uploaded after create, inline during edit**
- Rationale: The media API requires an existing listing ID. For new listings, the flow is: fill form → create listing → upload photos. For existing listings, photos can be managed immediately since the ID exists.
- Alternative: Two-step wizard (create, then photos page) — rejected; a single scrollable form with a photos section at the bottom is simpler.

**Decision 5: Update `MediaUrls` to match actual backend response**
- Rationale: Backend returns `thumb_160`, `thumb_320`, `card_640`, `gallery_1024`, `gallery_1920`, `og_1200x630` — not `Original`/`Thumbnail`. The current model silently fails to deserialize. Fix to match reality.
- Consideration: URLs may be null when listing is not published. Use `thumb_320` for list display and `gallery_1024` for detail view.

**Decision 6: City/Voivodeship as integer entry fields (not pickers)**
- Rationale: No city/voivodeship dictionary endpoint exists yet. Using `Entry` with numeric keyboard for IDs is functional for now. A proper location picker with autocomplete is a separate feature.
- Mitigation: When the location dictionary service is built, swap entries for pickers without changing the ViewModel contract.

## Risks / Trade-offs

- [Risk] `MediaUrls` change is **BREAKING** for any code using the old `Original`/`Thumbnail` properties → Mitigation: No existing UI code references these properties yet (service layer only). Verify with build.
- [Risk] Trust level TL0 users cannot create listings → Mitigation: Show a message explaining phone verification is required, with a link to the phone verification flow.
- [Risk] Listing limit exceeded → Mitigation: Catch the `LISTING_LIMIT_EXCEEDED` error and display a user-friendly message with current limit.
- [Trade-off] City/voivodeship as raw IDs is poor UX → Acceptable as a temporary solution; documented as a non-goal.
