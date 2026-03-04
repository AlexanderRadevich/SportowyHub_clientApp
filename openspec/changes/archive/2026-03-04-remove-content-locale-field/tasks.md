## 1. Remove Content Locale UI field

- [x] 1.1 In `CreateEditListingPage.xaml`, remove the Content Locale `VerticalStackLayout` block (Label + Entry with `{Binding ContentLocale}`)
- [x] 1.2 In `CreateEditListingViewModel.cs`, remove the `[ObservableProperty] ContentLocale` property and its `"pl"` default
- [x] 1.3 In `CreateEditListingViewModel.cs`, remove the `ContentLocale = match.ContentLocale ?? "pl"` assignment in `LoadExistingListing`

## 2. Auto-resolve locale from ILocaleService

- [x] 2.1 In `CreateEditListingViewModel.cs`, inject `ILocaleService` via primary constructor parameter
- [x] 2.2 In the `Save` method, call `ILocaleService.GetLocaleInfoAsync(ct)` before building the request and use the returned `Locale` as the `ContentLocale` value for both `CreateListingRequest` and `UpdateListingRequest`

## 3. Verification

- [x] 3.1 Run `dotnet build` and confirm zero errors
- [x] 3.2 Run `dotnet test` and confirm all existing tests pass (mock `ILocaleService` in any affected ViewModel tests)
