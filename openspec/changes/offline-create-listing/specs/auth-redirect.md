## Requirement: Unauthenticated FAB tap redirects to login

When a non-authenticated user taps the Create Listing FAB on the home page, the app SHALL redirect to the login screen with a return URL instead of showing a toast error. If the user is authenticated but unverified (trust level `Unverified`), the existing toast error behavior SHALL remain unchanged.

### Scenario: Unauthenticated user taps FAB
- **GIVEN** the user is not authenticated (`IsLoggedInAsync` returns false)
- **WHEN** the user taps the Create Listing FAB
- **THEN** the app SHALL call `NavigateToLoginWithReturnUrlAsync("//home")`
- **AND** the app SHALL NOT show a toast error
- **AND** the app SHALL NOT navigate to `create-edit-listing`

### Scenario: Authenticated user with sufficient trust level taps FAB
- **GIVEN** the user is authenticated
- **AND** the user's trust level is not `Unverified`
- **WHEN** the user taps the Create Listing FAB
- **THEN** the app SHALL navigate to `create-edit-listing`

### Scenario: Authenticated user with Unverified trust level taps FAB
- **GIVEN** the user is authenticated
- **AND** the user's trust level is `Unverified`
- **WHEN** the user taps the Create Listing FAB
- **THEN** the app SHALL show a toast error with `CreateListingPhoneRequired` message
- **AND** the app SHALL NOT navigate to `create-edit-listing`

### Scenario: GoToCreateListing checks auth before trust level
- **GIVEN** the `HomeViewModel.GoToCreateListing` method
- **WHEN** the method executes
- **THEN** it SHALL check `IsLoggedInAsync()` first
- **AND** if not logged in, it SHALL redirect to login with return URL without checking trust level
