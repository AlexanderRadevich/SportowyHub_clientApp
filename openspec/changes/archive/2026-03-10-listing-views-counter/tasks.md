## 1. API Models

- [x] 1.1 Add `ViewCount` (int, default 0) property to `ListingSummary` record
- [x] 1.2 Add `ViewCount` (int, default 0) property to `ListingDetail` record
- [x] 1.3 Add `ViewCount` (int, default 0) property to `SearchResultItem` record
- [x] 1.4 Add `ViewCount` (int, default 0) property to `MyListingSummary` record

## 2. View Count Converter

- [x] 2.1 Create `ViewCountFormatConverter` in `Converters/` (IValueConverter: 0–999 as-is, 1k+, 1M+ with InvariantCulture)
- [x] 2.2 Register `ViewCountFormatConverter` as a static resource in `App.xaml` or `Styles.xaml`

## 3. Listing Card UI

- [x] 3.1 Add view count row to `ListingCardView.xaml` below price (eye icon 12×12 + formatted count, FontSize 11, secondary text color)
- [x] 3.2 Bind visibility to `ViewCount > 0` so it hides when zero
- [x] 3.3 Verify light/dark theme support for icon tint and text color

## 4. Listing Detail Page

- [x] 4.1 Add view count indicator to `ListingDetailPage.xaml` below location, before divider (eye icon + formatted count, FontSize 14)
- [x] 4.2 Bind visibility to `ViewCount > 0` and listing loaded state

## 5. Tests

- [x] 5.1 Unit test `ViewCountFormatConverter` for all formatting tiers (0, 42, 999, 1000, 1234, 1000000, 1500000, null)
- [x] 5.2 Unit test `ListingSummary` deserialization with and without `view_count` field
- [x] 5.3 Unit test `ListingDetail` deserialization with and without `view_count` field

## 6. Verification

- [x] 6.1 Run `dotnet build` and confirm no warnings
- [x] 6.2 Run `dotnet test` and confirm all tests pass
