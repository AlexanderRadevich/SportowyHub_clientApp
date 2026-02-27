## ADDED Requirements

### Requirement: Account Profile placeholder page
The app SHALL have an `AccountProfilePage` registered as shell route `account-profile`. The page SHALL display a centered localized placeholder label. The page SHALL hide the tab bar (`Shell.TabBarIsVisible="False"`).

#### Scenario: Account Profile page displays placeholder
- **WHEN** the user navigates to the Account Profile page
- **THEN** a centered placeholder label SHALL be visible

#### Scenario: Account Profile page hides tab bar
- **WHEN** the Account Profile page is displayed
- **THEN** the shell tab bar SHALL NOT be visible

#### Scenario: Account Profile route is registered
- **WHEN** the app starts
- **THEN** the route `account-profile` SHALL be registered and navigable from the Profile tab
