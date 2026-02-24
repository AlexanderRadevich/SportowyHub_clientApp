### Requirement: Profile hub grouped-list layout
The Profile tab SHALL display a scrollable grouped-list layout containing a branding header and multiple sections. The layout SHALL use a `ScrollView` wrapping a `VerticalStackLayout`. The page title SHALL be sourced from the localized resource `TabProfile`.

#### Scenario: Profile hub renders with branding and sections
- **WHEN** the user navigates to the Profile tab
- **THEN** the page SHALL display the SportowyHub logo (`logo_full.png`) at the top, followed by an "Account" section and a "Settings" section, all within a scrollable container

#### Scenario: Profile hub is scrollable
- **WHEN** the content exceeds the visible area
- **THEN** the user SHALL be able to scroll vertically to see all sections

### Requirement: Section header styling
Each section SHALL have a header label displayed on a `Surface`-colored background strip. The header label SHALL use the `SectionHeader` style (semibold, 16px). The header strip SHALL have horizontal padding of 16px and vertical padding of 8px.

#### Scenario: Section header appearance in light theme
- **WHEN** the profile hub is displayed in light theme
- **THEN** section headers SHALL display bold text on a `Surface` (#F7F9FC) background strip

#### Scenario: Section header appearance in dark theme
- **WHEN** the profile hub is displayed in dark theme
- **THEN** section headers SHALL display bold text on a `SurfaceDark` (#1E1E1E) background strip

### Requirement: Account section with auth rows
The Account section SHALL display a section header with the localized text `ProfileAccountSection`. Below the header, the section SHALL contain two tappable rows: "Sign In" (`ProfileSignIn`) and "Create Account" (`ProfileCreateAccount`). Each row SHALL display the localized label on the left and a chevron indicator (">") on the right.

#### Scenario: Account section displays Sign In and Create Account rows
- **WHEN** the profile hub is displayed and the user is not logged in
- **THEN** the Account section SHALL show "Sign In" and "Create Account" as tappable rows with chevron indicators

#### Scenario: Tapping Sign In row navigates to Login page
- **WHEN** the user taps the "Sign In" row
- **THEN** the app SHALL navigate to the Login page via Shell navigation

#### Scenario: Tapping Create Account row navigates to Register page
- **WHEN** the user taps the "Create Account" row
- **THEN** the app SHALL navigate to the Registration page via Shell navigation

### Requirement: Settings section with picker rows
The Settings section SHALL display a section header with the localized text `ProfileSettingsSection`. Below the header, the section SHALL contain two rows: "Language" (`SettingsLanguage`) and "Theme" (`SettingsTheme`). Each row SHALL display the localized setting label on the left and the current selection value on the right. The settings section header label SHALL have `AutomationId="SettingsLabel"`. The language setting label SHALL have `AutomationId="LanguageLabel"`. The theme setting label SHALL have `AutomationId="ThemeLabel"`. The language Picker SHALL have `AutomationId="LanguagePicker"`. The theme Picker SHALL have `AutomationId="ThemePicker"`.

#### Scenario: Settings section displays Language and Theme rows
- **WHEN** the profile hub is displayed
- **THEN** the Settings section SHALL show "Language" and "Theme" rows with their current values displayed

#### Scenario: Settings elements are identifiable by AutomationId
- **WHEN** the profile hub is inspected via Appium or accessibility tools
- **THEN** the settings section label SHALL be locatable by `AutomationId` `SettingsLabel`, the language picker by `LanguagePicker`, the theme picker by `ThemePicker`, the language label by `LanguageLabel`, and the theme label by `ThemeLabel`

### Requirement: Row visual styling
Each tappable row SHALL have a minimum height of 48px, horizontal padding of 16px, and a bottom border separator using the theme's `Border` color. Row labels SHALL use the default Label style (14px). The current value text on the right side of settings rows SHALL use `TextSecondary` color.

#### Scenario: Row styling in light theme
- **WHEN** a row is displayed in light theme
- **THEN** it SHALL have a `Border` (#E5E7EB) bottom separator, `TextPrimary` (#1A1A1A) label, and `TextSecondary` (#6B7280) value text

#### Scenario: Row styling in dark theme
- **WHEN** a row is displayed in dark theme
- **THEN** it SHALL have a `BorderDark` (#2A2A2A) bottom separator, `TextPrimaryDark` (#FFFFFF) label, and `TextSecondaryDark` (#B0B3B8) value text

### Requirement: ProfileViewModel with CommunityToolkit.Mvvm
The profile hub SHALL be backed by a `ProfileViewModel` using `CommunityToolkit.Mvvm`. The ViewModel SHALL expose `RelayCommand`s for Sign In and Create Account navigation, and properties for the selected language and theme bound to Picker controls.

#### Scenario: ViewModel provides navigation commands
- **WHEN** the ProfileViewModel is instantiated
- **THEN** it SHALL expose `SignInCommand` and `CreateAccountCommand` relay commands that navigate to the respective auth pages
