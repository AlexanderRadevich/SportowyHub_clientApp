## Context

The search results page displays listings as text-only cards (title, price, city) in a single-column list. The home page uses the `ListingCardView` control that shows image placeholders, condition badges, favorite toggles, and view counts in a 2-column grid. The goal is to reuse `ListingCardView` on the search page for visual consistency.

The main technical challenge is that search results use `SearchResultItem` while `ListingCardView` expects `ListingSummary` — two different record types with overlapping but not identical fields.

## Goals / Non-Goals

**Goals:**
- Replace the text-only search result cards with `ListingCardView`
- Switch from single-column list to a 2-column wrapped grid layout matching the home page "All Products" section
- Preserve incremental loading (infinite scroll) for search results
- Support condition badge display using data from `SearchResultItem.Attributes`

**Non-Goals:**
- Changing the search bar, filter UI, suggestions, or any other search page element
- Adding favorite toggling to search results (requires separate API work; can be added later)
- Modifying `ListingCardView` internals or visual design
- Changing the `SearchResultItem` API response shape

## Decisions

### 1. Model bridging: Map `SearchResultItem` to `ListingSummary`

**Decision:** Create a mapping extension method `ToListingSummary()` on `SearchResultItem` that produces a `ListingSummary` for `ListingCardView` consumption.

**Alternatives considered:**
- *Make `ListingCardView` generic / accept an interface* — High coupling change, affects home page and my-listings usages, breaks existing bindable property contract.
- *Create a second card control for search* — Defeats the purpose of visual consistency; duplicates XAML.
- *Change the search API to return `ListingSummary`* — Backend change, different endpoint with different data shape, not feasible.

**Rationale:** A lightweight mapping keeps both models independent. `SearchResultItem` has all fields needed for `ListingSummary` (`Id`, `Title`, `Price`, `Currency`, `City`, `CategoryId` → mapped to `CategoryId`, `ViewCount`). The mapping is a one-liner extension method with no allocation overhead beyond the new record instance.

### 2. Layout: Replace `CollectionView` with `FlexLayout` + `BindableLayout`

**Decision:** Use the same `FlexLayout` with `Wrap="Wrap"` and `BindableLayout.ItemsSource` approach as the home page "All Products" section, wrapped in a `ScrollView`.

**Alternatives considered:**
- *Keep `CollectionView` with 2-column grid `ItemsLayout`* — `CollectionView` with `GridItemsLayout` doesn't guarantee the same visual result as `FlexLayout` wrapping, and mixing `CollectionView` features (like `RemainingItemsThreshold`) with `FlexLayout` is not possible.

**Trade-off:** Losing `CollectionView.RemainingItemsThreshold` for automatic incremental loading. Instead, detect scroll position on the wrapping `ScrollView` and trigger load-more manually when near the bottom. This is the same pattern many MAUI apps use with `BindableLayout`.

### 3. Condition badge: Extract from `SearchResultItem.Attributes`

**Decision:** During mapping, extract the `condition` attribute from `SearchResultItem.Attributes` to determine `HasCondition`, `ConditionText`, and `ConditionBadgeColor`. If `Attributes` contains a key `"condition"` with value `"new"` or `"used"`, map accordingly. Otherwise, hide the badge.

### 4. Favorite toggle: Omit for now

**Decision:** Do not wire up favorite toggling on search result cards. The `ToggleFavoriteCommand` will be left unbound (heart icon won't show interactable state). This requires knowing the user's favorites list, which the search flow doesn't currently load.

## Risks / Trade-offs

- **Infinite scroll regression** → Mitigate by implementing scroll-position-based loading on the wrapping `ScrollView` with the same threshold logic (load when within ~200dp of bottom).
- **Performance with large result sets** → `BindableLayout` materializes all items (no virtualization). Mitigate by keeping the existing page size (30) and loading more on scroll. For typical search usage (user scrolls through 30-90 results), this is acceptable. If perf issues arise, revisit with `CollectionView` + `GridItemsLayout`.
- **Model mapping overhead** → Negligible; records are small and mapping happens once per search response.
