## Why

The Profile tab currently shows only a login/register welcome screen, which is a dead-end for users who aren't ready to sign in. Users need access to app settings (language, theme) regardless of authentication state, and the profile screen should feel like a useful hub — not just a gate to auth flows. Inspired by apps like Allegro, a grouped-list layout provides a familiar, scalable pattern for account and settings options.

## What Changes

- Replace the current Profile tab welcome layout with a grouped-list screen containing sections and tappable rows
- Add an "Account" section with Sign In and Create Account rows (visible when logged out)
- Add a "Settings" section with:
  - **Language** row — opens a picker/page to choose between Polish, English, Ukrainian, and Russian; selection is persisted and applied immediately without app restart
  - **Theme** row — toggles between Light, Dark, and System (auto) modes; selection is persisted and applied immediately
- Keep the SportowyHub logo/branding at the top of the profile screen for identity
- Section headers use bold text on a slightly contrasting background strip (like the Allegro reference)
- Each row is a simple label with a chevron/arrow indicator

## Capabilities

### New Capabilities
- `profile-hub`: The profile tab's grouped-list layout, section structure, and navigation to auth/settings screens
- `settings-theme-picker`: In-app manual theme selection (Light / Dark / System) with persistence
- `settings-language-picker`: In-app language selection from supported languages with persistence and live reload

### Modified Capabilities
- `auth-screens`: The Profile logged-out state changes from a centered welcome layout to an "Account" section within the new grouped-list profile hub. The Create Account and Sign In actions remain but move into list rows instead of standalone buttons.
- `theming`: Theme switching changes from system-only automatic detection to also supporting a user-selected override (Light / Dark / System) persisted in `Preferences`.
- `multilanguage-support`: Language selection changes from system-only automatic detection to also supporting a user-selected override persisted in `Preferences`, applied at runtime without restart.

## Impact

- **Views/Profile/ProfilePage.xaml** — complete redesign from welcome layout to grouped-list layout
- **ViewModels/** — new ProfileHubViewModel (or rename existing) to handle settings state and navigation commands
- **Resources/Strings/** — new resource keys for section headers, row labels, picker options (e.g., `SettingsLanguage`, `SettingsTheme`, `SettingsThemeLight`, `SettingsThemeDark`, `SettingsThemeSystem`, `AccountSignIn`, `AccountCreateAccount`)
- **App.xaml.cs** — theme override logic reading from `Preferences` before applying system default
- **Helpers/Services** — possible small settings service or direct `Preferences` usage for persisting theme and language choices
- **Resources/Styles/Colors.xaml / Styles.xaml** — may need new styles for section headers and list rows
