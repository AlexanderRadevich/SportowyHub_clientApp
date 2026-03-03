# Home Create Listing FAB

## Purpose

Defines the floating action button (FAB) on the Home page that allows authenticated users to create a new listing, with trust-level-based access control.

## Requirements

### Requirement: FAB on home page for creating a listing
The Home page SHALL display a floating action button (FAB) overlaid on the listings feed. The FAB SHALL be a 56x56 circular button with `+` text, positioned at the bottom-right corner with 16dp margin, matching the existing FAB on MyListingsPage.

#### Scenario: FAB visible for logged-in user
- **WHEN** the user is authenticated
- **THEN** the FAB SHALL be visible on the Home page

#### Scenario: FAB hidden for anonymous user
- **WHEN** the user is not authenticated
- **THEN** the FAB SHALL not be visible

### Requirement: Auth state check on page appearing
The `HomeViewModel` SHALL inject `IAuthService` and expose an `IsLoggedIn` observable property. The auth state SHALL be checked each time the page appears, alongside the existing listings load.

#### Scenario: Check auth state on appearing
- **WHEN** the Home page appears
- **THEN** the ViewModel SHALL call `IAuthService.IsLoggedInAsync()` and set `IsLoggedIn` accordingly

#### Scenario: Auth state updates on re-appearing
- **WHEN** the user navigates away and returns to the Home page (e.g., after login or logout)
- **THEN** the `IsLoggedIn` property SHALL be re-evaluated

### Requirement: Navigate to create listing for TL1+ users
When a logged-in user with trust level TL1 or higher taps the FAB, the app SHALL navigate to the create/edit listing page.

#### Scenario: TL1 user taps FAB
- **WHEN** a user with trust level TL1 taps the FAB
- **THEN** the app SHALL navigate to `create-edit-listing` via `INavigationService`

#### Scenario: TL2+ user taps FAB
- **WHEN** a user with trust level TL2 or TL3 taps the FAB
- **THEN** the app SHALL navigate to `create-edit-listing` via `INavigationService`

### Requirement: Phone verification prompt for TL0 users
When a TL0 user taps the FAB, the app SHALL show an informational toast explaining that phone verification is required to create a listing. It SHALL NOT navigate to the create listing page.

#### Scenario: TL0 user taps FAB
- **WHEN** a user with trust level TL0 taps the FAB
- **THEN** the app SHALL show a toast with a message indicating phone verification is required
- **THEN** the app SHALL NOT navigate to the create listing page
