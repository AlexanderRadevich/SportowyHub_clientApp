## MODIFIED Requirements

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
