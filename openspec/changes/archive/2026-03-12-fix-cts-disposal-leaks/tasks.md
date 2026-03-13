# Tasks

## Tasks

- [x] Implement `IDisposable` on `SearchFilterPopupViewModel` with standard dispose pattern
- [x] Dispose `_locationDebounceCts` in `SearchFilterPopupViewModel.Dispose`
- [x] Dispose old CTS before creating new one in debounce methods of `SearchFilterPopupViewModel`
- [x] Implement `IDisposable` on `SearchViewModel` with standard dispose pattern
- [x] Dispose `_debounceCts` in `SearchViewModel.Dispose`
- [x] Dispose old CTS before creating new one in debounce methods of `SearchViewModel`
- [x] Add disposal trigger in `SearchFilterPopupPage` on popup close
- [x] Add disposal trigger in `SearchPage.OnDisappearing`
- [x] Verify `dotnet build` succeeds
