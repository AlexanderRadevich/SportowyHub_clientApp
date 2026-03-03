## ADDED Requirements

### Requirement: My Listings page registration
The app SHALL have a `MyListingsPage` registered as Shell route `my-listings`. The page SHALL hide the tab bar. The page SHALL use `MyListingsViewModel` as its `BindingContext`, injected via constructor. Both the page and ViewModel SHALL be registered in the DI container as transient services.

#### Scenario: My Listings route is navigable
- **WHEN** the user taps "My Listings" on the Profile page
- **THEN** the app SHALL navigate to the `my-listings` route and display the My Listings page

### Requirement: Status filter tabs
The My Listings page SHALL display a horizontal scrollable row of filter tabs at the top: All, Draft, Published, Pending, Rejected, Hidden. Tapping a tab SHALL reload the listing using the corresponding status filter (null for All, `"draft"`, `"published"`, `"pending_review"`, `"rejected"`, `"hidden"`). The selected tab SHALL be visually highlighted with the Primary theme color.

#### Scenario: All tab shows all listings
- **WHEN** the user selects the "All" tab
- **THEN** `GetMyListingsAsync(status: null)` SHALL be called and all listings SHALL be displayed

#### Scenario: Status tab filters listings
- **WHEN** the user selects the "Draft" tab
- **THEN** `GetMyListingsAsync(status: "draft")` SHALL be called and only draft listings SHALL be displayed

### Requirement: My Listings collection display
The page SHALL display a `CollectionView` of `MyListingSummary` items. Each item SHALL show: title, price with currency, city, status badge (color-coded), and created date. The collection SHALL support pull-to-refresh via `RefreshView`.

#### Scenario: Listing card displays all fields
- **WHEN** a listing with title="Bike", price=500, currency="PLN", status="published" is displayed
- **THEN** the card SHALL show "Bike", "500 PLN", a green "Published" badge, and the created date

#### Scenario: Status badge colors
- **WHEN** listings with different statuses are displayed
- **THEN** badges SHALL use: green for published, gray for draft, orange for pending_review, red for rejected, blue for hidden

### Requirement: Empty state per filter
When no listings exist for the selected filter, the page SHALL display a centered empty state message. For the "All" tab, the message SHALL include a prompt to create the first listing.

#### Scenario: Empty all tab shows create prompt
- **WHEN** the user has no listings and the "All" tab is selected
- **THEN** an empty state message SHALL be displayed encouraging the user to create their first listing

#### Scenario: Empty filtered tab shows no results message
- **WHEN** no listings match the selected status filter
- **THEN** an empty state message SHALL indicate no listings with that status

### Requirement: Listing item actions
Each listing card SHALL be tappable to navigate to the edit page. Each card SHALL display contextual action buttons based on its status: Draft (Publish, Delete), Published (Hide), Hidden (Publish, Delete), Rejected (Resubmit, Delete), Pending Review (no status actions).

#### Scenario: Tapping a listing navigates to edit
- **WHEN** the user taps a listing card
- **THEN** the app SHALL navigate to `create-edit-listing?id={listingId}`

#### Scenario: Publish action on draft listing
- **WHEN** the user taps the Publish button on a draft listing
- **THEN** `UpdateStatusAsync(id, "published")` SHALL be called and the list SHALL refresh

#### Scenario: Delete action shows confirmation
- **WHEN** the user taps the Delete button on a listing
- **THEN** a confirmation dialog SHALL appear, and on confirm, `DeleteListingAsync(id)` SHALL be called

#### Scenario: Resubmit action on rejected listing
- **WHEN** the user taps Resubmit on a rejected listing
- **THEN** `ResubmitForReviewAsync(id)` SHALL be called and the list SHALL refresh

### Requirement: Rejection reason display
For listings with status "rejected", the card SHALL display the `LastModeratorComment` text below the status badge in a warning-styled container.

#### Scenario: Rejected listing shows moderator comment
- **WHEN** a rejected listing has LastModeratorComment="Please add more details"
- **THEN** the card SHALL display "Please add more details" in a warning container

### Requirement: Create listing FAB
The page SHALL display a floating action button (FAB) or toolbar button to create a new listing. Tapping it SHALL navigate to `create-edit-listing` (no id parameter = create mode).

#### Scenario: FAB navigates to create page
- **WHEN** the user taps the create button
- **THEN** the app SHALL navigate to the create/edit listing page in create mode

### Requirement: My Listings localization
The app SHALL define localized strings across all 4 languages (pl, en, uk, ru) for: page title, filter tab labels (All, Draft, Published, Pending, Rejected, Hidden), empty state messages, action button labels (Publish, Hide, Resubmit, Delete), confirmation dialog texts, and status badge labels.

#### Scenario: All My Listings labels are localized
- **WHEN** the My Listings page is displayed in any supported language
- **THEN** all labels, buttons, tabs, and messages SHALL use localized strings from AppResources
