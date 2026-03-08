## ADDED Requirements

### Requirement: Category-to-color mapping
The system SHALL provide a `CategoryToColorConverter` that maps `CategoryId` values to predefined background colors. Unknown category IDs SHALL fall back to a neutral gray color.

#### Scenario: Known category ID
- **WHEN** a listing has a recognized `CategoryId`
- **THEN** the placeholder background SHALL use the mapped color for that category

#### Scenario: Unknown category ID
- **WHEN** a listing has an unrecognized `CategoryId`
- **THEN** the placeholder background SHALL use a neutral gray fallback color

### Requirement: Color palette definition
The system SHALL define a palette of 8-10 distinct pastel/muted colors in the app resources, one per major sport category. Colors SHALL be visually distinct and work well in both light and dark themes.

#### Scenario: Colors defined in resources
- **WHEN** the app initializes
- **THEN** placeholder colors SHALL be available as static resources or via converter lookup

### Requirement: Placeholder visual composition
Each placeholder SHALL consist of a colored `BoxView` background filling the card image area. No sport icon overlay is required in the initial implementation — the colored background alone provides visual distinction.

#### Scenario: Placeholder rendering
- **WHEN** a listing card image area renders
- **THEN** it SHALL show a solid colored rectangle with the category-mapped color

#### Scenario: Consistent sizing
- **WHEN** placeholders render in Hot Picks or All Products
- **THEN** the image area SHALL maintain a consistent aspect ratio (~4:3 or square) within the card
