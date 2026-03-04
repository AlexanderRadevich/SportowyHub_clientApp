## Requirement: Post-login navigation with return URL

After a successful login (email/password or Google OAuth), the `LoginViewModel` SHALL check the `IReturnUrlService` for a stored return URL. If a return URL exists, the app SHALL navigate to that URL instead of the default `//home` route. If no return URL exists, the app SHALL navigate to `//home` as before.

### Scenario: Login success with return URL set
- **GIVEN** `IReturnUrlService` has a stored return URL of `"//favorites"`
- **AND** the user is on the login page
- **WHEN** the user logs in successfully via email/password
- **THEN** the app SHALL call `ConsumeReturnUrl()` to retrieve `"//favorites"`
- **AND** the app SHALL navigate with `GoToAsync("..")`
- **AND** the app SHALL navigate with `GoToAsync("//favorites")`

### Scenario: Login success without return URL
- **GIVEN** `IReturnUrlService` has no stored return URL
- **AND** the user is on the login page
- **WHEN** the user logs in successfully via email/password
- **THEN** `ConsumeReturnUrl()` SHALL return null
- **AND** the app SHALL navigate with `GoToAsync("..")`
- **AND** the app SHALL navigate with `GoToAsync("//home")`

### Scenario: Google OAuth login success with return URL set
- **GIVEN** `IReturnUrlService` has a stored return URL of `"//home"`
- **AND** the user is on the login page
- **WHEN** the user logs in successfully via Google OAuth
- **THEN** the app SHALL call `ConsumeReturnUrl()` to retrieve `"//home"`
- **AND** the app SHALL navigate with `GoToAsync("..")`
- **AND** the app SHALL navigate with `GoToAsync("//home")`

### Scenario: Google OAuth login success without return URL
- **GIVEN** `IReturnUrlService` has no stored return URL
- **AND** the user is on the login page
- **WHEN** the user logs in successfully via Google OAuth
- **THEN** the app SHALL navigate with `GoToAsync("..")`
- **AND** the app SHALL navigate with `GoToAsync("//home")`

### Scenario: Login failure does not consume return URL
- **GIVEN** `IReturnUrlService` has a stored return URL of `"//favorites"`
- **AND** the user is on the login page
- **WHEN** the user attempts to log in and the login fails
- **THEN** `ConsumeReturnUrl()` SHALL NOT be called
- **AND** the return URL SHALL remain stored for the next successful login attempt

### Scenario: Return URL is consumed exactly once
- **GIVEN** `IReturnUrlService` has a stored return URL
- **WHEN** the user logs in successfully
- **AND** `ConsumeReturnUrl()` is called
- **THEN** subsequent calls to `ConsumeReturnUrl()` SHALL return null
- **AND** subsequent logins SHALL navigate to `//home`

### Scenario: LoginViewModel injects IReturnUrlService
- **GIVEN** the `LoginViewModel` constructor
- **WHEN** it is constructed via DI
- **THEN** it SHALL accept an `IReturnUrlService` parameter
