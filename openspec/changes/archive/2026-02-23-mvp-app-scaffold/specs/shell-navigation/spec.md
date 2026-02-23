## ADDED Requirements

### Requirement: Bottom tab bar with four tabs
The app SHALL display a fixed bottom tab bar with exactly four tabs: Home, Search, Favorites, and Profile. The tab bar SHALL be implemented using MAUI Shell `<TabBar>` in `AppShell.xaml`. Each tab SHALL display an icon and a text label.

#### Scenario: App launches with Home tab active
- **WHEN** the app launches
- **THEN** the Home tab SHALL be selected by default and the HomePage content SHALL be visible

#### Scenario: User taps a tab
- **WHEN** the user taps any tab (Home, Search, Favorites, or Profile)
- **THEN** the corresponding page SHALL be displayed and the tapped tab SHALL become the active tab

### Requirement: Active tab visual indicator
The active tab SHALL be visually distinguished from inactive tabs using the Primary color (`#DE0F21` in light theme, `#FF3B4D` in dark theme). Inactive tabs SHALL use a muted color (`Gray900` in light, `Gray200` in dark).

#### Scenario: Active tab highlighted in light theme
- **WHEN** a tab is selected and the app is in light theme
- **THEN** the tab icon and label SHALL be colored `#DE0F21` (Primary Red)

#### Scenario: Active tab highlighted in dark theme
- **WHEN** a tab is selected and the app is in dark theme
- **THEN** the tab icon and label SHALL be colored `#FF3B4D` (Primary Dark)

#### Scenario: Inactive tabs are muted
- **WHEN** a tab is not selected
- **THEN** its icon and label SHALL use the theme's unselected color

### Requirement: Tab icons as SVG resources
Each tab SHALL use an SVG icon from `Resources/Images/`. The icons SHALL be: `icon_home.svg`, `icon_search.svg`, `icon_favorites.svg`, `icon_profile.svg`. MAUI's build pipeline SHALL convert them to platform-appropriate formats.

#### Scenario: Tab icons render on all platforms
- **WHEN** the app runs on Android, iOS, macCatalyst, or Windows
- **THEN** all four tab icons SHALL render correctly at the appropriate display density

### Requirement: Tab bar persists across navigation
The bottom tab bar SHALL remain visible when navigating between tabs. It SHALL NOT be visible on modal pages pushed via `Shell.Current.GoToAsync` (e.g., Register, Login screens).

#### Scenario: Tab bar visible on main pages
- **WHEN** the user is on any of the four main tab pages
- **THEN** the bottom tab bar SHALL be visible

#### Scenario: Tab bar hidden on auth pages
- **WHEN** the user navigates to the Register or Login page
- **THEN** the bottom tab bar SHALL NOT be visible
