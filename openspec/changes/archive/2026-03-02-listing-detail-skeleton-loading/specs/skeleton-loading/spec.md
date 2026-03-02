## ADDED Requirements

### Requirement: SkeletonBox placeholder control
The app SHALL provide a reusable `SkeletonBox` control that renders a rounded rectangle with a muted background color to indicate loading content. The control SHALL support configurable `HeightRequest` and `WidthRequest` for sizing. The background color SHALL adapt to the current theme (light/dark) using `AppThemeBinding`.

#### Scenario: Render skeleton placeholder in light theme
- **WHEN** a `SkeletonBox` is placed in the visual tree while the app is in light theme
- **THEN** it SHALL display a rounded rectangle with a muted light-gray background color and rounded corners

#### Scenario: Render skeleton placeholder in dark theme
- **WHEN** a `SkeletonBox` is placed in the visual tree while the app is in dark theme
- **THEN** it SHALL display a rounded rectangle with a muted dark-gray background color and rounded corners

#### Scenario: Configurable dimensions
- **WHEN** a `SkeletonBox` is given specific `HeightRequest` and `WidthRequest` values
- **THEN** it SHALL render at the specified dimensions

### Requirement: Skeleton placeholder visibility pattern
Pages using skeleton placeholders SHALL show `SkeletonBox` elements while content is loading and replace them with real content once data is available. The skeleton elements and real content elements SHALL be mutually exclusive — never both visible at the same time.

#### Scenario: Show skeleton while loading
- **WHEN** the page is in a loading state for a content section
- **THEN** `SkeletonBox` placeholders SHALL be visible in place of the content that is being loaded

#### Scenario: Replace skeleton with content
- **WHEN** the data for a content section finishes loading successfully
- **THEN** the `SkeletonBox` placeholders SHALL be hidden and replaced with the actual content
