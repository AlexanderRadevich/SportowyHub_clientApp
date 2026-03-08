## MODIFIED Requirements

### Requirement: FAB on home page for creating a listing
The Home page SHALL display a floating action button (FAB) overlaid on the All Products grid. The FAB SHALL be a 56x56 circular button with `+` text, positioned at the bottom-right corner with 16dp margin. The FAB SHALL float above the `CollectionView` using Grid overlay positioning, coexisting with the new 2-column grid layout.

#### Scenario: FAB visible for logged-in user
- **WHEN** the user is authenticated
- **THEN** the FAB SHALL be visible on the Home page, overlaying the All Products grid

#### Scenario: FAB hidden for anonymous user
- **WHEN** the user is not authenticated
- **THEN** the FAB SHALL not be visible

#### Scenario: FAB remains accessible during scroll
- **WHEN** the user scrolls through the All Products grid
- **THEN** the FAB SHALL remain fixed at the bottom-right corner of the screen
