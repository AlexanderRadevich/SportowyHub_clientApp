## ADDED Requirements

### Requirement: Reusable ListingCardView control
The system SHALL provide a reusable `ListingCardView` ContentView in the `Controls/` folder. The card SHALL be used in both Hot Picks (horizontal carousel) and All Products (2-column grid) sections.

#### Scenario: Card used in Hot Picks
- **WHEN** a listing is displayed in the Hot Picks carousel
- **THEN** it SHALL use the `ListingCardView` with a fixed width (~180dp)

#### Scenario: Card used in All Products
- **WHEN** a listing is displayed in the All Products grid
- **THEN** it SHALL use the `ListingCardView` filling the available grid column width

### Requirement: Card image area with placeholder
The top portion of the card SHALL display a placeholder image area with a colored background based on the listing's `CategoryId`. The area SHALL have rounded top corners matching the card border radius.

#### Scenario: Placeholder displayed
- **WHEN** a listing card renders
- **THEN** the image area SHALL show a colored background determined by the `CategoryId`

### Requirement: Condition badge overlay
The card SHALL display a condition badge in the top-left corner of the image area. The badge SHALL show "NEW" with a dark/black background or "USED" with an orange/amber background. The badge SHALL only appear when condition data is available.

#### Scenario: NEW condition badge
- **WHEN** a listing has condition "new"
- **THEN** a badge with text "NEW" and dark background SHALL appear in the top-left corner

#### Scenario: USED condition badge
- **WHEN** a listing has condition "used"
- **THEN** a badge with text "USED" and orange background SHALL appear in the top-left corner

#### Scenario: No condition data
- **WHEN** a listing has no condition value
- **THEN** no badge SHALL be displayed

### Requirement: Favorite heart toggle on card
The card SHALL display a heart icon button in the top-right corner of the image area. The heart SHALL be filled when the listing is favorited and outlined when not.

#### Scenario: Favorited listing
- **WHEN** the listing is in the user's favorites
- **THEN** the heart icon SHALL appear filled

#### Scenario: Not favorited listing
- **WHEN** the listing is not in the user's favorites
- **THEN** the heart icon SHALL appear as an outline

#### Scenario: Toggle favorite from card
- **WHEN** the user taps the heart icon on a card
- **THEN** the favorite state SHALL toggle with optimistic UI update using `IFavoritesService`

### Requirement: Card text content
Below the image area, the card SHALL display the listing title and price. The title SHALL be a single line, truncated with ellipsis if too long.

#### Scenario: Display title and price
- **WHEN** a listing card renders
- **THEN** the title SHALL display on one line with `LineBreakMode="TailTruncation"`
- **THEN** the price SHALL display formatted with currency symbol

#### Scenario: Null price
- **WHEN** a listing has no price
- **THEN** the price area SHALL be empty or show a placeholder dash

### Requirement: Card tap navigation
Tapping anywhere on the card (except the heart button) SHALL navigate to the listing detail page.

#### Scenario: Tap card body
- **WHEN** the user taps the card body or image area
- **THEN** the app SHALL navigate to `listing-detail?id={listing.Id}`

### Requirement: Card theming
The card SHALL support light and dark themes. In light mode, the card background SHALL be white/light with subtle border or shadow. In dark mode, the card background SHALL be dark surface color.

#### Scenario: Light theme card
- **WHEN** the app is in light theme
- **THEN** the card SHALL use light background colors and dark text

#### Scenario: Dark theme card
- **WHEN** the app is in dark theme
- **THEN** the card SHALL use dark surface background and light text
