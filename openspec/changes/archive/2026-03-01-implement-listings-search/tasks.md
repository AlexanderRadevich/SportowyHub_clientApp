## 1. API Models

- [x] 1.1 Create `ListingSummary` record in `Models/Api/ListingSummary.cs` (Id, Slug, Title, Price, Currency, City, CategoryId, ContentLocale, PublishedAt)
- [x] 1.2 Create `ListingsResponse` record in `Models/Api/ListingsResponse.cs` (Listings, Total)
- [x] 1.3 Create `ListingDetail` record in `Models/Api/ListingDetail.cs` (all detail fields including Description, Region, Status, CreatedAt, LastModeratorComment)
- [x] 1.4 Create `SearchResultItem`, `GeoLocation`, `SearchAttribute` records in `Models/Api/SearchResultItem.cs`
- [x] 1.5 Create `SearchResponse` record in `Models/Api/SearchResponse.cs` (Items, Total, Limit, Offset)
- [x] 1.6 Register all new records in `SportowyHubJsonContext` with `[JsonSerializable]` attributes

## 2. Listings Service

- [x] 2.1 Create `IListingsService` interface in `Services/Listings/IListingsService.cs` with `GetListingsAsync`, `GetListingAsync`, `SearchAsync` methods (all with CancellationToken)
- [x] 2.2 Create `ListingsService` implementation in `Services/Listings/ListingsService.cs` using primary constructor with `IRequestProvider`; build query strings from parameters, omit nulls
- [x] 2.3 Register `IListingsService` as singleton `ListingsService` in `MauiProgram.cs`

## 3. Listing Detail Page

- [x] 3.1 Create `ListingDetailViewModel` in `ViewModels/ListingDetailViewModel.cs` — primary constructor (IListingsService, INavigationService, IToastService), IQueryAttributable, LoadListingCommand with CancellationToken, IsLoading/HasError properties
- [x] 3.2 Create `ListingDetailPage.xaml` and `.xaml.cs` in `Views/Listings/` — display title, description, price+currency, city, region, published date; ActivityIndicator for loading; error state
- [x] 3.3 Register `ListingDetailPage` and `ListingDetailViewModel` as transient in `MauiProgram.cs`
- [x] 3.4 Register Shell route `listing-detail` in `AppShell.xaml.cs`

## 4. Home Page Listings Feed

- [x] 4.1 Create `HomeViewModel` in `ViewModels/HomeViewModel.cs` — primary constructor (IListingsService, INavigationService, IToastService), ObservableCollection&lt;ListingSummary&gt;, LoadListingsCommand, LoadMoreListingsCommand, RefreshCommand, GoToSearchCommand, IsLoading/IsRefreshing/_hasMoreItems tracking
- [x] 4.2 Register `HomeViewModel` as transient in `MauiProgram.cs` and wire it to `HomePage`
- [x] 4.3 Update `HomePage.xaml` — replace placeholder content with RefreshView + CollectionView (RemainingItemsThreshold=5), listing card DataTemplate (title, price, city), empty state, ActivityIndicator; bind search bar tap to GoToSearchCommand
- [x] 4.4 Update `HomePage.xaml.cs` — remove code-behind `OnSearchBarTapped`, inject ViewModel via constructor and set BindingContext

## 5. Search Integration

- [x] 5.1 Update `SearchViewModel` — add IListingsService and INavigationService injection, ObservableCollection&lt;SearchResultItem&gt; for results, IsSearching/TotalResults properties, debounced search with CancellationTokenSource, LoadMoreSearchResultsCommand, GoToListingDetailCommand
- [x] 5.2 Update `SearchPage.xaml` — add search results CollectionView (hidden when text empty, visible when text present) with listing card DataTemplate, ActivityIndicator, "no results" empty state; keep suggestions visible only when text is empty
- [x] 5.3 Wire incremental loading on search results CollectionView (RemainingItemsThreshold=5, offset pagination, stop when offset+count >= total)

## 6. Verification

- [x] 6.1 Run `dotnet build` — zero errors, zero warnings
- [x] 6.2 Run `get_diagnostics` via MCP — no new diagnostics in modified files
- [x] 6.3 Run `detect_antipatterns` via MCP — no new violations (no `new HttpClient`, all async methods propagate CancellationToken, no empty catch blocks, no Shell.Current in ViewModels)
