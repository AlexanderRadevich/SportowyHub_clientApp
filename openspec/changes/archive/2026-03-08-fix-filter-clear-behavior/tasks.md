## 1. Fix "Clear Filters" popup behavior

- [x] 1.1 In `SearchFilterPopupViewModel.Reset`, after clearing local state, invoke `Applied` event with a default (empty) `SearchFilterState` so the popup closes and results refresh

## 2. Fix last-chip removal showing all items

- [x] 2.1 In `SearchViewModel.OnSearchTextChanged`, do not clear results when search text becomes empty if a search was previously active (allow `ExecuteSearch` in `RemoveFilter` to handle the refresh)

## 3. Verify

- [x] 3.1 Build and confirm no warnings or errors
