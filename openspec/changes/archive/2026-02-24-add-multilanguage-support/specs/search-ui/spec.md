## MODIFIED Requirements

### Requirement: Search bar on Home screen
The Home screen SHALL display a search bar at the top of the page. The search bar SHALL be a styled container (`Border` with `RoundRectangle`) containing a magnifying glass icon on the left and localized placeholder text (`SearchPlaceholder`). It SHALL NOT be an editable input on the Home screen — tapping it navigates to the Search page.

#### Scenario: Search bar is visible on Home screen
- **WHEN** the Home screen is displayed
- **THEN** a search bar SHALL be visible at the top with a magnifying glass icon and localized placeholder text

#### Scenario: Search bar light theme styling
- **WHEN** the Home screen is in light theme
- **THEN** the search bar background SHALL be `#F1F3F6`, the icon color SHALL be `#39485F`, and the text color SHALL be `#39485F`

#### Scenario: Search bar dark theme styling
- **WHEN** the Home screen is in dark theme
- **THEN** the search bar background SHALL be `#1E1E1E` with a subtle border, and the text color SHALL be `#FFFFFF`

#### Scenario: Tapping the search bar navigates to Search page
- **WHEN** the user taps the search bar on the Home screen
- **THEN** the app SHALL navigate to the dedicated Search page

### Requirement: Search page with autofocus input
The Search page SHALL display a full editable search input field at the top that receives focus automatically when the page appears. The input SHALL use localized placeholder text (`SearchPlaceholder`). The input SHALL include a clear button (visible when text is entered) and a back button in the top-left corner.

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

### Requirement: Recent and popular searches display
Below the search input, the Search page SHALL display two sections with localized headers: "Recent Searches" (`SearchRecentSearches`) and "Popular Searches" (`SearchPopularSearches`). For MVP, these SHALL contain hardcoded placeholder data displayed as tappable items in vertical lists.

#### Scenario: Recent searches section is displayed
- **WHEN** the Search page is displayed and no text is entered
- **THEN** a section with a localized "Recent Searches" header SHALL be visible with placeholder items

#### Scenario: Popular searches section is displayed
- **WHEN** the Search page is displayed and no text is entered
- **THEN** a section with a localized "Popular Searches" header SHALL be visible below recent searches with placeholder items

#### Scenario: Tapping a search suggestion fills the input
- **WHEN** the user taps a recent or popular search item
- **THEN** that item's text SHALL be placed into the search input field
