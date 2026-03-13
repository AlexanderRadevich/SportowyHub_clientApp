# Tasks: Fix Stream Leak in AddPhoto

## Tasks

- [x] Add `await using` to the stream from `OpenReadAsync` in `CreateEditListingViewModel.cs`
- [x] Verify the stream is fully consumed within the `await using` scope
- [x] Audit the rest of the file for other undisposed streams from `OpenReadAsync` or similar calls
- [x] Fix any additional undisposed streams found in audit
