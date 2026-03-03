## ADDED Requirements

### Requirement: My Listings row in Account section
When the user is logged in, the Account section SHALL display a "My Listings" tappable row (`ProfileMyListings`) with a chevron indicator, positioned between "Account Profile" and "Sign Out". Tapping it SHALL navigate to the `my-listings` route. The row SHALL NOT be visible when logged out.

#### Scenario: My Listings row visible when logged in
- **WHEN** the profile hub is displayed and the user is logged in
- **THEN** a "My Listings" row with a chevron indicator SHALL be visible between "Account Profile" and "Sign Out"

#### Scenario: My Listings row hidden when logged out
- **WHEN** the profile hub is displayed and the user is not logged in
- **THEN** the "My Listings" row SHALL NOT be visible

#### Scenario: Tapping My Listings navigates to my-listings
- **WHEN** the user taps the "My Listings" row
- **THEN** the app SHALL navigate to the `my-listings` Shell route

### Requirement: My Listings localization in profile hub
The app SHALL define the localized string `ProfileMyListings` across all 4 languages (pl, en, uk, ru).

#### Scenario: My Listings label is localized
- **WHEN** the profile hub is displayed in any supported language
- **THEN** the "My Listings" row label SHALL use the localized `ProfileMyListings` string
