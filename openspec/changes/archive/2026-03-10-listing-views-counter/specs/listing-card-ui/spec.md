## ADDED Requirements

### Requirement: View count indicator on listing card
The listing card SHALL display a view count indicator below the price area. The indicator SHALL consist of an eye icon (`icon_eye.svg`, 12×12) followed by the formatted view count using `ViewCountFormatConverter`. The text SHALL use secondary text color (`TextSecondary`/`TextSecondaryDark`) with `FontSize="11"` and `FontFamily="OpenSansRegular"`. The indicator SHALL be hidden when `ViewCount` is `0`.

#### Scenario: Card shows view count
- **WHEN** a listing card renders with `ViewCount` of `1234`
- **THEN** the card SHALL display an eye icon and `"1.2k"` below the price

#### Scenario: Card hides view count when zero
- **WHEN** a listing card renders with `ViewCount` of `0`
- **THEN** the view count indicator row SHALL NOT be visible

#### Scenario: View count theming
- **WHEN** the app switches between light and dark themes
- **THEN** the eye icon tint and text color SHALL update to match the current theme's secondary text color
