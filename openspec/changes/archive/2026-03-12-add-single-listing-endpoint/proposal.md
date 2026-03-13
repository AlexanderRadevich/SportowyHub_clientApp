## Why

`CreateEditListingViewModel.cs` fetches ALL user listings via `GetMyListingsAsync()` then filters with `FirstOrDefault` to find a single listing by ID. This wastes bandwidth and memory, especially for users with many listings.

## What Changes

Add a dedicated single-item endpoint `GetMyListingAsync(string id, ct)` to `IListingManagementService` and use it in the edit flow instead of fetching the full list.

## Capabilities

### New
- `GetMyListingAsync` method on `IListingManagementService` for fetching a single listing by ID

### Modified
- `ListingManagementService.cs` — implement the new API call
- `CreateEditListingViewModel.cs` — use single-item fetch instead of full-list fetch and filter

## Impact

- **Performance:** Eliminates unnecessary data transfer when editing a listing
- **Scalability:** Edit flow performance no longer degrades with listing count
- **Risk:** Low — requires corresponding backend endpoint to exist or be created
