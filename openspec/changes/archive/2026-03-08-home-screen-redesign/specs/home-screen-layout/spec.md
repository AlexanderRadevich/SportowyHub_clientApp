## ADDED Requirements

### Requirement: Home page header with app title and action buttons
The Home page SHALL display a header row containing the app title ("SportowyHub") on the left and three `ImageButton` controls on the right in a `HorizontalStackLayout`: theme toggle, favorites, and shopping cart.

#### Scenario: Header layout
- **WHEN** the Home page loads
- **THEN** the header SHALL show the app title aligned left and three icon buttons aligned right

#### Scenario: Theme toggle button
- **WHEN** the user taps the theme toggle button
- **THEN** the app SHALL switch between light and dark theme using the existing theme service

#### Scenario: Favorites button
- **WHEN** the user taps the favorites button
- **THEN** the app SHALL navigate to the Favorites/Saved tab via Shell navigation

#### Scenario: Shopping cart button
- **WHEN** the user taps the shopping cart button
- **THEN** no action SHALL be performed (placeholder for future implementation)

### Requirement: Search bar below header
The Home page SHALL display a styled search bar below the header. The search bar SHALL be a non-editable `Border` with a search icon, placeholder text, and a filter icon on the right. Tapping it SHALL navigate to the Search tab.

#### Scenario: Tap search bar
- **WHEN** the user taps the search bar
- **THEN** the app SHALL navigate to the Search tab via `GoToSearchCommand`

### Requirement: Category chips section
The Home page SHALL display a horizontally scrollable row of category chips below the search bar. The first three chips SHALL be condition filters (ALL, NEW, USED). Sport-section chips from the backend SHALL follow after the condition chips.

#### Scenario: Display condition and sport chips
- **WHEN** the Home page loads with sections available
- **THEN** the chip row SHALL show ALL, NEW, USED chips followed by sport-section chips

#### Scenario: Display condition chips only
- **WHEN** the Home page loads with no sport sections available
- **THEN** the chip row SHALL show only ALL, NEW, USED chips

#### Scenario: Selected chip visual state
- **WHEN** a chip is selected
- **THEN** the selected chip SHALL have a filled/dark background with contrasting text
- **THEN** unselected chips SHALL have an outlined/light background

#### Scenario: ALL chip selected by default
- **WHEN** the Home page first loads
- **THEN** the ALL chip SHALL be selected by default

### Requirement: Hot Picks horizontal carousel section
The Home page SHALL display a "Hot Picks" section below the chips with a section header ("HOT PICKS") on the left and a "SEE ALL →" button on the right. Below the header, a horizontal `CollectionView` SHALL display the first 6 listings as cards.

#### Scenario: Display Hot Picks
- **WHEN** listings are loaded
- **THEN** the Hot Picks section SHALL show up to 6 listing cards in a horizontal scroll

#### Scenario: Tap SEE ALL
- **WHEN** the user taps "SEE ALL →"
- **THEN** the app SHALL scroll to or focus on the "All Products" section below

#### Scenario: Tap Hot Picks card
- **WHEN** the user taps a listing card in Hot Picks
- **THEN** the app SHALL navigate to the listing detail page

### Requirement: All Products vertical grid section
The Home page SHALL display an "All Products" section below Hot Picks. The section header SHALL show "ALL PRODUCTS" on the left with a result count ("Showing N results") below it, and a "SORT" button on the right. Listings SHALL display in a 2-column `GridItemsLayout`.

#### Scenario: Display All Products grid
- **WHEN** listings are loaded
- **THEN** the All Products section SHALL display listings in a 2-column grid layout

#### Scenario: Result count label
- **WHEN** listings are loaded with a total count
- **THEN** the label SHALL show "Showing {total} results"

#### Scenario: Sort button placeholder
- **WHEN** the user taps the SORT button
- **THEN** no action SHALL be performed (placeholder for future sort implementation)

### Requirement: Unified scrolling with CollectionView.Header
The Home page SHALL use a single `CollectionView` for the All Products grid as the root scrollable element. The header, search bar, chips, and Hot Picks sections SHALL be placed inside the `CollectionView.Header` property to enable unified scrolling without nested `ScrollView`.

#### Scenario: Smooth scrolling
- **WHEN** the user scrolls the page
- **THEN** the header, search bar, chips, Hot Picks, and All Products grid SHALL scroll as a single continuous surface

#### Scenario: Infinite scroll preserved
- **WHEN** the user scrolls near the bottom of the All Products grid
- **THEN** the `RemainingItemsThresholdReachedCommand` SHALL trigger loading more listings
