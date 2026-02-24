## 1. Dependencies

- [x] 1.1 Add `CommunityToolkit.Maui` NuGet package to the project
- [x] 1.2 Register `UseMauiCommunityToolkit()` in `MauiProgram.cs` builder chain

## 2. Fix Icons

- [x] 2.1 Add `IconTintColorBehavior` with `AppThemeBinding` (Secondary light / White dark) to the `icon_search_bar.svg` Image in `Views/Home/HomePage.xaml`
- [x] 2.2 Add `IconTintColorBehavior` with `AppThemeBinding` to the `icon_back.svg` ImageButton in `Views/Search/SearchPage.xaml`
- [x] 2.3 Add `IconTintColorBehavior` with `AppThemeBinding` to the `icon_clear.svg` ImageButton in `Views/Search/SearchPage.xaml`

## 3. Verification

- [x] 3.1 Build the project and verify no compilation errors
- [x] 3.2 Verify search bar icons are visible in dark theme on both Home and Search pages
- [x] 3.3 Verify search bar icons use Secondary color in light theme
