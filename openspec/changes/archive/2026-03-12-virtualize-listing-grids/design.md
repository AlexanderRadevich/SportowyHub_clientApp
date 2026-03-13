# Design: Virtualize Listing Grids

## Context

HomePage and SearchPage render listing cards using `FlexLayout` + `BindableLayout.ItemsSource`. This creates all items upfront with no recycling. With infinite scroll loading 100+ items, memory grows unbounded and Android devices experience frame drops.

## Goals / Non-Goals

### Goals
- Introduce cell recycling via CollectionView on both pages
- Maintain the 2-column grid layout with current card appearance
- Remove manual card-width calculation from code-behind

### Non-Goals
- Change the card design or ListingCardView control
- Implement pull-to-refresh (separate concern)
- Change the infinite scroll / pagination logic

## Decisions

1. **CollectionView + GridItemsLayout** — Use `GridItemsLayout` with `Span="2"` for the 2-column grid. CollectionView handles cell recycling and layout measurement automatically.
2. **Keep DataTemplate** — Reuse existing `ListingCardView` inside the CollectionView's `ItemTemplate`.
3. **Remove code-behind sizing** — CollectionView with `GridItemsLayout` distributes space evenly, eliminating the need for manual `WidthRequest` calculations on `SizeChanged`.
4. **RemainingItemsThreshold** — Use CollectionView's built-in `RemainingItemsThreshold` for infinite scroll instead of manual scroll detection.

## Risks / Trade-offs

- **CollectionView MAUI bugs** — CollectionView on Android/iOS has known issues with certain layouts. Testing on physical devices is critical.
- **Visual differences** — `GridItemsLayout` spacing may differ slightly from `FlexLayout` spacing. May need to adjust `HorizontalItemSpacing` and `VerticalItemSpacing`.
- **Scroll position preservation** — Switching from FlexLayout to CollectionView may affect scroll position behavior during incremental loads.
