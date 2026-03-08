## 1. Data Model

- [x] 1.1 Create `ActiveFilterChip` record `(string Key, string Label)` in `SportowyHub.App/Models/`
- [x] 1.2 Add `SelectedCategoryName` (string?) observable property to `SearchFilterState`, include in `CopyFrom()` and `Reset()` (needed for chip label since only category ID is stored)
- [x] 1.3 Add `SelectedSortLabel` (string?) observable property to `SearchFilterState`, include in `CopyFrom()` and `Reset()` (needed for chip label since sort is stored as raw value like "newest")

## 2. ViewModel Logic

- [x] 2.1 Add `ObservableCollection<ActiveFilterChip> ActiveFilterChips` property to `SearchViewModel`
- [x] 2.2 Create `RebuildFilterChips()` private method in `SearchViewModel` that reads `FilterState` and populates `ActiveFilterChips` — one chip per non-null filter with appropriate label (section name, category name, location label, "≥ {priceMin}", "≤ {priceMax}", localized condition, sort label)
- [x] 2.3 Call `RebuildFilterChips()` from `UpdateFilterCount()` so chips rebuild on every filter change
- [x] 2.4 Add `RemoveFilterCommand(string key)` RelayCommand to `SearchViewModel` that switches on key ("sport", "category", "location", "priceMin", "priceMax", "condition", "sort") to null out the corresponding `FilterState` property, calls `UpdateFilterCount()`, and re-executes search
- [x] 2.5 Set `SelectedCategoryName` in `SearchFilterPopupViewModel.Apply()` from `SelectedCategory?.Name` and propagate through `SearchViewModel.OpenFilter()` via `CopyFrom()`
- [x] 2.6 Set `SelectedSortLabel` in `SearchFilterPopupViewModel.Apply()` from `SelectedSortOption?.Label` and propagate through `SearchViewModel.OpenFilter()` via `CopyFrom()`

## 3. Search Page UI

- [x] 3.1 Add chip row to `SearchPage.xaml`: horizontal `ScrollView` (Orientation="Horizontal") between the search bar Border (Row 0) and the activity indicator (Row 1) — update Grid RowDefinitions to add a new Auto row
- [x] 3.2 Inside the ScrollView, add a `BindableLayout` (HorizontalStackLayout with BindableLayout.ItemsSource bound to `ActiveFilterChips`) with a DataTemplate for each chip: `Border` with rounded corners containing label text + close `ImageButton`
- [x] 3.3 Bind chip close button Command to `RemoveFilterCommand` with `CommandParameter="{Binding Key}"`
- [x] 3.4 Make chip row visible only when `HasActiveFilters` is true
- [x] 3.5 Style chips with theme-appropriate colors using `AppThemeBinding` — background matches `SearchBarBackground`/`SearchBarBackgroundDark`, text matches `TextPrimary`/`TextPrimaryDark`, close icon tinted with `Secondary`/`White`

## 4. Build Verification

- [x] 4.1 Run `dotnet build` and fix any compilation errors
