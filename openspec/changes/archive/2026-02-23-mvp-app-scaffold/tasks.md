## 1. Project Setup & Dependencies

- [x] 1.1 Add `CommunityToolkit.Mvvm` NuGet package to `SportowyHub.csproj`
- [x] 1.2 Create folder structure: `Views/Home/`, `Views/Search/`, `Views/Favorites/`, `Views/Profile/`, `Views/Auth/`, `ViewModels/`
- [x] 1.3 Add SVG tab icons to `Resources/Images/`: `icon_home.svg`, `icon_search.svg`, `icon_favorites.svg`, `icon_profile.svg`
- [x] 1.4 Add SVG UI icons to `Resources/Images/`: `icon_eye.svg`, `icon_eye_off.svg`, `icon_clear.svg`, `icon_back.svg`, `icon_search_bar.svg` (magnifying glass for search bar)

## 2. Theming & Design System

- [x] 2.1 Replace `Resources/Styles/Colors.xaml` with SportowyHub brand palette (Primary, PrimaryDark, Secondary, Background, Surface, TextPrimary, TextSecondary, Border, SearchBarBackground) using `AppThemeBinding` for light/dark values
- [x] 2.2 Update `Resources/Styles/Styles.xaml` — replace template styles with branded styles: implicit `Page`, `Shell`, `Label`, `Entry`, `Button` styles; named `PrimaryButton` and `SecondaryButton` styles with 16px corner radius and 50px height; all using 8pt spacing grid
- [x] 2.3 Update Shell style in `Styles.xaml` to set `TabBarForegroundColor` to Primary/PrimaryDark, `TabBarUnselectedColor` to TextSecondary, and `TabBarBackgroundColor` to Background

## 3. Shell Navigation

- [x] 3.1 Create stub pages: `Views/Home/HomePage.xaml(.cs)`, `Views/Search/SearchPage.xaml(.cs)`, `Views/Favorites/FavoritesPage.xaml(.cs)`, `Views/Profile/ProfilePage.xaml(.cs)` — each as a basic `ContentPage` with a centered label showing the page name
- [x] 3.2 Replace `AppShell.xaml` with a `TabBar` containing 4 `Tab` items (Home, Search, Favorites, Profile) referencing the new pages with SVG icons
- [x] 3.3 Remove the old `MainPage.xaml` and `MainPage.xaml.cs` (template counter page)
- [x] 3.4 Register all pages in `MauiProgram.cs` via `builder.Services.AddTransient<T>()` for DI
- [x] 3.5 Register Shell routes for auth pages: `Routing.RegisterRoute("register", typeof(RegisterPage))` and `Routing.RegisterRoute("login", typeof(LoginPage))` in `AppShell.xaml.cs`

## 4. Home Screen & Search Bar

- [x] 4.1 Build `HomePage.xaml` with a custom search bar at the top: `Border` with `RoundRectangle` (12-16px radius) containing a horizontal layout with magnifying glass icon and "Search for products..." label; themed background (`SearchBarBackground`), themed text/icon color (`Secondary` light / `White` dark)
- [x] 4.2 Add `TapGestureRecognizer` on the search bar that navigates to the Search tab or Search page

## 5. Search Page

- [x] 5.1 Create `ViewModels/SearchViewModel.cs` with: `SearchText` observable property, `RecentSearches` and `PopularSearches` observable collections (hardcoded placeholder data), `ClearSearchCommand`, and logic to fill input on suggestion tap
- [x] 5.2 Build `Views/Search/SearchPage.xaml` with: editable `Entry` at the top (auto-focus on appearing), clear button visible when text is entered, back button in top area
- [x] 5.3 Add "Recent Searches" and "Popular Searches" sections below the search input as vertical lists of tappable items, bound to the view model collections
- [x] 5.4 Register `SearchViewModel` in `MauiProgram.cs` and inject into `SearchPage` constructor

## 6. Profile Screen (Logged-Out State)

- [x] 6.1 Build `Views/Profile/ProfilePage.xaml` with centered vertical layout: app logo image, "Welcome" title label, "Create Account" primary button, "Sign In" secondary button
- [x] 6.2 Wire "Create Account" button to navigate to `register` route via `Shell.Current.GoToAsync`
- [x] 6.3 Wire "Sign In" button to navigate to `login` route via `Shell.Current.GoToAsync`

## 7. Registration Screen

- [x] 7.1 Create `ViewModels/RegisterViewModel.cs` with: `Email`, `Password`, `ConfirmPassword` observable properties; `EmailError`, `PasswordStrength`, `ConfirmPasswordError` observable properties; real-time validation logic (email regex, password strength weak/medium/strong, confirm match); `CreateAccountCommand` with `CanExecute` bound to form validity; `IsPasswordVisible` and `IsConfirmPasswordVisible` toggle properties
- [x] 7.2 Create `Views/Auth/RegisterPage.xaml(.cs)` with vertical form: Email field (label + entry + error label), Password field (label + entry with eye toggle + strength indicator), Confirm Password field (label + entry with eye toggle + error label), "Create Account" primary button at bottom
- [x] 7.3 Register `RegisterViewModel` in `MauiProgram.cs` and inject into `RegisterPage`

## 8. Login Screen

- [x] 8.1 Create `ViewModels/LoginViewModel.cs` with: `Email`, `Password` observable properties; `IsPasswordVisible` toggle property; `LoginCommand` relay command; navigation commands for "Create Account" link
- [x] 8.2 Create `Views/Auth/LoginPage.xaml(.cs)` with vertical form: Email field (label + entry), Password field (label + entry with eye toggle), "Login" primary button, "Forgot password?" label (non-functional), "Create Account" tappable link
- [x] 8.3 Register `LoginViewModel` in `MauiProgram.cs` and inject into `LoginPage`

## 9. Final Wiring & Cleanup

- [x] 9.1 Verify all DI registrations in `MauiProgram.cs` are complete (all pages + all view models)
- [x] 9.2 Remove any remaining template artifacts (dotnet_bot.png reference, counter-related code)
- [x] 9.3 Build the project and verify it compiles without errors on at least one target framework
