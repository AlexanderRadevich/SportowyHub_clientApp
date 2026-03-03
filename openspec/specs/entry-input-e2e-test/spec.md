# Entry Input E2E Test

## Purpose

Defines the E2E tests that verify text entry fields on Search, Login, and Register screens using xUnit and Appium.

## Requirements

### Requirement: Entry input E2E tests verify text input on all fields
The `EntryInputTests` class SHALL implement `IClassFixture<AppiumDriverFixture>` and contain ordered tests that navigate to each screen, type a known string into each Entry field, read the value back, and assert equality using xUnit `Assert.Equal()`. After each assertion the field SHALL be cleared. Tests SHALL use `[TestPriority]` for ordering: Search screen entries first, then Login screen entries, then Register screen entries.

#### Scenario: Search Entry accepts typed text
- **WHEN** the test navigates to the Search page and types "running shoes" into the SearchEntry field
- **THEN** `Assert.Equal("running shoes", actualText)` SHALL pass

#### Scenario: Login Email Entry accepts typed text
- **WHEN** the test navigates to the Login page and types "test@example.com" into the LoginEmailEntry field
- **THEN** `Assert.Equal("test@example.com", actualText)` SHALL pass

#### Scenario: Login Password Entry accepts typed text
- **WHEN** the test types "Password123!" into the LoginPasswordEntry field
- **THEN** `Assert.Equal("Password123!", actualText)` SHALL pass

#### Scenario: Register Email Entry accepts typed text
- **WHEN** the test navigates to the Register page and types "new@example.com" into the RegisterEmailEntry field
- **THEN** `Assert.Equal("new@example.com", actualText)` SHALL pass

#### Scenario: Register Phone Entry accepts typed text
- **WHEN** the test types "123456789" into the RegisterPhoneEntry field
- **THEN** `Assert.Equal("123456789", actualText)` SHALL pass

#### Scenario: Register Password Entry accepts typed text and length is verified
- **WHEN** the test types "StrongPass1!" into the RegisterPasswordEntry field
- **THEN** `Assert.Equal("StrongPass1!", actualText)` SHALL pass and `Assert.Equal(expectedLength, actualText.Length)` SHALL verify the length

#### Scenario: Register Confirm Password Entry accepts typed text and length is verified
- **WHEN** the test types "StrongPass1!" into the RegisterConfirmPasswordEntry field
- **THEN** `Assert.Equal("StrongPass1!", actualText)` SHALL pass and `Assert.Equal(expectedLength, actualText.Length)` SHALL verify the length

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
