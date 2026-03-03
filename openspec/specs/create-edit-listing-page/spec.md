## Purpose

Defines the Create/Edit Listing page, its ViewModel, form fields, photo management, save logic, and localization for creating new listings and editing existing ones.

## Requirements

### Requirement: Create/Edit Listing page registration
The app SHALL have a `CreateEditListingPage` registered as Shell route `create-edit-listing`. The page SHALL hide the tab bar. The page SHALL use `CreateEditListingViewModel` as its `BindingContext`, injected via constructor. Both SHALL be registered as transient in DI. The page SHALL accept an optional `id` query parameter; when present, the page operates in edit mode.

#### Scenario: Create mode when no id provided
- **WHEN** the user navigates to `create-edit-listing` without an id parameter
- **THEN** the page SHALL display an empty form in create mode with a "Create Listing" title

#### Scenario: Edit mode when id provided
- **WHEN** the user navigates to `create-edit-listing?id={listingId}`
- **THEN** the page SHALL load the existing listing via the listings service and pre-populate all form fields

### Requirement: Category two-step picker
The form SHALL include a category selection using a two-step flow: first select a Section, then select a Category within that section. The pickers SHALL be populated from `ISectionsService.GetSectionsAsync()` and `GetCategoriesAsync(sectionId)`. The selected `Category.Id` SHALL be stored as `CategoryId` on the request.

#### Scenario: Section selection loads categories
- **WHEN** the user selects a section from the first picker
- **THEN** the category picker SHALL be populated with categories from `GetCategoriesAsync(sectionId)`

#### Scenario: Category is required for create
- **WHEN** the user attempts to save without selecting a category
- **THEN** a validation error SHALL be displayed below the category picker

### Requirement: Listing form fields
The form SHALL include: Title (Entry, required, max 255), Description (Editor, multiline, max 5000), Price (Entry, numeric keyboard, optional), Currency (Picker with PLN/EUR/USD options, optional), City ID (Entry, numeric keyboard), Voivodeship ID (Entry, numeric keyboard), Content Locale (Picker with pl/en/uk/ru options). All fields SHALL use compiled bindings and follow the app's form styling pattern.

#### Scenario: Form displays all fields
- **WHEN** the create/edit page is displayed
- **THEN** all form fields SHALL be visible with localized labels

#### Scenario: Title validation
- **WHEN** the user attempts to save with an empty title
- **THEN** a validation error SHALL be displayed below the title field

### Requirement: Photo management section
The form SHALL include a photos section at the bottom showing uploaded images as a horizontal scrollable grid of thumbnails. Each thumbnail SHALL have a delete button overlay. An "Add Photo" button SHALL open the device media picker. Photos SHALL be uploaded via `IMediaService.UploadAsync` and deleted via `IMediaService.DeleteAsync`.

#### Scenario: Add photo in edit mode
- **WHEN** the user taps "Add Photo" on an existing listing
- **THEN** the media picker SHALL open, and the selected photo SHALL be uploaded to the listing

#### Scenario: Add photo in create mode
- **WHEN** the user taps "Add Photo" before saving a new listing
- **THEN** the photo SHALL be queued locally, and after the listing is created, all queued photos SHALL be uploaded

#### Scenario: Delete photo
- **WHEN** the user taps the delete button on a photo thumbnail
- **THEN** `IMediaService.DeleteAsync(mediaId)` SHALL be called and the thumbnail SHALL be removed

#### Scenario: Photo limit
- **WHEN** the listing already has 12 photos
- **THEN** the "Add Photo" button SHALL be hidden or disabled

### Requirement: Save command
The ViewModel SHALL expose a `SaveCommand` that validates required fields, then calls `CreateListingAsync` (create mode) or `UpdateListingAsync` (edit mode). On success in create mode, queued photos SHALL be uploaded, then the page SHALL navigate back. On success in edit mode, the page SHALL navigate back. On failure, error messages SHALL be displayed.

#### Scenario: Successful create
- **WHEN** the user fills required fields and taps Save in create mode
- **THEN** `CreateListingAsync` SHALL be called, queued photos SHALL be uploaded, a success toast SHALL appear, and the page SHALL navigate back

#### Scenario: Successful edit
- **WHEN** the user modifies fields and taps Save in edit mode
- **THEN** `UpdateListingAsync` SHALL be called, a success toast SHALL appear, and the page SHALL navigate back

#### Scenario: Validation error on save
- **WHEN** the user taps Save with missing required fields (title, category)
- **THEN** validation errors SHALL be displayed below the respective fields and the API SHALL NOT be called

#### Scenario: API error on save
- **WHEN** the API returns an error (e.g., LISTING_LIMIT_EXCEEDED)
- **THEN** the error message SHALL be displayed via toast and the user SHALL remain on the form

### Requirement: Loading and save states
The page SHALL show an `ActivityIndicator` while loading an existing listing in edit mode. The Save button SHALL be disabled while `IsSaving` is true.

#### Scenario: Loading state in edit mode
- **WHEN** the page opens in edit mode
- **THEN** an `ActivityIndicator` SHALL be visible until the listing data is loaded

#### Scenario: Save button disabled during save
- **WHEN** the user taps Save
- **THEN** the Save button SHALL be disabled until the operation completes

### Requirement: Create/Edit Listing localization
The app SHALL define localized strings across all 4 languages for: page titles (Create Listing, Edit Listing), field labels, picker items, validation messages, success/error messages, and photo section labels.

#### Scenario: All create/edit labels are localized
- **WHEN** the create/edit page is displayed in any supported language
- **THEN** all labels, buttons, and messages SHALL use localized strings from AppResources
