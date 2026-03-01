## ADDED Requirements

### Requirement: Favorite toggle on listing detail

The `ListingDetailPage` SHALL display a heart button that allows the user to add or remove the listing from favorites. The button state SHALL reflect whether the listing is currently favorited.

#### Scenario: Show favorited state
- **WHEN** the listing detail loads and `IFavoritesService.IsFavorite(id)` returns `true`
- **THEN** the heart button SHALL appear filled/active

#### Scenario: Show unfavorited state
- **WHEN** the listing detail loads and `IFavoritesService.IsFavorite(id)` returns `false`
- **THEN** the heart button SHALL appear as an outline/inactive

#### Scenario: Toggle favorite on (add)
- **WHEN** the user taps the heart button and the listing is not favorited
- **THEN** the heart SHALL immediately show as filled (optimistic)
- **THEN** `IFavoritesService.AddAsync(id)` SHALL be called
- **THEN** on failure, the heart SHALL revert to outline and a toast error SHALL be shown

#### Scenario: Toggle favorite off (remove)
- **WHEN** the user taps the heart button and the listing is favorited
- **THEN** the heart SHALL immediately show as outline (optimistic)
- **THEN** `IFavoritesService.RemoveAsync(id)` SHALL be called
- **THEN** on failure, the heart SHALL revert to filled and a toast error SHALL be shown

#### Scenario: Not logged in
- **WHEN** the user taps the heart button and is not logged in
- **THEN** the app SHALL navigate to the login page
- **THEN** no API call SHALL be made

#### Scenario: Disable during API call
- **WHEN** an add/remove API call is in progress
- **THEN** the heart button SHALL be disabled to prevent double-tap
