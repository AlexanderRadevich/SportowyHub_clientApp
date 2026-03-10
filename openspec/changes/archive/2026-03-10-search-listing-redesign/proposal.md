## Why

The search results page uses a minimal text-only card layout (title, price, city) while the home page uses the rich `ListingCardView` control with image placeholders, condition badges, favorite toggles, and view counts. This inconsistency makes search results feel like a downgrade and reduces the information available to users when browsing results — they can't see the condition, favorite status, or view count without tapping into each listing.

## What Changes

- Replace the inline text-only card template in the search results `CollectionView` with the reusable `ListingCardView` control
- Switch from a single-column full-width list to a 2-column grid layout matching the home page "All Products" section
- Wire up favorite toggling and tap navigation through the existing `ListingCardView` bindable properties
- Ensure the search results pass condition, favorite state, and view count data to the card control

## Capabilities

### New Capabilities

_(none)_

### Modified Capabilities

- `search-ui`: Search results display changes from text-only list cards to rich `ListingCardView` grid cards matching the home page layout

## Impact

- **Views/Search/SearchPage.xaml** — Replace the results `CollectionView` item template and layout
- **ViewModels/Search/SearchViewModel.cs** — May need to expose favorite toggle command and condition data if not already available for search results
- **Controls/ListingCardView** — No changes expected; reuse as-is
- **openspec/specs/search-ui/** — Delta spec for the new results layout
