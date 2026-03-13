## 1. Filter State Model

- [x] 1.1 Create `SearchFilterState` partial class extending `ObservableObject` in `SportowyHub.App/Models/` with observable properties: `SelectedSection` (Section?), `SelectedCategoryId` (int?), `SelectedVoivodeshipId` (int?), `SelectedCityId` (int?), `SelectedLocationLabel` (string?), `PriceMin` (float?), `PriceMax` (float?), `Condition` (string?), `Sort` (string?), computed `ActiveFilterCount`, and `Reset()` method

## 2. Localization

- [x] 2.1 Add localization strings to all 4 .resx files (pl, en, uk, ru): filter button labels (FilterTitle, FilterSport, FilterCategory, FilterCategoryPlaceholder, FilterLocation, FilterPriceFrom, FilterPriceTo, FilterCondition, FilterConditionNew, FilterConditionUsed, FilterSort, FilterSortNewest, FilterSortPriceAsc, FilterSortPriceDesc, FilterApply, FilterReset, BrowseByCategory)

## 3. Search Filter Popup

- [x] 3.1 Create `SearchFilterPopup.xaml` + `.xaml.cs` as a CommunityToolkit.Maui `Popup` in `SportowyHub.App/Views/Search/` with scrollable layout containing: sport Picker, category Picker (disabled until sport selected), location Entry with autocomplete results list, price range Entry fields, condition chips (New/Used), sort Picker, and Apply/Reset buttons
- [x] 3.2 Add filter icon SVG asset (`icon_filter.svg`) to `Resources/Images/`
- [x] 3.3 Wire popup data binding: sport picker loads sections from `ISectionsService`, category picker cascades from selected sport, location autocomplete calls `IGeographyService` with 300ms debounce, separator items rendered as dividers

## 4. SearchViewModel Filter Integration

- [x] 4.1 Add `ISectionsService`, `IGeographyService`, and `ILocaleService` to `SearchViewModel` constructor injection
- [x] 4.2 Add `SearchFilterState` property and `ActiveFilterCount` observable property to `SearchViewModel`
- [x] 4.3 Add `OpenFilterCommand` that shows `SearchFilterPopup` and applies returned filter state
- [x] 4.4 Update `ExecuteSearch` to pass all `SearchFilterState` properties to `IListingsService.SearchAsync` (sport, categoryId, cityId, voivodeshipId, priceMin, priceMax, sort, condition)
- [x] 4.5 Implement `IQueryAttributable` on `SearchViewModel` to receive `sport` query parameter, pre-populate `SearchFilterState`, and auto-execute search
- [x] 4.6 Update `OnSearchTextChanged` to trigger search with active filters even when text changes (filters persist across text changes)

## 5. Search Page UI Updates

- [x] 5.1 Add filter `ImageButton` with `icon_filter.svg` to search bar row in `SearchPage.xaml` (right side, after clear button)
- [x] 5.2 Add active filter count badge overlay on the filter button, bound to `ActiveFilterCount`, visible only when count > 0
- [x] 5.3 Support filter-only browsing: show search results CollectionView when filters are active even if search text is empty
- [x] 5.4 Register `SearchFilterPopup` for DI in `MauiProgram.cs`

## 6. Home Page Category Browse

- [x] 6.1 Add `ISectionsService` and `ILocaleService` to `HomeViewModel` constructor injection
- [x] 6.2 Add `Sections` ObservableCollection and `HasSections` property to `HomeViewModel`, load sections on page appearance (swallow errors silently)
- [x] 6.3 Add `GoToFilteredSearchCommand` that switches to Search tab passing sport slug as query parameter
- [x] 6.4 Add horizontal `CollectionView` with sport-section cards to `HomePage.xaml` between search bar and listings feed, bound to `Sections`, visible only when `HasSections` is true

## 7. Build Verification

- [x] 7.1 Run `dotnet build` and fix any compilation errors
- [x] 7.2 Verify filter popup opens/closes correctly on at least one platform
