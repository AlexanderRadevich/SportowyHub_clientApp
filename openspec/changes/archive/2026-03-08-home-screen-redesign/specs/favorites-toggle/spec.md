## ADDED Requirements

### Requirement: Favorite toggle on listing cards
The `ListingCardView` SHALL display a heart icon button. The card SHALL accept a `ToggleFavoriteCommand` bindable property from the parent ViewModel. The heart state SHALL reflect `IFavoritesService.IsFavorite(id)`.

#### Scenario: Heart shown on Hot Picks card
- **WHEN** a listing card renders in the Hot Picks section
- **THEN** the heart icon SHALL be visible in the top-right corner of the image area

#### Scenario: Heart shown on All Products card
- **WHEN** a listing card renders in the All Products grid
- **THEN** the heart icon SHALL be visible in the top-right corner of the image area

#### Scenario: Toggle favorite from home screen card
- **WHEN** the user taps the heart icon on a home screen listing card
- **THEN** the favorite state SHALL toggle with optimistic UI update
- **THEN** on failure, the state SHALL revert and a toast error SHALL be shown

#### Scenario: Not logged in user taps heart on card
- **WHEN** a non-authenticated user taps the heart icon on a listing card
- **THEN** the app SHALL navigate to the login page

### Requirement: HomeViewModel favorite management
The `HomeViewModel` SHALL expose a `ToggleFavoriteCommand` that accepts a `ListingSummary` parameter. It SHALL use `IFavoritesService` for add/remove operations and `IAuthService` for auth checks.

#### Scenario: Favorites loaded on page appear
- **WHEN** the Home page appears and user is authenticated
- **THEN** `IFavoritesService.LoadFavoriteIdsAsync` SHALL be called to populate the favorites cache
