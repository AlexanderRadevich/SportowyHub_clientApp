## MODIFIED Requirements

### Requirement: Listing detail page layout
The listing detail page SHALL display the listing title, price with currency, location (city + region), description, and published date. It SHALL also display a favorite toggle heart button in the toolbar area allowing the user to add or remove the listing from favorites.

#### Scenario: Display listing details
- **WHEN** a listing detail is loaded successfully
- **THEN** the page SHALL display title, formatted price, location, description, published date, and a favorite heart button

#### Scenario: Loading state
- **WHEN** the listing is being loaded
- **THEN** an `ActivityIndicator` SHALL be visible and content SHALL be hidden

#### Scenario: Error state
- **WHEN** the listing fails to load
- **THEN** a generic error message SHALL be displayed
