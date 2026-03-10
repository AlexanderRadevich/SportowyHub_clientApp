## ADDED Requirements

### Requirement: Search page split-pill filter chip layout
Each active filter chip on the search page SHALL use a split-pill layout with two distinct zones inside a single rounded pill border: a label zone (left) and a remove zone (right), separated by a vertical divider.

#### Scenario: Chip displays label zone with dot indicator
- **WHEN** an active filter chip is displayed on the search page
- **THEN** the label zone SHALL show a colored dot indicator (6×6 logical pixels) followed by the filter label text
- **THEN** the dot color SHALL use the accent/secondary theme color

#### Scenario: Chip displays remove zone with close icon
- **WHEN** an active filter chip is displayed on the search page
- **THEN** the remove zone SHALL display a "×" close icon (12×12 logical pixels)
- **THEN** the remove zone SHALL be visually separated from the label zone by a 1px vertical divider

#### Scenario: Remove zone tap target meets accessibility guidelines
- **WHEN** an active filter chip is displayed on the search page
- **THEN** the remove zone tappable area SHALL be at least 44 logical pixels wide and fill the full chip height

#### Scenario: Tapping remove zone removes the filter
- **WHEN** the user taps anywhere in the remove zone
- **THEN** the filter SHALL be removed (same behavior as existing RemoveFilterCommand)

#### Scenario: Tapping label zone does not remove the filter
- **WHEN** the user taps the label zone of a filter chip
- **THEN** no action SHALL occur — the filter SHALL remain active

### Requirement: Search page filter chip theming
Active filter chips SHALL support both light and dark themes using existing app theme resources.

#### Scenario: Chip renders in dark theme
- **WHEN** the app is in dark theme
- **THEN** the chip border stroke SHALL use the dark theme border color
- **THEN** the chip background SHALL use the dark theme search bar background color
- **THEN** the label text and close icon SHALL use the dark theme text/icon colors

#### Scenario: Chip renders in light theme
- **WHEN** the app is in light theme
- **THEN** the chip border stroke SHALL use the light theme border color
- **THEN** the chip background SHALL use the light theme search bar background color
- **THEN** the label text and close icon SHALL use the light theme text/icon colors

## REMOVED Requirements

### Requirement: Search page filter chip close button size
**Reason**: Replaced by the split-pill layout requirement which defines the complete chip structure including remove zone sizing and tap targets.
**Migration**: The new "Search page split-pill filter chip layout" requirement supersedes all close button size and spacing specifications.
