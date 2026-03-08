## Context

The home screen currently uses a 3-row Grid: fake search bar, horizontal category chips, and a flat vertical CollectionView of listing cards showing only title, price, and city. The redesign targets a modern marketplace layout inspired by the "FIELD." reference app — with a branded header, condition-based filtering, a "Hot Picks" carousel, and a 2-column product grid with richer card content.

Existing infrastructure:
- `IListingsService.SearchAsync` already supports `condition` and `sort` query parameters
- `IFavoritesService` provides `IsFavorite`, `AddAsync`, `RemoveAsync` — currently unused on home screen
- `BoolToHeartConverter` and heart icons (`icon_heart_filled.svg`, `icon_heart_outline.svg`) already exist
- `FilterChip` style exists for category chips
- Color system supports light/dark via `AppThemeBinding`
- Backend `ListingSummary` has `CategoryId` but no brand, condition, rating, or image fields

## Goals / Non-Goals

**Goals:**
- Redesign home screen layout to match the reference app's visual hierarchy
- Add header with app title + theme toggle, favorites, and cart icon buttons
- Add condition filter chips (ALL, NEW, USED) before sport-section chips
- Create "Hot Picks" horizontal carousel section with rich listing cards
- Create "All Products" 2-column vertical grid with sort control and result count
- Show placeholder images on listing cards (colored background per category)
- Display favorite toggle on each listing card
- Maintain existing infinite scroll pagination for "All Products"
- Support both light and dark themes

**Non-Goals:**
- Implementing shopping cart functionality (button only, no action)
- Backend API changes or new endpoints
- Adding real product images (placeholder strategy only)
- Implementing sort functionality backend calls (UI placeholder only for now)
- Star ratings display (backend doesn't provide rating data)
- Brand name display (backend doesn't provide brand field)
- Bottom tab bar redesign (keep existing Shell tabs)

## Decisions

### 1. Single ScrollView with nested layouts vs. multiple CollectionViews

**Decision:** Use a single `ScrollView` containing the header, search bar, chips, Hot Picks (horizontal `CollectionView`), and All Products (vertical `CollectionView` with fixed height items in a `BindableLayout` or 2-column grid).

**Why:** A single scroll container provides smooth unified scrolling. The "Hot Picks" section is a small fixed set (e.g., first 6 items) so a horizontal `CollectionView` inside `ScrollView` works well. For "All Products," use `CollectionView` with `GridItemsLayout` (2 columns) as the main scrollable area — wrapping everything above it in a header template.

**Revised approach:** Use a single `CollectionView` for All Products as the root scrollable element with `Header` property containing the entire top section (header bar, search, chips, Hot Picks). This avoids nested scrolling issues and keeps infinite scroll working natively.

**Alternative rejected:** Two separate `CollectionView` controls would cause nested scroll conflicts on Android.

### 2. Listing card as DataTemplate vs. reusable ContentView control

**Decision:** Create a reusable `ListingCardView` ContentView in `Controls/` used by both Hot Picks and All Products templates.

**Why:** Both sections display the same card structure (image placeholder, condition badge, favorite heart, title, price). A shared control avoids duplication and ensures visual consistency. The only difference is sizing — Hot Picks cards have fixed width (~180dp), All Products cards fill half the grid.

**Alternative rejected:** Inline DataTemplates would duplicate the card XAML in two places.

### 3. Placeholder image strategy

**Decision:** Use colored `BoxView` backgrounds mapped from `CategoryId` to a predefined color palette, with a centered sport category icon (from existing SVG icons or a generic placeholder). Implement via a `CategoryToColorConverter` and `CategoryToIconConverter`.

**Why:** Simple, lightweight, visually distinctive per category, no external image loading needed. Matches the reference app's colorful card backgrounds.

**Color mapping:** Define 8-10 pastel/muted colors (one per major sport category). Unknown categories fall back to a neutral gray.

### 4. Condition filter chips implementation

**Decision:** Add a `SelectedCondition` property to `HomeViewModel` (values: null/"all", "new", "used"). When selected, reload listings using `SearchAsync` with the `condition` parameter instead of `GetListingsAsync`. The condition chips render before sport-section chips in the same horizontal scroll.

**Why:** `IListingsService.SearchAsync` already accepts a `condition` parameter, so no service changes needed. Using the search endpoint with only a condition filter returns the same data shape.

### 5. Hot Picks data source

**Decision:** Hot Picks displays the first 6 items from the initial listings load (or a separate call with `limit=6`). No separate "featured" API endpoint exists, so reuse the main listings data sorted by newest.

**Why:** Avoids backend changes. The "hot picks" are simply the newest/first listings. If a condition filter is active, Hot Picks also filters accordingly.

### 6. Header action buttons

**Decision:** Add 3 `ImageButton` controls in a horizontal `HorizontalStackLayout` aligned right in the header row:
- Theme toggle: triggers existing theme switching logic (reuse `SettingsViewModel` theme toggle or call `IPreferences` directly)
- Favorites: navigates to the existing Favorites tab via Shell
- Cart: placeholder button with no command (shows a toast or does nothing)

**Why:** Keeps header lightweight. Theme toggle reuses existing infrastructure. Favorites navigation uses Shell routing.

### 7. FAB coexistence with new layout

**Decision:** Keep the FAB overlaid at bottom-right of the page using Grid overlay positioning (same as current). It floats above the All Products grid.

**Why:** No change needed — current implementation already works as an overlay.

## Risks / Trade-offs

**[Nested scrolling on Android]** → Mitigation: Use CollectionView.Header for all content above the grid, avoiding nested ScrollView. Test on Android emulator early.

**[No real images]** → Mitigation: Placeholder design should look intentional, not broken. Use vibrant category colors and sport icons to make cards visually appealing without photos.

**[Missing backend fields (brand, rating, condition)]** → Mitigation: Omit brand and rating from cards (non-goal). For condition badge, either derive from a future API field or hide the badge until backend supports it. Show badge only if a `Condition` property is added to `ListingSummary`.

**[Hot Picks = first 6 listings]** → Mitigation: This is a simplification. If users expect curated picks, we'd need a backend endpoint later. For now, "newest listings" is reasonable.

**[Performance with 2-column grid + images]** → Mitigation: Placeholders are lightweight (BoxView + Label). No bitmap loading means no memory pressure. CollectionView virtualizes items automatically.
