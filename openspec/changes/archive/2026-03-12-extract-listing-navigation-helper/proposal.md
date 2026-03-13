# Extract Listing Navigation Helper

## Why

The same query string pattern for navigating to listing detail is repeated in three ViewModels: `HomeViewModel`, `SearchViewModel`, and `FavoritesViewModel`. This duplicates route construction logic and creates a maintenance burden if the route or parameters change.

## What Changes

Extract a `GoToListingDetailAsync` method on `INavigationService` (or as a static extension method) that encapsulates the route and parameter construction. Refactor all three ViewModels to call it.

## Capabilities

### New

- `GoToListingDetailAsync` method on `INavigationService`

### Modified

- `ShellNavigationService.cs` -- implement the new method
- `HomeViewModel.cs` -- replace inline navigation with helper call
- `SearchViewModel.cs` -- replace inline navigation with helper call
- `FavoritesViewModel.cs` -- replace inline navigation with helper call

## Impact

- **Scope:** Navigation service interface/implementation, three ViewModels
- **Risk:** Very low -- extracting existing duplicated code
- **Testing:** Navigation calls easier to verify with a single method signature
- **UX:** No user-visible changes
