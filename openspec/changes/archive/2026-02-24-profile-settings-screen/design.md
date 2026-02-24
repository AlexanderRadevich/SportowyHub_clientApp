## Context

The Profile tab currently renders a centered welcome layout (`ProfilePage.xaml`) with logo, "Create Account" button, and "Sign In" button. There is no ViewModel backing this page — navigation is handled via code-behind `Clicked` events. The app already has:

- **Multilanguage support** via `.resx` files (pl, en, uk, ru) with system-auto detection in `App` constructor
- **Theming** with light/dark semantic colors and `AppThemeBinding`, following system preference only
- **CommunityToolkit.Mvvm** for MVVM in other ViewModels (RegisterViewModel, LoginViewModel, SearchViewModel)
- **MAUI Shell** with 4-tab navigation and route-based `GoToAsync` for auth pages
- **MAUI `Preferences`** API available (no extra dependency needed) for key-value persistence

The reference design (Allegro app) shows a scrollable list with section headers on contrasting background strips and simple tappable rows with chevrons.

## Goals / Non-Goals

**Goals:**
- Replace the Profile tab with a grouped-list hub screen showing Account and Settings sections
- Allow users to select app language (pl/en/uk/ru) from within the app, persisted across sessions
- Allow users to select app theme (Light/Dark/System) from within the app, persisted across sessions
- Apply language and theme changes immediately without requiring app restart
- Follow existing MVVM patterns (CommunityToolkit.Mvvm) and localization conventions

**Non-Goals:**
- User account management (logged-in profile state, avatar, user info) — not in scope
- Separate settings page/modal — settings rows live inline on the profile hub with picker controls
- Cloud sync of preferences — local `Preferences` storage only
- Additional settings beyond language and theme

## Decisions

### 1. Inline settings with pickers vs. separate Settings page

**Decision:** Inline pickers directly on the profile hub page (tap row → shows picker/options).

**Rationale:** Fewer navigation levels, simpler implementation, matches the Allegro reference where options are directly accessible. A separate settings page adds a navigation layer with only 2 items in it.

**Alternative considered:** Dedicated `SettingsPage` pushed via Shell navigation. Rejected — overkill for 2 settings, and the user explicitly wanted everything on the profile screen.

### 2. Language picker UX — bottom sheet vs. in-page radio list vs. native Picker

**Decision:** Use a MAUI `Picker` control styled as a tappable row. When tapped, it shows the platform-native selection UI (dropdown on Windows, bottom wheel on iOS, dialog on Android).

**Rationale:** Zero custom UI code, platform-native feel, accessible by default. The row displays the current language name, and tapping it opens the native picker.

**Alternative considered:** Custom bottom sheet with radio buttons. Rejected — requires significant custom UI for no real benefit with only 4 options.

### 3. Theme picker UX — same approach

**Decision:** Use a MAUI `Picker` for theme selection (Light / Dark / System), same pattern as language.

**Rationale:** Consistent with language picker. Three options don't warrant a toggle or segmented control.

### 4. Persistence mechanism

**Decision:** Use `Microsoft.Maui.Storage.Preferences` for both language and theme preferences.

Keys:
- `app_language` → stores culture code string (`"pl"`, `"en"`, `"uk"`, `"ru"`, or `"system"` for auto-detect)
- `app_theme` → stores `"light"`, `"dark"`, or `"system"`

Default for both: `"system"` (current behavior preserved).

**Rationale:** Already available in MAUI, no extra dependencies, appropriate for simple key-value settings.

### 5. Applying theme changes at runtime

**Decision:** Set `Application.Current.UserAppTheme` to `AppTheme.Light`, `AppTheme.Dark`, or `AppTheme.Unspecified` (system). This is a one-liner that immediately switches all `AppThemeBinding` values.

On app startup in `App` constructor, read `Preferences.Get("app_theme", "system")` and apply before `InitializeComponent()`.

**Rationale:** MAUI's built-in `UserAppTheme` property is designed exactly for this. No need for custom theme switching infrastructure.

### 6. Applying language changes at runtime

**Decision:** On language change:
1. Set `CultureInfo.CurrentUICulture` and `CultureInfo.CurrentCulture` to the selected culture
2. Save to `Preferences`
3. Recreate `MainPage` by re-assigning `Application.Current.Windows[0].Page = new AppShell()` to force all pages to re-read `{x:Static}` resource references

On app startup in `App` constructor, read `Preferences.Get("app_language", "system")` before the existing culture detection logic. If not `"system"`, override to the stored culture.

**Rationale:** MAUI `{x:Static}` bindings are resolved once at page construction time and don't update dynamically. Recreating the Shell is the simplest reliable approach to refresh all strings. This is a standard MAUI pattern for runtime language switching.

**Alternative considered:** Using a `LocalizationResourceManager` with `INotifyPropertyChanged` bindings instead of `{x:Static}`. Rejected — would require refactoring all existing XAML localization references across every page, too invasive for this change.

### 7. Profile page layout structure

**Decision:** `ScrollView` → `VerticalStackLayout` containing:
1. Logo/branding at top (smaller than current, aligned left or center)
2. **Account** section: section header label + Border-wrapped stack of tappable rows (Sign In, Create Account)
3. **Settings** section: section header label + Border-wrapped stack of rows (Language picker, Theme picker)

Each row: `Grid` with label on the left, current value + chevron on the right, wrapped in a `TapGestureRecognizer` or using the Picker directly.

Section headers use the existing `SectionHeader` style on a `Surface`-colored background strip.

**Rationale:** Matches Allegro reference, uses existing styles, scrollable for future sections.

### 8. ViewModel approach

**Decision:** Create `ProfileViewModel` using `CommunityToolkit.Mvvm` with:
- `[RelayCommand]` for SignIn and CreateAccount navigation
- Properties for `SelectedLanguageIndex` and `SelectedThemeIndex` bound to Pickers
- Property change handlers that persist and apply settings

**Rationale:** Consistent with existing ViewModels. Moves logic out of code-behind.

## Risks / Trade-offs

**[Language change recreates entire Shell]** → This causes a brief visual flash. Mitigation: acceptable UX trade-off since language changes are infrequent. Users in the Allegro reference also see a similar reload pattern.

**[Language preference overrides system language]** → Users who set a language then change their phone language may be confused. Mitigation: "System" is the default option and always available to revert.

**[No confirmation dialog for settings changes]** → Changes apply instantly. Mitigation: both settings are easily reversible by the user, no destructive action.

**[Picker styling may vary across platforms]** → Native pickers look different on Android/iOS/Windows. Mitigation: this is expected and desirable (platform-native feel). Row styling is consistent.
