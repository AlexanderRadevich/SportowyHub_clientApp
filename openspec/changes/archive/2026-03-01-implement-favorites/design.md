## Context

The Favorites tab exists as a placeholder. The backend provides 4 private endpoints under `/api/private/favorites` (all require Bearer auth). The client already has patterns for authenticated calls (`IRequestProvider` with token parameter), paginated lists (`HomeViewModel`), and navigation to listing detail.

Backend response shapes (from controller source):
- **GET /ids**: `{ "ids": ["uuid", ...], "count": N }`
- **GET /**: `{ "items": [{ id, title, price, currency, city, status, slug, serial_id }], "total", "page", "per_page", "pages" }`
- **POST /{id}**: `{ "status": "added", "favorites_count": N }` (or 409/400/404 errors)
- **DELETE /{id}**: `{ "status": "removed", "favorites_count": N }`

## Goals / Non-Goals

**Goals:**
- Paginated favorites list with pull-to-refresh on FavoritesPage
- Heart toggle on ListingDetailPage (add/remove with optimistic UI)
- In-memory favorite IDs cache for instant "is favorited" checks
- Auth-gated: show sign-in prompt when not logged in
- Remove from favorites directly from the favorites list

**Non-Goals:**
- Heart button on Home feed / Search result cards (future enhancement)
- Offline-first with SQLite caching
- Favorite count badge on the tab bar
- Swipe-to-delete gesture on favorites list

## Decisions

### 1. In-memory favorite IDs cache in FavoritesService

Maintain a `HashSet<string>` of favorite listing IDs in the singleton `FavoritesService`. Load IDs on first access via `GET /ids`. This allows instant `IsFavorite(id)` checks without network calls.

**Why:** The IDs endpoint returns a lightweight array (typical user has <100 favorites). A HashSet provides O(1) lookups for the heart button state on detail pages.

**Cache invalidation:** Update the set locally on add/remove. Full refresh on pull-to-refresh or when navigating to FavoritesPage.

### 2. Optimistic UI for favorite toggle

When the user taps the heart:
1. Immediately update the UI (filled/unfilled heart)
2. Fire the API call
3. On failure, revert the UI and show a toast error

**Why:** Waiting for network response before updating the heart feels sluggish.

### 3. FavoritesPage uses page-based pagination (not offset)

The backend uses `page`/`per_page` (not offset/limit like the listings endpoint). The ViewModel tracks `_currentPage` and `_totalPages`.

### 4. Auth check via IAuthService.IsLoggedInAsync()

FavoritesViewModel checks auth state on appearing. If not logged in, shows a "Sign in" prompt with a button navigating to the login page. If logged in, loads favorites.

### 5. Reuse ListingSummary-style card layout

The favorites list items have the same shape as home feed cards (id, title, price, currency, city). Reuse the same DataTemplate pattern.

### 6. FavoriteItem model maps to backend response

Create a `FavoriteItem` record matching the backend `items[]` shape: `id`, `title`, `price`, `currency`, `city`, `status`, `slug`, `serialId`.

## Risks / Trade-offs

- **Stale cache after login/logout** → Clear the IDs cache on logout. Reload on FavoritesPage appearing.
- **Race condition on rapid toggle** → Disable the button during the API call. Optimistic UI still shows instant feedback.
- **409 on add (already favorited)** → Treat as success (idempotent from the user's perspective), update cache.
