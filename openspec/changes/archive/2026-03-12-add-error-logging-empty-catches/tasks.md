# Tasks

## Tasks

- [x] Inject `ILogger<ApiErrorParser>` and add logging to catch block at line 17
- [x] Inject `ILogger<AuthService>` and add logging to catch block at line 176
- [x] Inject `ILogger<FavoritesService>` and add logging to Conflict catch block at line 46
- [x] Add logging to `HomeViewModel` catch blocks at line 69
- [x] Add logging to `LoginViewModel` catch block at line 133
- [x] Add logging to `RegisterViewModel` catch block at line 238
- [x] Add logging to `SearchFilterPopupViewModel` catch blocks at lines 125 and 211
- [x] Add logging to `SearchViewModel` catch blocks at lines 168 and 260
- [x] Use `LogDebug` level for intentional `TaskCanceledException` catches in debounce scenarios
- [x] Verify all ILogger registrations resolve correctly via `dotnet build`
