## MODIFIED Requirements

### Requirement: Search page with autofocus input
The Search page SHALL display a full editable search input field at the top that receives focus automatically when the page appears. The input SHALL use localized placeholder text (`SearchPlaceholder`). The input SHALL include a clear button (visible when text is entered) and a back button in the top-left corner. Both the back and clear icons SHALL use a theme-aware tint color: `Secondary` (#39485F) in light theme and `White` (#FFFFFF) in dark theme. The Search Entry SHALL have `AutomationId="SearchEntry"`.

#### Scenario: Search page input is auto-focused
- **WHEN** the Search page is displayed
- **THEN** the search input field SHALL be focused and the keyboard SHALL appear

#### Scenario: Search input shows localized placeholder
- **WHEN** the Search page is displayed and no text is entered
- **THEN** the search input SHALL display localized placeholder text

#### Scenario: Clear button appears when text is entered
- **WHEN** the user types text into the search input
- **THEN** a clear button SHALL appear inside the search field

#### Scenario: Clear button clears the input
- **WHEN** the user taps the clear button
- **THEN** the search input text SHALL be cleared

#### Scenario: Back button returns to previous page
- **WHEN** the user taps the back button on the Search page
- **THEN** the app SHALL navigate back to the previous page

#### Scenario: Search page icons adapt to dark theme
- **WHEN** the Search page is in dark theme
- **THEN** the back and clear icons SHALL be tinted `#FFFFFF` (White) and be clearly visible against the dark search bar background

#### Scenario: Search page icons adapt to light theme
- **WHEN** the Search page is in light theme
- **THEN** the back and clear icons SHALL be tinted `#39485F` (Secondary)

#### Scenario: Search Entry is locatable by AutomationId
- **WHEN** the Search page is displayed
- **THEN** the Search Entry SHALL have `AutomationId="SearchEntry"`
