# Theming

### Requirement: Brand color palette in Colors.xaml
`Resources/Styles/Colors.xaml` SHALL define the SportowyHub brand palette replacing all default template colors. The following semantic color keys SHALL be defined:

| Key | Light Value | Dark Value |
|-----|------------|------------|
| Primary | `#DE0F21` | `#DE0F21` |
| PrimaryDark | `#FF3B4D` | `#FF3B4D` |
| Secondary | `#39485F` | `#39485F` |
| Background | `#FFFFFF` | `#121212` |
| Surface | `#F7F9FC` | `#1E1E1E` |
| TextPrimary | `#1A1A1A` | `#FFFFFF` |
| TextSecondary | `#6B7280` | `#B0B3B8` |
| Border | `#E5E7EB` | `#2A2A2A` |
| SearchBarBackground | `#F1F3F6` | `#1E1E1E` |

#### Scenario: Colors.xaml contains all semantic keys
- **WHEN** the app resources are loaded
- **THEN** all keys listed in the brand palette table SHALL be defined and resolvable

### Requirement: Automatic light/dark theme switching
The app SHALL support three theme modes: Light, Dark, and System. In System mode, the app SHALL follow the device's system theme preference using `AppThemeBinding`. In Light or Dark mode, the app SHALL override the system preference by setting `Application.Current.UserAppTheme`. The active theme mode SHALL be determined by the user's persisted preference in `Preferences` (key: `app_theme`), defaulting to System (`AppTheme.Unspecified`) if no preference is stored. The theme preference SHALL be applied in the `App` constructor before `InitializeComponent()`.

#### Scenario: Device is in light mode with System theme selected
- **WHEN** the device OS is set to light mode and the user's theme preference is "system"
- **THEN** all `AppThemeBinding` values SHALL resolve to their Light variants

#### Scenario: Device is in dark mode with System theme selected
- **WHEN** the device OS is set to dark mode and the user's theme preference is "system"
- **THEN** all `AppThemeBinding` values SHALL resolve to their Dark variants

#### Scenario: User has selected Light theme override
- **WHEN** the user's theme preference is "light" regardless of device OS setting
- **THEN** `Application.Current.UserAppTheme` SHALL be `AppTheme.Light` and all `AppThemeBinding` values SHALL resolve to their Light variants

#### Scenario: User has selected Dark theme override
- **WHEN** the user's theme preference is "dark" regardless of device OS setting
- **THEN** `Application.Current.UserAppTheme` SHALL be `AppTheme.Dark` and all `AppThemeBinding` values SHALL resolve to their Dark variants

#### Scenario: Theme override applied before UI loads
- **WHEN** the app starts with a stored theme preference
- **THEN** `UserAppTheme` SHALL be set before `InitializeComponent()` in the `App` constructor

### Requirement: Branded control styles in Styles.xaml
`Resources/Styles/Styles.xaml` SHALL define implicit and named styles for MAUI controls using the brand color palette. The implicit `Entry` style SHALL include a `CursorColor` setter with `AppThemeBinding` using `Primary` for light theme and `PrimaryDark` for dark theme.

#### Scenario: Primary button style
- **WHEN** a `Button` uses the `PrimaryButton` style
- **THEN** it SHALL have background `Primary` (light) / `PrimaryDark` (dark), text color `#FFFFFF`, corner radius 16px, and minimum height 50px

#### Scenario: Secondary button style
- **WHEN** a `Button` uses the `SecondaryButton` style
- **THEN** it SHALL have a transparent background, border color `Primary`, text color `Primary`, corner radius 16px, and minimum height 50px

#### Scenario: Default page background
- **WHEN** any `ContentPage` is displayed
- **THEN** its background color SHALL be the `Background` theme color

#### Scenario: Shell tab bar theming
- **WHEN** the Shell renders the tab bar
- **THEN** the tab bar background SHALL use `Background`, foreground (active) SHALL use `Primary`/`PrimaryDark`, and unselected tabs SHALL use `TextSecondary`

#### Scenario: Entry cursor is visible in light theme
- **WHEN** an `Entry` field is focused in light theme
- **THEN** the cursor color SHALL be `Primary` (`#DE0F21`)

#### Scenario: Entry cursor is visible in dark theme
- **WHEN** an `Entry` field is focused in dark theme
- **THEN** the cursor color SHALL be `PrimaryDark` (`#FF3B4D`)

### Requirement: 8pt spacing system
All padding, margins, and spacing values SHALL follow an 8pt grid system (multiples of 8: 8, 16, 24, 32, etc.). Consistent spacing SHALL be maintained across all screens and themes.

#### Scenario: Page content padding
- **WHEN** any main content page is displayed
- **THEN** horizontal padding SHALL be 16px (2 x 8pt) and content spacing SHALL use 8pt multiples

### Requirement: Rounded corners
Interactive elements (buttons, cards, search bar, input fields) SHALL use rounded corners between 12px and 16px as specified per component in the design spec.

#### Scenario: Button corner radius
- **WHEN** a primary or secondary button is rendered
- **THEN** its corner radius SHALL be 16px

#### Scenario: Search bar corner radius
- **WHEN** the search bar container is rendered
- **THEN** its corner radius SHALL be between 12px and 16px

### Requirement: Dark mode avoids pure black
The dark theme background SHALL be `#121212` (not `#000000`). Surface elements SHALL use `#1E1E1E`. Borders SHALL be used instead of shadows in dark mode for separation.

#### Scenario: Dark mode background is not pure black
- **WHEN** the app is in dark mode
- **THEN** the main background color SHALL be `#121212`

#### Scenario: Dark mode uses borders over shadows
- **WHEN** a card or elevated element is displayed in dark mode
- **THEN** it SHALL use a subtle border for visual separation rather than a drop shadow
