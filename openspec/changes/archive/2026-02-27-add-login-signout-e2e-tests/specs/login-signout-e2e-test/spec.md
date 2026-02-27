## ADDED Requirements

### Requirement: Configurable test credentials
`TestConfig` SHALL expose `TestEmail` and `TestPassword` string constants for login E2E tests. The default values SHALL be `"alex10@gmail.com"` and `"qwerty12345"` respectively.

#### Scenario: Test credentials are accessible from TestConfig
- **WHEN** a test class references `TestConfig.TestEmail` and `TestConfig.TestPassword`
- **THEN** the values SHALL be `"alex10@gmail.com"` and `"qwerty12345"`

### Requirement: Login button AutomationId
The Login page (`LoginPage.xaml`) SHALL have `AutomationId="LoginButton"` on the login submit button so it can be targeted by E2E tests.

#### Scenario: Login button is findable by AutomationId
- **WHEN** the Login page is displayed
- **THEN** an element with `AutomationId="LoginButton"` SHALL be present

### Requirement: LoginPage page object — TapLogin
The `LoginPage` page object SHALL expose a `TapLogin()` method that finds and taps the login button via `MobileBy.Id("LoginButton")`.

#### Scenario: TapLogin submits the login form
- **WHEN** `TapLogin()` is called after email and password have been entered
- **THEN** the login button SHALL be tapped, submitting the form

### Requirement: ProfilePage page object — TapAccountProfile
The `ProfilePage` page object SHALL expose a `TapAccountProfile()` method that taps the Account Profile row (visible when logged in). The element SHALL be found by a suitable locator.

#### Scenario: TapAccountProfile opens the account profile page
- **WHEN** the user is logged in and `TapAccountProfile()` is called on the Profile page
- **THEN** the Account Profile page SHALL open

### Requirement: ProfilePage page object — IsSignInRowVisible
The `ProfilePage` page object SHALL expose an `IsSignInRowVisible()` method that returns `true` if the Sign In row (`AutomationId="SignInRow"`) is visible, and `false` if it is not found within a short wait.

#### Scenario: IsSignInRowVisible returns true when logged out
- **WHEN** the user is not logged in and the Profile page is displayed
- **THEN** `IsSignInRowVisible()` SHALL return `true`

#### Scenario: IsSignInRowVisible returns false when logged in
- **WHEN** the user is logged in and the Profile page is displayed
- **THEN** `IsSignInRowVisible()` SHALL return `false`

### Requirement: AccountProfilePage page object
The test project SHALL have an `AccountProfilePage` page object class with a `TapSignOut()` method that taps the Sign Out button (`AutomationId="SignOutButton"`), and a `ConfirmSignOut()` method that taps the confirm button on the native Android `AlertDialog` by trying localized button texts across all 4 supported languages.

#### Scenario: TapSignOut taps the sign-out button
- **WHEN** `TapSignOut()` is called on the Account Profile page
- **THEN** the Sign Out button SHALL be tapped and the confirmation dialog SHALL appear

#### Scenario: ConfirmSignOut confirms the dialog
- **WHEN** `ConfirmSignOut()` is called while the sign-out confirmation dialog is displayed
- **THEN** the confirm button SHALL be tapped using localized text matching (pl: "Wyloguj", en: "Sign Out", uk: "Вийти", ru: "Выйти")

### Requirement: Toast assertion helper
The test project SHALL provide an `AssertNoErrorToast(AndroidDriver driver)` helper method (static, in a helper class or as an extension). The method SHALL use a short explicit wait (2 seconds) and `FindElements` (plural) to check for Snackbar elements. If any Snackbar element is found, the assertion SHALL fail with a descriptive message including the toast text.

#### Scenario: No toast present — assertion passes
- **WHEN** `AssertNoErrorToast()` is called and no Snackbar is visible
- **THEN** the assertion SHALL pass

#### Scenario: Error toast present — assertion fails
- **WHEN** `AssertNoErrorToast()` is called and a Snackbar with error text is visible
- **THEN** the assertion SHALL fail with a message containing the toast text

### Requirement: Login E2E test
The `LoginSignOutTests` test class SHALL contain a test (Order 1) that performs a full login flow: navigate to Profile tab → tap Sign In → enter `TestConfig.TestEmail` and `TestConfig.TestPassword` → tap Login → wait for navigation → assert the Home tab is active (by checking for a Home tab element) → assert no error toast appeared.

#### Scenario: Successful login navigates to Home
- **WHEN** the test enters valid credentials and taps Login
- **THEN** the app SHALL navigate to the Home tab
- **AND** no error toast SHALL be displayed

#### Scenario: Login test starts from logged-out state
- **WHEN** the test begins
- **THEN** the Profile page SHALL show the Sign In row (logged-out state)

### Requirement: Sign-out E2E test
The `LoginSignOutTests` test class SHALL contain a test (Order 2) that performs a full sign-out flow: navigate to Profile tab → tap Account Profile → tap Sign Out → confirm the dialog → wait for navigation back to Profile → assert the Sign In row is visible (logged-out state) → assert no error toast appeared.

#### Scenario: Successful sign-out returns to logged-out profile
- **WHEN** the test taps Sign Out and confirms the dialog
- **THEN** the app SHALL navigate back to the Profile page
- **AND** the Sign In row SHALL be visible
- **AND** no error toast SHALL be displayed

#### Scenario: Sign-out test runs after login
- **WHEN** the sign-out test begins
- **THEN** the app SHALL be in logged-in state (from the preceding login test)
