# Extract Listing Navigation Helper -- Design

## Context

Three ViewModels construct the same query string pattern to navigate to listing detail. The route name and parameter keys are hardcoded in each location.

## Goals / Non-Goals

### Goals

- Single method for listing detail navigation
- Route name and parameter keys defined in one place
- All three ViewModels use the shared method

### Non-Goals

- Generalizing navigation helpers for all routes (only listing detail for now)
- Changing the navigation route or parameters

## Decisions

- Add `GoToListingDetailAsync(int listingId)` to `INavigationService` interface
- Implement in `ShellNavigationService` with the existing route and query parameter pattern
- Prefer interface method over static extension to maintain testability via mocks
- ViewModels call `_navigationService.GoToListingDetailAsync(id)` instead of building the route inline

## Risks / Trade-offs

- **Interface expansion:** Adding a domain-specific method to a general navigation interface. Acceptable since listing detail is the primary navigation target; if more accumulate, consider a separate `IListingNavigator`.
- **Breaking change to interface:** Any custom `INavigationService` implementations must add the new method
