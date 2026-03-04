## Requirement: Create Listing FAB is always visible

The "Create Listing" floating action button on the home page SHALL be visible to all users regardless of authentication state. The FAB SHALL NOT be conditionally hidden based on `IsLoggedIn`.

### Scenario: FAB visible when user is not logged in
- **GIVEN** the user is not authenticated
- **WHEN** the home page is displayed
- **THEN** the Create Listing FAB SHALL be visible

### Scenario: FAB visible when user is logged in
- **GIVEN** the user is authenticated
- **WHEN** the home page is displayed
- **THEN** the Create Listing FAB SHALL be visible

### Scenario: FAB does not have IsVisible binding to IsLoggedIn
- **GIVEN** the `HomePage.xaml` layout
- **WHEN** the FAB `Button` element is defined
- **THEN** it SHALL NOT have an `IsVisible` binding to `IsLoggedIn`
- **AND** it SHALL use the `FloatingActionButton` style
- **AND** it SHALL bind its `Command` to `GoToCreateListingCommand`
