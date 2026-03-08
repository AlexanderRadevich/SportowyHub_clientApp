## ADDED Requirements

### Requirement: Active filter chips row visibility
The Search page SHALL display a horizontal row of filter chips between the search bar and the results area. The row SHALL be visible only when at least one filter is active in `SearchFilterState`. The row SHALL be hidden when no filters are active.

#### Scenario: Filters active after applying from popup
- **WHEN** user applies filters via the filter popup (e.g., sport = "Hockey", condition = "New")
- **THEN** the chip row appears below the search bar showing one chip per active filter

#### Scenario: No active filters
- **WHEN** all filters are cleared (either via chip removal or filter reset)
- **THEN** the chip row is hidden

#### Scenario: Navigating from home sport category
- **WHEN** user taps a sport category on the Home page and navigates to Search
- **THEN** the chip row appears with a chip showing the selected sport name

### Requirement: One chip per active filter
Each active filter property in `SearchFilterState` SHALL render as a separate chip. The chip SHALL display a human-readable label:
- **Sport**: section name (e.g., "Hockey")
- **Category**: category name — requires storing the category name in filter state
- **Location**: the `SelectedLocationLabel` value
- **Price min**: "≥ {value}" format
- **Price max**: "≤ {value}" format
- **Condition**: localized condition label (`FilterConditionNew` or `FilterConditionUsed`)
- **Sort**: localized sort option label (`FilterSortNewest`, `FilterSortPriceAsc`, `FilterSortPriceDesc`)

#### Scenario: Multiple filters produce multiple chips
- **WHEN** user applies sport = "Hockey", condition = "New", and sort = "Price ↑"
- **THEN** three separate chips appear: "Hockey", "New", "Price ↑"

#### Scenario: Price range filters
- **WHEN** user sets price min = 50 and price max = 200
- **THEN** two chips appear: "≥ 50" and "≤ 200"

### Requirement: Chip close button removes individual filter
Each chip SHALL have a close button (X icon). Tapping the close button SHALL clear only that specific filter from `SearchFilterState`, update `ActiveFilterCount`, rebuild the chip list, and re-execute the search with remaining filters.

#### Scenario: Remove sport filter chip
- **WHEN** user taps the close button on the "Hockey" sport chip
- **THEN** `SearchFilterState.SelectedSection` is set to null
- **AND** the chip is removed from the row
- **AND** search re-executes without the sport filter
- **AND** `ActiveFilterCount` decreases by 1

#### Scenario: Remove last filter chip
- **WHEN** user taps close on the only remaining chip
- **THEN** the filter is cleared, the chip row hides, and search re-executes (or clears results if search text is also empty)

### Requirement: Horizontal scrolling for overflow
The chip row SHALL scroll horizontally when chips exceed the available screen width. The row SHALL NOT wrap to multiple lines.

#### Scenario: Many active filters
- **WHEN** user applies 5+ filters that exceed screen width
- **THEN** the chip row scrolls horizontally to reveal all chips

### Requirement: Chip visual style
Each chip SHALL be styled as a rounded pill (similar to home page sport category cards) with the filter label text and a close icon. The chip SHALL use theme-appropriate colors matching the existing search bar style.

#### Scenario: Light and dark theme
- **WHEN** app theme changes between light and dark
- **THEN** chip background, text, and close icon colors update to match the current theme
