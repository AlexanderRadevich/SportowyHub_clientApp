## Why

The `sync-api-endpoints` change created all service-layer infrastructure for listing management (`IListingManagementService`, `IMediaService`, `ISectionsService`), but no UI exists to let users manage their listings. Users need to create, edit, publish, and delete listings directly from the app.

## What Changes

- Add "My Listings" page accessible from the Profile hub (new row in Account section, visible when logged in)
- Add a `MyListingSummary` model that includes `Status` and `CreatedAt` fields (the `/api/private/listings/my` endpoint returns these, unlike the public listings endpoint)
- Update `MediaUrls` model to match actual backend response (6 derivative URLs, not `Original`/`Thumbnail`)
- Add Create/Edit Listing page with full form: category picker (two-step via sections), title, description, price, currency, city/voivodeship, content locale, photo management
- Add status management actions (publish, unpublish/hide, resubmit for review, delete) with confirmation dialogs
- Add status filter tabs on My Listings page (All, Draft, Published, Pending Review, Rejected, Hidden)
- Add rejection reason display from `LastModeratorComment`
- Add 4-language localization strings for all new UI elements

## Capabilities

### New Capabilities

- `my-listings-page`: My Listings list page with status filtering, status actions, and navigation to create/edit
- `create-edit-listing-page`: Create and edit listing form with category picker, media upload, and all listing fields

### Modified Capabilities

- `profile-hub`: Add "My Listings" row in the Account section (visible when logged in)
- `listings-api-models`: Add `MyListingSummary` model with `Status` field; update `MediaUrls` to match actual backend derivative URLs

## Impact

- **New pages**: `MyListingsPage`, `CreateEditListingPage` (XAML + code-behind)
- **New ViewModels**: `MyListingsViewModel`, `CreateEditListingViewModel`
- **Models**: New `MyListingSummary`; updated `MediaUrls` record; updated `ListingsResponse` or new `MyListingsResponse`
- **Existing pages**: `ProfilePage.xaml` — add "My Listings" row
- **Navigation**: New Shell routes `my-listings` and `create-edit-listing`
- **DI**: Register new pages and ViewModels as transient in `MauiProgram.cs`
- **Localization**: New `.resx` entries across 4 languages (pl, en, uk, ru)
- **JSON context**: Register new models in `SportowyHubJsonContext`
