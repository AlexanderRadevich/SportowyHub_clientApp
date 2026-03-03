## 1. FlexibleDecimalConverter

- [x] 1.1 Create `FlexibleDecimalConverter` class in `Services/Api/FlexibleDecimalConverter.cs` implementing `JsonConverter<decimal?>` that handles `Number`, `String`, and `Null` JSON tokens
- [x] 1.2 Number token: call `reader.GetDecimal()`
- [x] 1.3 String token: parse with `decimal.TryParse` using `InvariantCulture`, return `null` on failure
- [x] 1.4 Write method: write `NumberValue` for non-null, `NullValue` for null

## 2. Model Price Type Changes

- [x] 2.1 Change `ListingSummary.Price` from `string?` to `decimal?` with `[property: JsonConverter(typeof(FlexibleDecimalConverter))]`
- [x] 2.2 Change `ListingDetail.Price` from `string?` to `decimal?` with `[property: JsonConverter(typeof(FlexibleDecimalConverter))]`
- [x] 2.3 Change `FavoriteItem.Price` from `string?` to `decimal?` with `[property: JsonConverter(typeof(FlexibleDecimalConverter))]`

## 3. ViewModel Caller Fixes

- [x] 3.1 Update `HomeViewModel.GoToListingDetail` to format `Price` as `Price?.ToString(CultureInfo.InvariantCulture) ?? string.Empty`
- [x] 3.2 Update `FavoritesViewModel.GoToListingDetail` to format `Price` as `Price?.ToString(CultureInfo.InvariantCulture) ?? string.Empty`

## 4. Verification

- [x] 4.1 Run `dotnet build` with 0 errors, 0 warnings
- [x] 4.2 Verify XAML bindings (`MultiBinding StringFormat`, `StringFormat='{0:F2}'`) work with `decimal?` (no changes needed)
