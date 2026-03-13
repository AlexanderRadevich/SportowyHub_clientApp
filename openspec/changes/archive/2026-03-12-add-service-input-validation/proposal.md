# Add Service Input Validation

## Why

Several services pass user input directly to the API without validation. `MessagingService.SendMessageAsync` does not check the message body, `FavoritesService.AddAsync` does not validate listingId, `ListingManagementService.UpdateListingAsync` does not guard the id, and `MediaService.UploadAsync` does not check listingId or fileName. Invalid input reaches the network layer before failing.

## What Changes

Add `ArgumentException.ThrowIfNullOrWhiteSpace` and similar guard clauses at service method entry points to fail fast with clear error messages.

## Capabilities

### New

- Guard clauses at all service method entry points that accept user-controlled input

### Modified

- `MessagingService.cs` -- validate message body
- `FavoritesService.cs` -- validate listingId
- `ListingManagementService.cs` -- validate id parameter
- `MediaService.cs` -- validate listingId and fileName

## Impact

- **Scope:** Four service files, plus new unit tests
- **Risk:** Very low -- additive guards, no behavioral change for valid input
- **Testing:** New tests verify `ArgumentException` for null/empty inputs
- **UX:** No change for valid input; invalid input fails faster with clearer messages
