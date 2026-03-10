## ADDED Requirements

### Requirement: ViewCountFormatConverter value converter
The system SHALL provide a `ViewCountFormatConverter` implementing `IValueConverter` that formats integer view counts for display. The converter SHALL format values as follows: 0–999 displayed as-is (e.g., `"42"`), 1000–999999 as `"X.Xk"` (e.g., `"1.2k"`), 1000000+ as `"X.XM"` (e.g., `"1.2M"`). Trailing `.0` SHALL be omitted (e.g., 1000 → `"1k"`, not `"1.0k"`). The converter SHALL use `CultureInfo.InvariantCulture` for decimal formatting.

#### Scenario: Format small count
- **WHEN** the converter receives value `42`
- **THEN** it SHALL return `"42"`

#### Scenario: Format zero
- **WHEN** the converter receives value `0`
- **THEN** it SHALL return `"0"`

#### Scenario: Format thousands
- **WHEN** the converter receives value `1234`
- **THEN** it SHALL return `"1.2k"`

#### Scenario: Format exact thousand
- **WHEN** the converter receives value `1000`
- **THEN** it SHALL return `"1k"`

#### Scenario: Format millions
- **WHEN** the converter receives value `1500000`
- **THEN** it SHALL return `"1.5M"`

#### Scenario: Format exact million
- **WHEN** the converter receives value `1000000`
- **THEN** it SHALL return `"1M"`

#### Scenario: Null or non-integer input
- **WHEN** the converter receives `null` or a non-integer value
- **THEN** it SHALL return `"0"`

### Requirement: View count hidden when zero
The view count indicator SHALL be hidden (not rendered) when `ViewCount` is `0`. This ensures graceful degradation when the backend has not yet deployed the `view_count` field in API responses.

#### Scenario: View count is zero
- **WHEN** a listing has `ViewCount` equal to `0`
- **THEN** the view count indicator SHALL NOT be visible on the card or detail page

#### Scenario: View count is positive
- **WHEN** a listing has `ViewCount` greater than `0`
- **THEN** the view count indicator SHALL be visible with the eye icon and formatted count
