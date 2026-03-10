## ADDED Requirements

### Requirement: View count display on detail page
The listing detail page SHALL display a view count indicator below the location line, before the horizontal divider. The indicator SHALL consist of an eye icon followed by the formatted view count using `ViewCountFormatConverter`. The text SHALL use `FontSize="14"` with secondary text color. The indicator SHALL be hidden when `ViewCount` is `0` or when the listing has not yet loaded.

#### Scenario: Detail page shows view count
- **WHEN** the listing detail loads with `ViewCount` of `567`
- **THEN** the page SHALL display an eye icon and `"567"` below the location

#### Scenario: Detail page hides view count when zero
- **WHEN** the listing detail loads with `ViewCount` of `0`
- **THEN** the view count indicator SHALL NOT be visible

#### Scenario: View count hidden during loading
- **WHEN** the listing detail is still loading (skeleton state)
- **THEN** the view count indicator SHALL NOT be visible
