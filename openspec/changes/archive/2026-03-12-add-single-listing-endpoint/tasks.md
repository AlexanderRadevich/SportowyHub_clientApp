## Tasks

- [x] Add `Task<ListingDetail> GetMyListingAsync(string id, CancellationToken ct = default)` to `IListingManagementService`
- [x] Implement `GetMyListingAsync` in `ListingManagementService` with API call to single-listing endpoint
- [x] Update `CreateEditListingViewModel.LoadExistingListing` to use `GetMyListingAsync` instead of `GetMyListingsAsync` + `FirstOrDefault`
- [x] Remove the full-list fetch from the edit flow
- [x] Build and verify the edit listing flow works correctly
