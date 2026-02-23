## MODIFIED Requirements

### Requirement: Profile logged-out state
When the user is not logged in, the Profile tab SHALL display a centered welcome layout containing: the SportowyHub logo (`logo_full.png`), a "Welcome" title, a primary "Create Account" button, and a secondary "Sign In" button.

#### Scenario: Profile shows welcome view when logged out
- **WHEN** the user navigates to the Profile tab and is not logged in
- **THEN** the screen SHALL display the SportowyHub logo (`logo_full.png`), "Welcome" title, "Create Account" primary button, and "Sign In" secondary button, all centered vertically

#### Scenario: Create Account button navigates to Register page
- **WHEN** the user taps the "Create Account" button
- **THEN** the app SHALL navigate to the Registration page

#### Scenario: Sign In button navigates to Login page
- **WHEN** the user taps the "Sign In" button
- **THEN** the app SHALL navigate to the Login page
