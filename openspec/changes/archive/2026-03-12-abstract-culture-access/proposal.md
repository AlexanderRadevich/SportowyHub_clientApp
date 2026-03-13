# Abstract Culture Access

## Why

Five locations access `Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName` directly instead of using the existing `ILocaleService` abstraction. This bypasses the service layer, makes unit testing harder (requires thread manipulation), and creates inconsistency if `ILocaleService` ever applies custom logic.

## What Changes

Inject `ILocaleService` into the three affected ViewModels and replace all direct `Thread.CurrentThread.CurrentUICulture` access with the service's locale accessor.

## Capabilities

### New

- None -- uses existing `ILocaleService`

### Modified

- `SearchFilterPopupViewModel.cs` -- inject `ILocaleService`, replace 3 direct culture accesses
- `HomeViewModel.cs` -- inject `ILocaleService`, replace 1 direct culture access
- `SearchViewModel.cs` -- inject `ILocaleService`, replace 1 direct culture access

## Impact

- **Scope:** Three ViewModel files
- **Risk:** Very low -- replacing direct access with an existing abstraction
- **Testing:** ViewModels become easier to test (mock `ILocaleService` instead of manipulating thread culture)
- **UX:** No user-visible changes
