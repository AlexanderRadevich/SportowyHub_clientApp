# Tasks

## Tasks

- [x] Make `_favoriteIds` field `readonly` in `FavoritesService`
- [x] Replace field reassignment in `LoadFavoriteIdsAsync` with `Clear()` + `TryAdd()` loop
- [x] Replace field reassignment in `ClearCache` with `Clear()`
- [x] Verify no other code reassigns the `_favoriteIds` field
- [x] Run `dotnet build` to confirm no compilation errors
