## MODIFIED Requirements

### Requirement: Profile logged-out state
When the user is not logged in, the Profile tab SHALL display a grouped-list hub layout. The Account section SHALL contain a "Sign In" row (`ProfileSignIn`) and a "Create Account" row (`ProfileCreateAccount`) as tappable list items with chevron indicators. Tapping "Sign In" SHALL navigate to the Login page. Tapping "Create Account" SHALL navigate to the Registration page. All text labels SHALL be sourced from `AppResources` localized resources.

#### Scenario: Profile shows account rows when logged out
- **WHEN** the user navigates to the Profile tab and is not logged in
- **THEN** the screen SHALL display an Account section containing "Sign In" and "Create Account" tappable rows with chevron indicators

#### Scenario: Create Account row navigates to Register page
- **WHEN** the user taps the "Create Account" row
- **THEN** the app SHALL navigate to the Registration page

#### Scenario: Sign In row navigates to Login page
- **WHEN** the user taps the "Sign In" row
- **THEN** the app SHALL navigate to the Login page
