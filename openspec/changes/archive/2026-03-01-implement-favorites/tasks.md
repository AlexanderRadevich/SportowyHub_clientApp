## 1. API Models

- [x] 1.1 Create `FavoriteItem` record in `Models/Api/` matching backend shape (Id, Title, Price, Currency, City, Status, Slug, SerialId)
- [x] 1.2 Create `FavoritesIdsResponse` record (Ids, Count)
- [x] 1.3 Create `FavoritesListResponse` record (Items, Total, Page, PerPage, Pages)
- [x] 1.4 Create `FavoriteActionResponse` record (Status, FavoritesCount)
- [x] 1.5 Register all new types in `SportowyHubJsonContext`

## 2. Favorites Service

- [x] 2.1 Create `IFavoritesService` interface in `Services/Favorites/` with `LoadFavoriteIdsAsync`, `IsFavorite`, `GetFavoritesAsync`, `AddAsync`, `RemoveAsync`, `ClearCache`
- [x] 2.2 Implement `FavoritesService` with in-memory `HashSet<string>` IDs cache, using `IRequestProvider` and `IAuthService` for authenticated calls
- [x] 2.3 Register `IFavoritesService` / `FavoritesService` as singleton in `MauiProgram.cs`
- [x] 2.4 Call `IFavoritesService.ClearCache()` in logout flows (ProfileViewModel + AccountProfileViewModel)

## 3. Favorites Page & ViewModel

- [x] 3.1 Create `FavoritesViewModel` with page-based pagination (LoadFavorites, RefreshFavorites, LoadMoreFavorites commands), auth check, and empty/sign-in states
- [x] 3.2 Add `RemoveFavoriteCommand` and `GoToListingDetailCommand` to `FavoritesViewModel`
- [x] 3.3 Replace `FavoritesPage.xaml` placeholder with full layout: auth-gated content, RefreshView, CollectionView with listing cards, empty state, loading indicator
- [x] 3.4 Update `FavoritesPage.xaml.cs` to inject `FavoritesViewModel` and call `AppearingCommand` in `OnAppearing`
- [x] 3.5 Register `FavoritesViewModel` (transient) in `MauiProgram.cs`

## 4. Listing Detail Favorite Toggle

- [x] 4.1 Inject `IFavoritesService` and `IAuthService` into `ListingDetailViewModel`, add `IsFavorited`, `IsTogglingFavorite` properties and `ToggleFavoriteCommand`
- [x] 4.2 Load favorite state after listing loads (`IsFavorited = favoritesService.IsFavorite(id)`)
- [x] 4.3 Add heart button to `ListingDetailPage.xaml` (icon_heart_outline / icon_heart_filled based on `IsFavorited`)

## 5. Localized Strings

- [x] 5.1 Add localized strings to all 4 .resx files: `FavoritesEmpty`, `FavoritesSignInPrompt`, `FavoritesSignIn`
- [x] 5.2 Add entries to `AppResources.Designer.cs`

## 6. SVG Icons

- [x] 6.1 Add `icon_heart_outline.svg` and `icon_heart_filled.svg` to `Resources/Images/`

## 7. Verification

- [x] 7.1 Build succeeds (`dotnet build`)
- [x] 7.2 No Roslyn diagnostics errors (`get_diagnostics`)
