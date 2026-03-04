## Requirement: Favorites sign-in uses return URL

When an unauthenticated user taps the sign-in button on the Favorites tab, the app SHALL navigate to the login screen with a return URL of `//favorites`. After successful login, the user SHALL be returned to the Favorites tab instead of the home page.

### Scenario: Unauthenticated user taps sign-in on Favorites
- **GIVEN** the user is not authenticated
- **AND** the user is on the Favorites tab
- **WHEN** the user taps the sign-in button
- **THEN** the app SHALL call `NavigateToLoginWithReturnUrlAsync("//favorites")`

### Scenario: User returns to Favorites after login
- **GIVEN** the user tapped sign-in on the Favorites tab
- **AND** the return URL `"//favorites"` was stored
- **WHEN** the user logs in successfully
- **THEN** the app SHALL navigate to `//favorites`
- **AND** the Favorites tab SHALL load the user's favorites via `AppearingCommand`

### Scenario: SignIn command uses NavigateToLoginWithReturnUrlAsync
- **GIVEN** the `FavoritesViewModel.SignIn` command
- **WHEN** it is invoked
- **THEN** it SHALL call `nav.NavigateToLoginWithReturnUrlAsync("//favorites")` instead of `nav.GoToAsync("login")`
