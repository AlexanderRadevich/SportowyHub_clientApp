## ADDED Requirements

### Requirement: Edit button on Account Profile page
The Account Profile page SHALL display an "Edit" button that navigates to the `edit-profile` route. The button SHALL be visible when profile data is loaded (not in loading or error state). The button SHALL pass the current profile data to the Edit Profile page.

#### Scenario: Edit button visible when profile is loaded
- **WHEN** the Account Profile page displays profile data
- **THEN** an "Edit" button SHALL be visible in the header or toolbar area

#### Scenario: Edit button navigates to edit page
- **WHEN** the user taps the Edit button
- **THEN** the app SHALL navigate to the `edit-profile` route with the current profile data

#### Scenario: Profile refreshes after returning from edit
- **WHEN** the user saves changes on the Edit Profile page and navigates back
- **THEN** the Account Profile page SHALL re-fetch the profile via `LoadProfileCommand` in `OnAppearing`
