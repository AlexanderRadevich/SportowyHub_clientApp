## MODIFIED Requirements

### Requirement: Chip close button removes individual filter
Each chip SHALL have a close button (X icon). Tapping the close button SHALL clear only that specific filter from `SearchFilterState`, update `ActiveFilterCount`, rebuild the chip list, and re-execute the search with remaining filters. When the last filter is removed and search text is empty, the system SHALL execute an unfiltered search to show all available items rather than clearing results.

#### Scenario: Remove sport filter chip
- **WHEN** user taps the close button on the "Hockey" sport chip
- **THEN** `SearchFilterState.SelectedSection` is set to null
- **AND** the chip is removed from the row
- **AND** search re-executes without the sport filter
- **AND** `ActiveFilterCount` decreases by 1

#### Scenario: Remove last filter chip with empty search text
- **WHEN** user taps close on the only remaining chip and search text is empty
- **THEN** the filter is cleared, the chip row hides, and an unfiltered search executes showing all available items

#### Scenario: Remove last filter chip with search text present
- **WHEN** user taps close on the only remaining chip and search text is non-empty
- **THEN** the filter is cleared, the chip row hides, and search re-executes using the search text only

## ADDED Requirements

### Requirement: Clear Filters closes popup and refreshes results
When user taps "Clear Filters" in the filter popup, the system SHALL reset all filter fields to defaults, close the popup, and trigger a search refresh with no filters applied.

#### Scenario: Clear Filters with active filters
- **WHEN** user opens filter popup with sport = "Hockey" and condition = "New" applied
- **AND** user taps "Clear Filters"
- **THEN** the popup closes
- **AND** all filters are cleared from `SearchFilterState`
- **AND** the chip row hides
- **AND** search re-executes with no filters (showing all items or text-only results)

#### Scenario: Clear Filters with no active filters
- **WHEN** user opens filter popup with no filters applied
- **AND** user taps "Clear Filters"
- **THEN** the popup closes
- **AND** search results remain unchanged (already unfiltered)
