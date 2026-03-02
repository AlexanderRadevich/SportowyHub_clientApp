## 1. SkeletonBox Control

- [x] 1.1 Create `SkeletonBox` control in `Controls/` — a `BoxView` subclass with rounded corners and theme-aware muted background color via `AppThemeBinding`
- [x] 1.2 Add skeleton placeholder color resources to `Colors.xaml` for both light and dark themes

## 2. ListingDetailViewModel — Preview Properties

- [x] 2.1 Add `PreviewTitle`, `PreviewPrice`, `PreviewCurrency`, `PreviewCity` observable properties to `ListingDetailViewModel`
- [x] 2.2 Update `ApplyQueryAttributes` to extract `title`, `price`, `currency`, `city` from query params and populate preview properties
- [x] 2.3 Update `FormattedPrice` to fall back to `PreviewPrice`/`PreviewCurrency` when `Listing` is null
- [x] 2.4 Update `FormattedLocation` to fall back to `PreviewCity` when `Listing` is null
- [x] 2.5 Add a `DisplayTitle` computed property that returns `Listing?.Title ?? PreviewTitle` for the page title and header binding

## 3. ListingDetailPage — Progressive Layout

- [x] 3.1 Remove the full-screen `ActivityIndicator` overlay
- [x] 3.2 Make the `ScrollView` always visible (remove `IsVisible` binding to `IsLoading`)
- [x] 3.3 Bind header elements (title, price, city, favorite button) to the new fallback properties so they display immediately
- [x] 3.4 Add `SkeletonBox` placeholders for description (multi-line height) and published date, visible when `IsLoading` is true
- [x] 3.5 Toggle visibility between skeleton placeholders and real content labels using `IsLoading` binding

## 4. Source ViewModels — Pass Preview Data

- [x] 4.1 Update `HomeViewModel.GoToListingDetail` to pass `title`, `price`, `currency`, `city` from `ListingSummary` in navigation query params
- [x] 4.2 Update `SearchViewModel.GoToListingDetail` to pass `title`, `price` (float→string conversion), `currency`, `city` from `SearchResultItem`
- [x] 4.3 Update `FavoritesViewModel.GoToListingDetail` to pass `title`, `price`, `currency`, `city` from `FavoriteItem`

## 5. Verification

- [x] 5.1 Build the solution and fix any compiler errors
- [x] 5.2 Run existing tests and fix any failures
- [ ] 5.3 Manual test: navigate to listing detail from Home feed — verify title/price/city appear instantly with skeleton placeholders, then full content loads
- [ ] 5.4 Manual test: navigate from Search and Favorites — same behavior
- [ ] 5.5 Manual test: verify light and dark theme skeleton colors
