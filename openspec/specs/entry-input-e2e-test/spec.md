# Entry Input E2E Test

### Requirement: Entry input E2E tests verify text input on all fields
The `EntryInputTests` test fixture SHALL inherit `AppiumSetup` and contain ordered tests that navigate to each screen, type a known string into each Entry field, read the value back, and assert equality. After each assertion the field SHALL be cleared. Tests SHALL be ordered: Search screen entries first, then Login screen entries, then Register screen entries.

#### Scenario: Search Entry accepts typed text
- **WHEN** the test navigates to the Search page and types "running shoes" into the SearchEntry field
- **THEN** the Entry's text value SHALL equal "running shoes"

#### Scenario: Login Email Entry accepts typed text
- **WHEN** the test navigates to the Login page and types "test@example.com" into the LoginEmailEntry field
- **THEN** the Entry's text value SHALL equal "test@example.com"

#### Scenario: Login Password Entry accepts typed text
- **WHEN** the test types "Password123!" into the LoginPasswordEntry field
- **THEN** the Entry's text value SHALL equal "Password123!"

#### Scenario: Register Email Entry accepts typed text
- **WHEN** the test navigates to the Register page and types "new@example.com" into the RegisterEmailEntry field
- **THEN** the Entry's text value SHALL equal "new@example.com"

#### Scenario: Register Phone Entry accepts typed text
- **WHEN** the test types "123456789" into the RegisterPhoneEntry field
- **THEN** the Entry's text value SHALL equal "123456789"

#### Scenario: Register Password Entry accepts typed text
- **WHEN** the test types "StrongPass1!" into the RegisterPasswordEntry field
- **THEN** the Entry's text value SHALL equal "StrongPass1!"

#### Scenario: Register Confirm Password Entry accepts typed text
- **WHEN** the test types "StrongPass1!" into the RegisterConfirmPasswordEntry field
- **THEN** the Entry's text value SHALL equal "StrongPass1!"

### Requirement: SearchPage page object exposes text entry methods
The `SearchPage` page object SHALL expose `TypeSearch(string text)`, `GetSearchText()`, and `ClearSearch()` methods that interact with the `SearchEntry` element via `MobileBy.Id("SearchEntry")`.

#### Scenario: SearchPage TypeSearch enters text
- **WHEN** `TypeSearch("test")` is called
- **THEN** the SearchEntry field SHALL contain "test"

#### Scenario: SearchPage ClearSearch clears the field
- **WHEN** `ClearSearch()` is called after typing
- **THEN** the SearchEntry field SHALL be empty

### Requirement: LoginPage page object exposes text entry methods
The `LoginPage` page object SHALL expose `TypeEmail()`, `TypePassword()`, `GetEmailText()`, `GetPasswordText()`, `ClearEmail()`, and `ClearPassword()` methods that interact with `LoginEmailEntry` and `LoginPasswordEntry` via `MobileBy.Id()`.

#### Scenario: LoginPage TypeEmail enters text
- **WHEN** `TypeEmail("test@example.com")` is called
- **THEN** the LoginEmailEntry field SHALL contain "test@example.com"

#### Scenario: LoginPage ClearEmail clears the field
- **WHEN** `ClearEmail()` is called after typing
- **THEN** the LoginEmailEntry field SHALL be empty

### Requirement: RegisterPage page object exposes text entry methods
The `RegisterPage` page object SHALL expose type, get-text, and clear methods for all four fields: Email (`RegisterEmailEntry`), Phone (`RegisterPhoneEntry`), Password (`RegisterPasswordEntry`), and ConfirmPassword (`RegisterConfirmPasswordEntry`) via `MobileBy.Id()`.

#### Scenario: RegisterPage TypeEmail enters text
- **WHEN** `TypeEmail("new@example.com")` is called
- **THEN** the RegisterEmailEntry field SHALL contain "new@example.com"

#### Scenario: RegisterPage ClearAll clears all fields
- **WHEN** clear methods are called for all four fields after typing
- **THEN** all four Entry fields SHALL be empty
