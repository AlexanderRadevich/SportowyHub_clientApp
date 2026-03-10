## 1. Replace filter chip DataTemplate

- [x] 1.1 Replace the existing `HorizontalStackLayout`-based filter chip template in `SearchPage.xaml` with a `Grid`-based split-pill layout: left column (dot + label), vertical divider, right column (× icon)
- [x] 1.2 Add a 6×6 colored dot indicator (`Border` with `RoundRectangle`) before the label text in the label zone
- [x] 1.3 Add a 1px `BoxView` vertical divider between the label and remove zones
- [x] 1.4 Set the remove zone to at least 44px wide with the × icon centered (12×12) and `TapGestureRecognizer` bound to `RemoveFilterCommand`
- [x] 1.5 Ensure the label zone has no gesture recognizer (tap does nothing)

## 2. Theme and styling

- [x] 2.1 Apply `AppThemeBinding` for chip border stroke, background, label text color, dot color, divider color, and close icon tint — using existing theme resources
- [ ] 2.2 Verify chip renders correctly in both light and dark themes

## 3. Verify

- [x] 3.1 Build the project (`dotnet build`) with no errors
- [ ] 3.2 Visually verify the split-pill chips match the reference design on Android/Windows
- [ ] 3.3 Confirm removing a filter via the × zone works and tapping the label zone does not trigger removal
