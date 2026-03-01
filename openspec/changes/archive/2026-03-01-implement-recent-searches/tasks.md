## 1. Service Layer

- [x] 1.1 Create `IRecentSearchesService` interface in `Services/RecentSearches/` with `GetAll()`, `Add(string)`, and `Clear()` methods
- [x] 1.2 Implement `RecentSearchesService` using `Preferences` + `System.Text.Json` serialization with `SportowyHubJsonContext`
- [x] 1.3 Register `IRecentSearchesService` / `RecentSearchesService` as singleton in `MauiProgram.cs`

## 2. ViewModel Integration

- [x] 2.1 Inject `IRecentSearchesService` into `SearchViewModel` via primary constructor
- [x] 2.2 Replace hardcoded `RecentSearches` initializer with empty collection, add `LoadRecentSearches()` method that refreshes from service
- [x] 2.3 Call `LoadRecentSearches()` on page appearance (add `AppearingCommand` or equivalent)
- [x] 2.4 Call `IRecentSearchesService.Add(query)` in `ExecuteSearch` on successful initial search (offset == 0), then refresh collection
- [x] 2.5 Add `ClearRecentSearchesCommand` that calls `IRecentSearchesService.Clear()` and clears the observable collection

## 3. UI Updates

- [x] 3.1 Add "Clear" action label next to the "Recent Searches" header in `SearchPage.xaml`, bound to `ClearRecentSearchesCommand`
- [x] 3.2 Bind the recent searches section visibility to `RecentSearches.Count > 0`
- [x] 3.3 Add localized string `SearchClearRecent` ("Clear" / "Wyczyść" / etc.) to all 4 `.resx` files

## 4. Verification

- [x] 4.1 Build succeeds (`dotnet build`)
- [x] 4.2 No Roslyn diagnostics errors (`get_diagnostics`)
- [x] 4.3 Unit tests pass (`dotnet test`)
