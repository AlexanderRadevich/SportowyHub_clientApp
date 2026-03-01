## Why

The Favorites tab is a placeholder showing only a title label. The backend already has full favorite endpoints (`/api/private/favorites`). Users need to save listings they're interested in and quickly access them later.

## What Changes

- Create `IFavoritesService` to call the 4 backend endpoints (list, ids, add, remove)
- Add API response models for favorites (`FavoritesIdsResponse`, `FavoritesListResponse`, `FavoriteActionResponse`)
- Build out `FavoritesPage` with a paginated list of favorited listings (same card layout as Home feed)
- Add a `FavoritesViewModel` with load, refresh, pull-to-refresh, and remove-from-favorites
- Add a favorite/unfavorite heart button on `ListingDetailPage`
- Cache favorite IDs in memory for instant UI state (is this listing favorited?)
- Handle unauthenticated state — show a "Sign in to use favorites" message on the Favorites tab
- Add localized strings for favorites-related UI text

## Capabilities

### New Capabilities
- `favorites-service`: API service layer for favorites endpoints — IFavoritesService, response models, in-memory IDs cache, DI registration
- `favorites-page`: FavoritesPage UI and FavoritesViewModel — paginated list, pull-to-refresh, empty state, auth-gated, remove action
- `favorites-toggle`: Heart button on ListingDetailPage to add/remove favorites with optimistic UI update

### Modified Capabilities
- `listing-detail`: Add favorite toggle button to the listing detail page

## Impact

- New `Services/Favorites/` — `IFavoritesService`, `FavoritesService`
- New `Models/Api/` — `FavoritesIdsResponse`, `FavoritesListResponse`, `FavoriteActionResponse`, `FavoriteItem`
- New `ViewModels/FavoritesViewModel.cs`
- Modified `Views/Favorites/FavoritesPage.xaml(.cs)` — replace placeholder
- Modified `Views/Listings/ListingDetailPage.xaml` — add heart button
- Modified `ViewModels/ListingDetailViewModel.cs` — add favorite toggle command
- Modified `MauiProgram.cs` — register new service, ViewModel, page
- Modified `SportowyHubJsonContext.cs` — add new response types
- Modified `.resx` files — add localized strings
