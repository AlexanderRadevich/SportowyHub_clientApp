## ADDED Requirements

### Requirement: Home page condition chips
The Home page SHALL display condition filter chips (ALL, NEW, USED) at the beginning of the category chips row. These chips SHALL be distinct from the search page filter chips — they filter the home page listings directly.

#### Scenario: Condition chips always visible
- **WHEN** the Home page loads
- **THEN** ALL, NEW, USED chips SHALL always be visible regardless of whether sport sections loaded

#### Scenario: Tap condition chip filters listings
- **WHEN** the user taps a condition chip (e.g., NEW)
- **THEN** the chip becomes selected (filled background)
- **THEN** listings on the home page reload with the selected condition filter

#### Scenario: Condition chips before sport chips
- **WHEN** both condition chips and sport section chips are available
- **THEN** the horizontal chip row SHALL show condition chips first, followed by sport section chips

### Requirement: Home page chip selection state
Only one chip SHALL be selected at a time in the condition group (ALL, NEW, USED). Sport section chips SHALL function independently — tapping a sport chip navigates to the Search page with that sport filter (existing behavior).

#### Scenario: Switch condition selection
- **WHEN** the user taps USED while NEW is selected
- **THEN** USED becomes selected (filled) and NEW becomes unselected (outlined)

#### Scenario: Sport chip tap does not change condition
- **WHEN** the user taps a sport section chip
- **THEN** the condition chip selection SHALL remain unchanged
- **THEN** the app SHALL navigate to the Search tab with the sport filter applied
