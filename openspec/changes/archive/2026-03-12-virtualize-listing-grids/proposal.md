# Virtualize Listing Grids

## Why

`HomePage.xaml` and `SearchPage.xaml` use `FlexLayout` + `BindableLayout.ItemsSource` with no virtualization. Every item is instantiated in memory. Infinite scroll loads 100+ items causing unbounded memory growth and frame drops on Android.

## What Changes

Replace `FlexLayout`+`BindableLayout` with `CollectionView` using `GridItemsLayout` (2 columns) for cell recycling. Remove manual card-width calculation from code-behind since CollectionView handles layout automatically.

## Capabilities

### New
- Cell recycling via `CollectionView` on both listing grids

### Modified
- `HomePage.xaml` — replace FlexLayout with CollectionView+GridItemsLayout
- `HomePage.xaml.cs` — remove manual card-width calculation
- `SearchPage.xaml` — replace FlexLayout with CollectionView+GridItemsLayout
- `SearchPage.xaml.cs` — remove manual card-width calculation and duplicated scroll logic

## Impact

- **Performance** — bounded memory usage regardless of item count; smooth scrolling on Android
- **UX** — eliminates frame drops and potential OOM crashes with large result sets
- **Code** — less code-behind; layout logic delegated to CollectionView
