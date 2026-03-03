## Why

The backend returns the `price` field in two different JSON types depending on the endpoint: the list endpoint (`GET /api/v1/listings`) sends it as a **float** (from Elasticsearch), while the detail endpoint (`GET /api/v1/listings/{id}`) sends it as a **string** (from Doctrine's decimal column). This causes `System.Text.Json` deserialization failures when opening a listing detail, because the app's model expects a single consistent type.

## What Changes

- Add a `FlexibleDecimalConverter` (`JsonConverter<decimal?>`) that accepts both JSON number tokens and JSON string tokens, parsing either into `decimal?`
- Change `Price` from `string?` to `decimal?` on `ListingSummary`, `ListingDetail`, and `FavoriteItem` records
- Apply `[JsonConverter(typeof(FlexibleDecimalConverter))]` attribute on each `Price` property
- Fix all C# callers that previously treated `Price` as `string?` (navigation query string builders in `HomeViewModel` and `FavoritesViewModel`)

## Capabilities

### New Capabilities

- `flexible-decimal-converter`: A reusable JSON converter that deserializes both numeric and string JSON values into `decimal?`, handling the backend's inconsistent price serialization

### Modified Capabilities

- `listings-api-models`: `Price` field type changes from `string?` to `decimal?` on `ListingSummary`, `ListingDetail`, and `FavoriteItem`

## Impact

- **Models**: `ListingSummary.cs`, `ListingDetail.cs`, `FavoriteItem.cs` — property type change
- **ViewModels**: `HomeViewModel.cs`, `FavoritesViewModel.cs` — navigation query string formatting
- **JSON infrastructure**: New converter in `Services/Api/FlexibleDecimalConverter.cs`
- **XAML bindings**: No changes needed — `MultiBinding StringFormat` and `StringFormat='{0:F2}'` work correctly with `decimal?`
