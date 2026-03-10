## 1. Model Mapping

- [x] 1.1 Create `SearchResultItemExtensions` static class with `ToListingSummary()` extension method in `SportowyHub.App/Models/Api/` — map Id, Slug, Title, Price (float? → decimal?), Currency, City, CategoryId (string → int), PublishedAt, ViewCount
- [x] 1.2 Create a helper method `ExtractCondition(SearchResultItem)` that returns (bool hasCondition, string? conditionText, Color badgeColor) by reading the `"condition"` attribute from `Attributes`

## 2. Search Page XAML

- [x] 2.1 Add `controls` xmlns for `ListingCardView` to `SearchPage.xaml`
- [x] 2.2 Replace the search results `CollectionView` (lines 217-272) with a `ScrollView` containing a `FlexLayout` with `Wrap="Wrap"`, `Direction="Row"`, `JustifyContent="SpaceBetween"`, and `BindableLayout.ItemsSource` bound to `SearchResults`
- [x] 2.3 Set up the `BindableLayout.ItemTemplate` using `ListingCardView` with bindings for `Listing`, `TapCommand`, `HasCondition`, `ConditionText`, `ConditionBadgeColor`, and `Margin="0,0,0,12"`
- [x] 2.4 Preserve the empty view ("no results found" label) as a separate element controlled by `ShowNoResults` visibility
- [x] 2.5 Preserve the `MultiTrigger` visibility logic (hide results when no search text, no filters, and no results)

## 3. ViewModel Changes

- [x] 3.1 Add a `MappedSearchResults` observable collection of `ListingSummary` to `SearchViewModel`, or convert inline in XAML via a value converter — decide based on binding complexity
- [x] 3.2 Update `GoToListingDetailCommand` to accept `ListingSummary` parameter (or keep `SearchResultItem` and look up from the parallel collection)
- [x] 3.3 Wire up scroll-based incremental loading: handle `ScrollView.Scrolled` event, check if scroll position is within 200dp of bottom, call `LoadMoreSearchResultsCommand`

## 4. Testing

- [x] 4.1 Write unit tests for `ToListingSummary()` mapping — happy path with all fields, null optional fields, CategoryId string-to-int conversion
- [x] 4.2 Write unit tests for condition extraction — "new", "used", missing attribute, null attributes list
- [x] 4.3 Verify build succeeds with `dotnet build`
- [x] 4.4 Run full test suite with `dotnet test`
