## ADDED Requirements

### Requirement: Favorites page shows paginated list

The `FavoritesPage` SHALL display the user's favorited listings in a paginated `CollectionView` with the same card layout as the Home feed (title, price with currency, city).

#### Scenario: Load favorites on page appearance
- **WHEN** the user navigates to the Favorites tab and is logged in
- **THEN** the page SHALL load the first page of favorites (page=1, per_page=20)
- **THEN** each item SHALL display title, price with currency, and city

#### Scenario: Load more on scroll
- **WHEN** the user scrolls near the bottom and more pages exist
- **THEN** the next page SHALL be loaded and appended to the list

#### Scenario: No more pages
- **WHEN** the current page equals the total pages
- **THEN** no further requests SHALL be made

### Requirement: Pull-to-refresh favorites

The `FavoritesPage` SHALL support pull-to-refresh to reload the favorites list from page 1.

#### Scenario: Pull to refresh
- **WHEN** the user pulls to refresh
- **THEN** the list SHALL reload from page 1
- **THEN** the favorites IDs cache SHALL be refreshed

### Requirement: Empty state when no favorites

The `FavoritesPage` SHALL display an empty state message when the user has no favorites.

#### Scenario: No favorites
- **WHEN** the user has no favorites and is logged in
- **THEN** a localized "No favorites yet" message (`FavoritesEmpty`) SHALL be displayed

### Requirement: Auth-gated favorites page

The `FavoritesPage` SHALL check auth state on appearance. If the user is not logged in, it SHALL show a sign-in prompt instead of the favorites list.

#### Scenario: Not logged in
- **WHEN** the user navigates to the Favorites tab and is not logged in
- **THEN** a localized message "Sign in to save favorites" (`FavoritesSignInPrompt`) SHALL be displayed
- **THEN** a "Sign In" button SHALL navigate to the login page

#### Scenario: Logged in
- **WHEN** the user navigates to the Favorites tab and is logged in
- **THEN** the favorites list SHALL be loaded and displayed

### Requirement: Remove from favorites list

Each item in the favorites list SHALL have a remove action (heart button or swipe) to unfavorite the listing.

#### Scenario: Tap remove on a favorite
- **WHEN** the user taps the heart/remove button on a favorite item
- **THEN** `IFavoritesService.RemoveAsync(id)` SHALL be called
- **THEN** the item SHALL be removed from the list immediately (optimistic)
- **THEN** on API failure, the item SHALL be re-added and a toast error shown

### Requirement: Navigate to listing detail from favorites

Tapping a favorite item SHALL navigate to the listing detail page.

#### Scenario: Tap favorite item
- **WHEN** the user taps a favorite listing card
- **THEN** the app SHALL navigate to `listing-detail?id={item.Id}` via `INavigationService`

### Requirement: Loading indicator

The `FavoritesViewModel` SHALL expose an `IsLoading` property for the initial load indicator.

#### Scenario: Loading state
- **WHEN** favorites are being loaded for the first time
- **THEN** `IsLoading` SHALL be true and an `ActivityIndicator` SHALL be visible
