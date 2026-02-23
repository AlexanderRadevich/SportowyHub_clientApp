## Why

SportowyHub needs its foundational mobile UI shell. The project currently contains only the default .NET 10 MAUI template (counter demo). Before any feature work can begin, the app needs its core navigation structure, screen shells, authentication flows, and a branded design system with light/dark theme support. This scaffold establishes the UI foundation that all future features will build on.

## What Changes

- Replace the default MAUI template content with a Shell-based bottom tab navigation (Home, Search, Favorites, Profile)
- Implement a branded design system with light and dark themes using the SportowyHub color palette (Primary Red `#DE0F21`, Dark Blue-Gray `#39485F`)
- Add a search bar on the Home screen and a dedicated Search screen with recent/popular searches UI
- Build Profile screen with logged-out state (welcome view with Create Account / Sign In buttons)
- Create Registration screen (email, password, confirm password) with real-time validation and password strength indicator
- Create Login screen (email, password) with show/hide toggle and forgot password link
- All screens are UI-only — no backend integration in this phase

## Capabilities

### New Capabilities
- `shell-navigation`: Bottom tab navigation with 4 tabs (Home, Search, Favorites, Profile), Shell routing, active tab highlighting, icons + labels
- `theming`: Light and dark theme resource dictionaries, brand color system, 8pt spacing, rounded corners, theme-aware styles for all controls
- `search-ui`: Search bar component on Home screen, dedicated Search page with autofocus, recent/popular searches layout, clear and back buttons
- `auth-screens`: Profile logged-out state, Registration screen with validation (email format, password strength, confirm match), Login screen with show/hide password, navigation between auth flows

### Modified Capabilities
<!-- None — this is a greenfield scaffold -->

## Impact

- **AppShell.xaml**: Replaced with TabBar containing 4 ShellContent items and bottom tab configuration
- **MainPage.xaml/.cs**: Gutted and rebuilt as the Home screen with search bar
- **App.xaml**: Resource dictionaries updated with brand colors and theme-aware styles
- **Resources/Styles/Colors.xaml**: Replaced with SportowyHub light/dark color palette
- **Resources/Styles/Styles.xaml**: Updated with branded control styles (buttons, inputs, labels, cards)
- **New pages**: SearchPage, FavoritesPage, ProfilePage, RegisterPage, LoginPage (XAML + code-behind)
- **New view models**: Optional — depends on design decision around MVVM adoption
- **Dependencies**: No new NuGet packages required for MVP (MAUI Shell + built-in controls only)
- **Platforms**: Android, iOS, macCatalyst, Windows (all targets preserved from template)
