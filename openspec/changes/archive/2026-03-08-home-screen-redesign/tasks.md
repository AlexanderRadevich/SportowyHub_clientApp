## 1. Resources & Converters

- [x] 1.1 Add placeholder color palette to `Colors.xaml` — define 8-10 category placeholder colors (pastel/muted, working in both light and dark themes)
- [x] 1.2 Create `CategoryToColorConverter` in `Converters/` — maps `CategoryId` to placeholder background color, with neutral gray fallback
- [x] 1.3 Add shopping cart icon SVG to `Resources/Images/` (e.g., `icon_cart.svg`)
- [x] 1.4 Add theme toggle icon SVG to `Resources/Images/` (e.g., `icon_theme.svg` or sun/moon icons)
- [x] 1.5 Add localization strings for new UI labels: "HOT PICKS", "SEE ALL", "ALL PRODUCTS", "SORT", "Showing {0} results", chip labels "ALL", "NEW", "USED"

## 2. ListingCardView Control

- [x] 2.1 Create `Controls/ListingCardView.xaml` + `ListingCardView.xaml.cs` ContentView with bindable properties: `Listing` (ListingSummary), `IsFavorited` (bool), `TapCommand` (ICommand), `ToggleFavoriteCommand` (ICommand)
- [x] 2.2 Implement card image area — `BoxView` with `CategoryToColorConverter` background color, consistent aspect ratio (~4:3), rounded top corners
- [x] 2.3 Implement condition badge overlay — Label in top-left corner with "NEW" (dark bg) or "USED" (orange bg), visibility bound to condition availability
- [x] 2.4 Implement favorite heart button — `ImageButton` in top-right corner using `BoolToHeartConverter`, bound to `ToggleFavoriteCommand`
- [x] 2.5 Implement card text area — title (single line, tail truncation) and price with currency formatting below image area
- [x] 2.6 Add `AppThemeBinding` for card background, text colors, and border for light/dark theme support

## 3. HomeViewModel Updates

- [x] 3.1 Add `HotPicks` observable collection property (max 6 items, populated from first 6 of main listings load)
- [x] 3.2 Add `SelectedCondition` observable property (null = ALL, "new", "used") with `SelectConditionCommand` that sets the value and reloads listings
- [x] 3.3 Update `LoadListingsAsync` — when `SelectedCondition` is set, use `SearchAsync(condition:)` instead of `GetListingsAsync`; populate `HotPicks` from first 6 results
- [x] 3.4 Add `TotalResults` observable property, set from API response total count
- [x] 3.5 Add `ToggleFavoriteCommand` accepting `ListingSummary` — auth check, optimistic UI, call `IFavoritesService.AddAsync`/`RemoveAsync`, revert on failure with toast
- [x] 3.6 Inject `IFavoritesService` into HomeViewModel; call `LoadFavoriteIdsAsync` on page appear when authenticated
- [x] 3.7 Add `ToggleThemeCommand` — switch between light/dark theme using existing theme service/preferences
- [x] 3.8 Add `GoToFavoritesCommand` — navigate to Saved/Favorites tab via Shell
- [x] 3.9 Add `GoToCartCommand` — placeholder command (no-op or toast "Coming soon")

## 4. HomePage.xaml Rewrite

- [x] 4.1 Replace page root with single `CollectionView` using `GridItemsLayout` (2 columns, vertical) for All Products section; set `RemainingItemsThreshold=5` and bind `RemainingItemsThresholdReachedCommand`
- [x] 4.2 Build `CollectionView.Header` DataTemplate containing: header bar, search bar, chips row, Hot Picks section
- [x] 4.3 Implement header bar — app title label (left) + `HorizontalStackLayout` of 3 `ImageButton` controls (theme, favorites, cart) aligned right
- [x] 4.4 Implement search bar — styled `Border` with search icon, placeholder text, filter icon; `TapGestureRecognizer` bound to `GoToSearchCommand`
- [x] 4.5 Implement category chips row — horizontal `CollectionView` or `BindableLayout` inside `ScrollView`; render ALL/NEW/USED condition chips first, then sport-section chips; bind selected state to `SelectedCondition`
- [x] 4.6 Implement Hot Picks section — "HOT PICKS" header + "SEE ALL →" button + horizontal `CollectionView` bound to `HotPicks` collection using `ListingCardView` with fixed ~180dp width
- [x] 4.7 Implement All Products section header — "ALL PRODUCTS" label + "Showing N results" subtitle + "SORT" button (placeholder)
- [x] 4.8 Set `CollectionView.ItemTemplate` for All Products grid using `ListingCardView` filling grid column width
- [x] 4.9 Keep FAB overlay — position FAB at bottom-right using Grid overlay, same as current implementation
- [x] 4.10 Wrap `CollectionView` in `RefreshView` for pull-to-refresh binding
- [x] 4.11 Add empty state view when no listings and not loading

## 5. DI Registration & Wiring

- [x] 5.1 Register `CategoryToColorConverter` in app resources or as a static converter
- [x] 5.2 Verify `IFavoritesService` is already registered in `MauiProgram.cs` (no new registrations expected)

## 6. Testing & Verification

- [x] 6.1 Build and verify no compilation errors (`dotnet build`)
- [ ] 6.2 Run on Android emulator — verify unified scrolling, Hot Picks carousel, 2-column grid, condition chip filtering
- [ ] 6.3 Test light/dark theme toggle from header button — verify all card elements theme correctly
- [ ] 6.4 Test favorite heart toggle on cards — verify optimistic UI and auth gating
- [ ] 6.5 Test condition chip filtering (ALL → NEW → USED) — verify listings reload and Hot Picks update
- [ ] 6.6 Test infinite scroll in All Products grid — verify pagination still works
- [ ] 6.7 Test FAB visibility and navigation for authenticated vs anonymous users
