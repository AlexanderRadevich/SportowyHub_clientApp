## Why

Users want to see how popular a listing is before clicking into it. The backend already tracks view counts (`view_count` column on `listings` table, incremented on each detail page visit, excluding owner views), but the public JSON API does not expose this field. Adding view count display gives users a quick popularity signal and increases engagement with high-traffic listings.

## What Changes

- Backend API responses for `GET /api/v1/listings` and `GET /api/v1/listings/{id}` include a `view_count` field
- The MAUI client parses `view_count` from listing API responses
- Listing cards show a view count indicator (eye icon + count)
- Listing detail page shows the view count

## Capabilities

### New Capabilities

- `listing-views-display`: UI components and logic for rendering view counts on listing cards and the listing detail page

### Modified Capabilities

- `listings-api-models`: Add `ViewCount` (int) property to the listing API model to capture the new `view_count` JSON field
- `listing-card-ui`: Add view count indicator (eye icon + formatted count) to the listing card layout
- `listing-detail`: Display view count on the listing detail page

## Impact

- **Backend (out of scope for MAUI repo)**: `view_count` must be added to API serialization — coordinate with backend team
- **Models**: `Listing` or listing DTO gains a `ViewCount` property
- **UI**: `ListingCard` control and listing detail page updated with view count display
- **Tests**: Unit tests for view count formatting (e.g., 1234 → "1.2k") and model deserialization
