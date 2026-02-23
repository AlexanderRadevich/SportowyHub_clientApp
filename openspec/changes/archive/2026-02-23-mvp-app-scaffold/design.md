## Context

SportowyHub is a .NET 10 MAUI mobile app currently containing only the default template (counter demo). The project targets Android, iOS, macCatalyst, and Windows. It uses MAUI Shell with XAML source generation enabled (`MauiXamlInflator: SourceGen`), OpenSans fonts, and the standard `Colors.xaml` + `Styles.xaml` resource dictionaries.

The goal is to replace this template with a fully branded MVP shell: bottom tab navigation, search UI, authentication screens, and a light/dark design system — all UI-only with no backend.

## Goals / Non-Goals

**Goals:**
- Establish a clean, maintainable project structure that future features can build on
- Deliver all 6 screens (Home, Search, Favorites, Profile, Register, Login) with branded styling
- Implement theme-aware resource dictionaries matching the spec's color palette exactly
- Use MAUI Shell's built-in TabBar for bottom navigation (no third-party dependencies)
- Keep the code-behind minimal; use MVVM with CommunityToolkit.Mvvm for view models

**Non-Goals:**
- No backend API integration, HTTP clients, or data persistence
- No real authentication logic (screens are UI shells only)
- No product listings, payments, or content feeds
- No push notifications or background services
- No unit tests in this phase (structure will support adding them later)
- No custom renderers or platform-specific UI code

## Decisions

### 1. MVVM with CommunityToolkit.Mvvm

**Decision:** Adopt MVVM pattern using `CommunityToolkit.Mvvm` NuGet package.

**Rationale:** The auth screens need form state management (email, password, validation errors, button enabled/disabled). Code-behind would get messy fast. CommunityToolkit.Mvvm provides source-generated `[ObservableProperty]`, `[RelayCommand]`, and `INotifyPropertyChanged` — zero boilerplate. It's the official Microsoft-recommended MVVM toolkit for MAUI.

**Alternative considered:** Pure code-behind — simpler for static screens but becomes painful for forms with validation logic. Not worth the technical debt.

### 2. Project Structure — Feature Folders

**Decision:** Organize by feature, not by type.

```
/Views/Home/HomePage.xaml(.cs)
/Views/Search/SearchPage.xaml(.cs)
/Views/Favorites/FavoritesPage.xaml(.cs)
/Views/Profile/ProfilePage.xaml(.cs)
/Views/Auth/RegisterPage.xaml(.cs)
/Views/Auth/LoginPage.xaml(.cs)
/ViewModels/SearchViewModel.cs
/ViewModels/RegisterViewModel.cs
/ViewModels/LoginViewModel.cs
/Resources/Styles/Colors.xaml
/Resources/Styles/Styles.xaml
```

**Rationale:** Feature folders keep related files close. ViewModels get a separate top-level folder since MAUI's build system expects XAML pages in specific structures. Static pages (Home, Favorites) won't need view models initially.

**Alternative considered:** Flat `/Pages` folder — works for small apps but doesn't scale. Feature folders set the right pattern from day one.

### 3. Shell TabBar Navigation

**Decision:** Use MAUI Shell `<TabBar>` with 4 `<Tab>` items in `AppShell.xaml`.

```xml
<TabBar>
    <Tab Title="Home" Icon="icon_home.png">
        <ShellContent ContentTemplate="{DataTemplate views:HomePage}" />
    </Tab>
    <Tab Title="Search" Icon="icon_search.png">
        <ShellContent ContentTemplate="{DataTemplate views:SearchPage}" />
    </Tab>
    <Tab Title="Favorites" Icon="icon_favorites.png">
        <ShellContent ContentTemplate="{DataTemplate views:FavoritesPage}" />
    </Tab>
    <Tab Title="Profile" Icon="icon_profile.png">
        <ShellContent ContentTemplate="{DataTemplate views:ProfilePage}" />
    </Tab>
</TabBar>
```

**Rationale:** Shell TabBar gives us native bottom tabs on all platforms with minimal code. Active tab color is controlled via `Shell.TabBarForegroundColor` in the theme. No need for a custom tab bar control in the MVP.

**Alternative considered:** Custom `AbsoluteLayout` bottom bar — full design control but significant effort and platform inconsistencies. Overkill for MVP.

### 4. Auth Screens as Modal Navigation

**Decision:** Register and Login pages will be pushed via Shell navigation (`Shell.Current.GoToAsync`), not placed in the TabBar. They are reached from the Profile tab's logged-out state.

**Rationale:** Auth screens are transient flows, not persistent tabs. Modal-style navigation (push/pop) matches user expectation: tap "Create Account" → navigate to Register → complete or go back. Keeping them out of the TabBar avoids tab clutter.

### 5. Theme System — AppThemeBinding in Colors.xaml

**Decision:** Define semantic color keys in `Colors.xaml` using `AppThemeBinding` for light/dark values. Styles in `Styles.xaml` reference these keys.

```xml
<!-- Colors.xaml -->
<Color x:Key="Primary">#DE0F21</Color>
<Color x:Key="PrimaryDark">#FF3B4D</Color>
<Color x:Key="Background">{AppThemeBinding Light=#FFFFFF, Dark=#121212}</Color>
<Color x:Key="Surface">{AppThemeBinding Light=#F7F9FC, Dark=#1E1E1E}</Color>
<Color x:Key="TextPrimary">{AppThemeBinding Light=#1A1A1A, Dark=#FFFFFF}</Color>
<Color x:Key="TextSecondary">{AppThemeBinding Light=#6B7280, Dark=#B0B3B8}</Color>
```

**Rationale:** `AppThemeBinding` is MAUI's built-in mechanism for theme switching — it reacts to system theme automatically. Semantic names (`Surface`, `TextPrimary`) decouple screens from specific hex values. The spec's color table maps 1:1 to these keys.

**Alternative considered:** Separate `LightTheme.xaml` / `DarkTheme.xaml` merged dictionaries swapped at runtime — more flexible but more complex. `AppThemeBinding` is simpler and sufficient for two themes.

### 6. Search Bar — Custom Layout, Not MAUI SearchBar Control

**Decision:** Build the Home screen search bar as a styled `Frame` + `Entry` composite, not using the native `SearchBar` control.

**Rationale:** The spec requires a specific visual design (rounded corners 12-16px, custom background color `#F1F3F6`, magnifying glass icon on the left). The native `SearchBar` control renders differently per platform and is hard to style to match the spec. A `Border` with `RoundRectangle` + `Entry` gives pixel-perfect control.

Tapping the Home search bar will navigate to the dedicated SearchPage (autofocus, recent/popular searches) rather than searching inline.

### 7. Form Validation — ViewModel-Driven

**Decision:** Validation logic lives in view models. Errors are exposed as observable properties and displayed below each field.

- Email: regex validation on text change
- Password: length check + strength indicator (weak/medium/strong based on character variety)
- Confirm password: match check against password field
- Submit button: bound to `CanExecute` of the relay command (disabled until form is valid)

**Rationale:** CommunityToolkit.Mvvm's `[RelayCommand(CanExecute = ...)]` handles the disabled-button-until-valid pattern natively. Keeps validation testable and out of code-behind.

### 8. Icons — SVG Resources

**Decision:** Use SVG files in `Resources/Images/` for tab bar icons and UI icons (search, eye toggle, back arrow). MAUI converts these to platform-appropriate formats at build time.

**Rationale:** SVGs scale cleanly across all display densities. MAUI's build pipeline handles the conversion. No need for multiple PNG resolutions.

### 9. DI Registration

**Decision:** Register all pages and view models in `MauiProgram.cs` using `builder.Services`.

```csharp
builder.Services.AddTransient<SearchPage>();
builder.Services.AddTransient<SearchViewModel>();
// etc.
```

**Rationale:** Enables constructor injection of view models into pages. Standard MAUI pattern. Transient lifetime is correct for pages (new instance per navigation).

## Risks / Trade-offs

- **Shell TabBar styling limitations** → The native tab bar may not perfectly match the spec's visual design on all platforms. Mitigation: Accept platform-native tab rendering for MVP; custom tab bar can be explored in a future iteration if needed.

- **No backend means hardcoded UI states** → Auth screens will show validation but can't actually register/log in. Mitigation: Design view models with clear interfaces (e.g., `IAuthService`) so backend integration is a drop-in replacement later.

- **CommunityToolkit.Mvvm is the only new dependency** → This is a Microsoft-maintained package with broad adoption. Risk is minimal. Pinning to a stable version avoids surprises.

- **XAML source generation (`MauiXamlInflator: SourceGen`)** → This is a .NET 10 feature that may have edge cases. Mitigation: Already enabled in the template; if issues arise, individual pages can opt out via `Inflator="Runtime"` metadata.

- **Search UI is static** → Recent/popular searches will be hardcoded placeholder data. Mitigation: Data will come from observable collections in the view model, making it trivial to replace with real data later.
