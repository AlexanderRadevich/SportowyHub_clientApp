# Abstract Culture Access -- Tasks

## Tasks

- [x] Verify `ILocaleService` exposes a two-letter language code accessor (add one if missing)
- [x] Inject `ILocaleService` into `SearchFilterPopupViewModel` and replace all 3 direct culture accesses
- [x] Inject `ILocaleService` into `HomeViewModel` and replace direct culture access
- [x] Inject `ILocaleService` into `SearchViewModel` and replace direct culture access
- [x] Update unit tests to mock `ILocaleService` instead of manipulating thread culture
- [x] Verify locale is correct at runtime on Android and iOS
