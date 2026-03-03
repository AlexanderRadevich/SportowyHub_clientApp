## 1. Model Updates

- [x] 1.1 Create `MyListingSummary` record with `Id`, `Slug`, `Title`, `Status`, `Price` (decimal? with FlexibleDecimalConverter), `Currency`, `ContentLocale`, `CreatedAt`, `PublishedAt` in `Models/Api/MyListingSummary.cs`
- [x] 1.2 Create `MyListingsResponse` record with `Listings` (List&lt;MyListingSummary&gt;) and `Total` (int) in `Models/Api/MyListingsResponse.cs`
- [x] 1.3 Update `MediaUrls` record: replace `Original`/`Thumbnail` with `Thumb160`, `Thumb320`, `Card640`, `Gallery1024`, `Gallery1920`, `Og1200x630` (all string?)
- [x] 1.4 Register `MyListingSummary` and `MyListingsResponse` in `SportowyHubJsonContext`
- [x] 1.5 Update `IListingManagementService.GetMyListingsAsync` return type from `ListingsResponse` to `MyListingsResponse`
- [x] 1.6 Update `ListingManagementService.GetMyListingsAsync` implementation to deserialize as `MyListingsResponse`

## 2. Localization

- [x] 2.1 Add localized strings to all 4 `.resx` files (pl, en, uk, ru): `ProfileMyListings`, `MyListingsTitle`, `MyListingsFilterAll`, `MyListingsFilterDraft`, `MyListingsFilterPublished`, `MyListingsFilterPending`, `MyListingsFilterRejected`, `MyListingsFilterHidden`, `MyListingsEmpty`, `MyListingsEmptyCreate`, `MyListingsPublish`, `MyListingsHide`, `MyListingsResubmit`, `MyListingsDelete`, `MyListingsDeleteConfirmTitle`, `MyListingsDeleteConfirmMessage`, `MyListingsStatusDraft`, `MyListingsStatusPublished`, `MyListingsStatusPending`, `MyListingsStatusRejected`, `MyListingsStatusHidden`
- [x] 2.2 Add localized strings for create/edit page: `CreateListingTitle`, `EditListingTitle`, `ListingFieldCategory`, `ListingFieldSection`, `ListingFieldTitle`, `ListingFieldDescription`, `ListingFieldPrice`, `ListingFieldCurrency`, `ListingFieldCityId`, `ListingFieldVoivodeshipId`, `ListingFieldLocale`, `ListingPhotosTitle`, `ListingAddPhoto`, `ListingSave`, `ListingCreateSuccess`, `ListingEditSuccess`, `ListingTitleRequired`, `ListingCategoryRequired`

## 3. Profile Hub — My Listings Row

- [x] 3.1 Add `GoToMyListingsCommand` to `ProfileViewModel` that navigates to `my-listings`
- [x] 3.2 Add "My Listings" row to `ProfilePage.xaml` in Account section between "Account Profile" and "Sign Out", visible only when `IsLoggedIn`

## 4. My Listings Page

- [x] 4.1 Create `MyListingsViewModel` with: `ObservableCollection<MyListingSummary>` Listings, `IsLoading`, `IsRefreshing`, `IsEmpty`, `SelectedFilter` (string?), filter command, refresh command, load command, delete command, publish command, hide command, resubmit command, go-to-edit command, go-to-create command
- [x] 4.2 Create `MyListingsPage.xaml` with: status filter tabs (horizontal scroll), `RefreshView` wrapping `CollectionView`, each item as a card with title/price/status badge/created date/action buttons, empty state view, FAB for create
- [x] 4.3 Create `MyListingsPage.xaml.cs` code-behind with constructor DI and `OnAppearing` override

## 5. Create/Edit Listing Page

- [x] 5.1 Create `CreateEditListingViewModel` implementing `IQueryAttributable`: section/category pickers, form fields (Title, Description, Price, Currency, CityId, VoivodeshipId, ContentLocale), `IsEditMode`, `IsSaving`, `IsLoading`, photo collection, `SaveCommand`, `AddPhotoCommand`, `DeletePhotoCommand`, `LoadSectionsCommand`, `LoadCategoriesCommand`
- [x] 5.2 Create `CreateEditListingPage.xaml` with: scrollable form, two-step category picker (Section picker → Category picker), Entry/Editor fields for all listing properties, Currency/Locale pickers, photos horizontal scroll with add/delete, Save button
- [x] 5.3 Create `CreateEditListingPage.xaml.cs` code-behind with constructor DI and `OnAppearing` override

## 6. Registration & Navigation

- [x] 6.1 Register `MyListingsPage`, `MyListingsViewModel`, `CreateEditListingPage`, `CreateEditListingViewModel` as transient in `MauiProgram.cs`
- [x] 6.2 Register Shell routes `my-listings` and `create-edit-listing` in `AppShell.xaml.cs`

## 7. Verification

- [x] 7.1 Run `dotnet build` with 0 errors, 0 warnings
- [x] 7.2 Verify all new models are registered in `SportowyHubJsonContext`
