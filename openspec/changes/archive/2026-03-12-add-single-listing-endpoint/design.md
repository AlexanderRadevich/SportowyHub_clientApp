## Context

When editing a listing, the app fetches all user listings and filters client-side to find the one being edited. This is inefficient and scales poorly.

## Goals / Non-Goals

**Goals:**
- Add a single-listing fetch method to `IListingManagementService`
- Use it in the edit listing flow to fetch only the needed listing

**Non-Goals:**
- Changing the "my listings" list view (it still uses the bulk endpoint)
- Modifying the backend API (assumes endpoint exists or will be created separately)

## Decisions

- Add `Task<Result<ListingDetail>> GetMyListingAsync(string id, CancellationToken ct)` to `IListingManagementService`
- Backend endpoint: `GET /api/listings/{id}` (confirm with backend team)
- `CreateEditListingViewModel.LoadExistingListing` calls the new method directly
- Keep the existing `GetMyListingsAsync` for the list view

## Risks / Trade-offs

- **Backend dependency:** Requires the single-listing endpoint to exist. If not available, this change is blocked until the backend implements it.
- **Caching:** If listings are cached locally, fetching a single item may miss the cache. Not a concern currently since no caching layer exists.
