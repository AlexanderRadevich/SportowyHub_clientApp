## Requirement: Profile sign-in uses return URL

When an unauthenticated user taps the sign-in button on the Profile tab, the app SHALL navigate to the login screen with a return URL of `//profile`. After successful login, the user SHALL be returned to the Profile tab instead of the home page.

### Scenario: Unauthenticated user taps sign-in on Profile
- **GIVEN** the user is not authenticated
- **AND** the user is on the Profile tab
- **WHEN** the user taps the sign-in button
- **THEN** the app SHALL call `NavigateToLoginWithReturnUrlAsync("//profile")`

### Scenario: User returns to Profile after login
- **GIVEN** the user tapped sign-in on the Profile tab
- **AND** the return URL `"//profile"` was stored
- **WHEN** the user logs in successfully
- **THEN** the app SHALL navigate to `//profile`
- **AND** the Profile tab SHALL refresh auth state via `RefreshAuthStateCommand`

### Scenario: SignInAsync command uses NavigateToLoginWithReturnUrlAsync
- **GIVEN** the `ProfileViewModel.SignInAsync` command
- **WHEN** it is invoked
- **THEN** it SHALL call `nav.NavigateToLoginWithReturnUrlAsync("//profile")` instead of `nav.GoToAsync("login")`

### Scenario: CreateAccountAsync remains unchanged
- **GIVEN** the `ProfileViewModel.CreateAccountAsync` command
- **WHEN** it is invoked
- **THEN** it SHALL continue to call `nav.GoToAsync("register")` without setting a return URL
- **AND** the registration flow SHALL follow its own email verification navigation
