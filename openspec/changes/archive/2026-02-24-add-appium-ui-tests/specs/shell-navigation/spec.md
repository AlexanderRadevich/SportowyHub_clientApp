## MODIFIED Requirements

### Requirement: Bottom tab bar with four tabs
The app SHALL display a fixed bottom tab bar with exactly four tabs: Home, Search, Favorites, and Profile. The tab bar SHALL be implemented using MAUI Shell `<TabBar>` in `AppShell.xaml`. Each tab SHALL display an icon and a localized text label. Tab titles SHALL be sourced from `AppResources` localized resources (`TabHome`, `TabSearch`, `TabFavorites`, `TabProfile`). Each `ShellContent` item SHALL have an `AutomationId` attribute: `TabHome`, `TabSearch`, `TabFavorites`, and `TabProfile` respectively.

#### Scenario: App launches with Home tab active
- **WHEN** the app launches
- **THEN** the Home tab SHALL be selected by default and the HomePage content SHALL be visible

#### Scenario: User taps a tab
- **WHEN** the user taps any tab (Home, Search, Favorites, or Profile)
- **THEN** the corresponding page SHALL be displayed and the tapped tab SHALL become the active tab

#### Scenario: Tab labels display in system language
- **WHEN** the app launches with the device language set to a supported language
- **THEN** the tab labels SHALL display in that language (e.g., "Strona główna", "Szukaj", "Ulubione", "Profil" for Polish)

#### Scenario: Tabs are identifiable by AutomationId
- **WHEN** the tab bar is inspected via Appium or accessibility tools
- **THEN** each tab SHALL be locatable by its `AutomationId`: `TabHome`, `TabSearch`, `TabFavorites`, `TabProfile`
