## MODIFIED Requirements

### Requirement: Listing form fields
The form SHALL include: Title (Entry, required, max 255), Description (Editor, multiline, max 5000), Price (Entry, numeric keyboard, optional), Currency (Picker with PLN/EUR/USD options, optional), City ID (Entry, numeric keyboard), Voivodeship ID (Entry, numeric keyboard). All fields SHALL use compiled bindings and follow the app's form styling pattern. The form SHALL NOT include a Content Locale field — the content locale SHALL be determined automatically from the current app language.

#### Scenario: Form displays all fields
- **WHEN** the create/edit page is displayed
- **THEN** all form fields SHALL be visible with localized labels and no Content Locale field SHALL be present

#### Scenario: Title validation
- **WHEN** the user attempts to save with an empty title
- **THEN** a validation error SHALL be displayed below the title field

### Requirement: Save command
The ViewModel SHALL expose a `SaveCommand` that validates required fields, resolves the current app locale from `ILocaleService.GetLocaleInfoAsync()`, then calls `CreateListingAsync` (create mode) or `UpdateListingAsync` (edit mode) with the resolved locale as `ContentLocale`. On success in create mode, queued photos SHALL be uploaded, then the page SHALL navigate back. On success in edit mode, the page SHALL navigate back. On failure, error messages SHALL be displayed.

#### Scenario: Successful create
- **WHEN** the user fills required fields and taps Save in create mode
- **THEN** `ILocaleService.GetLocaleInfoAsync()` SHALL be called, `CreateListingAsync` SHALL be called with `ContentLocale` set to the resolved locale, queued photos SHALL be uploaded, a success toast SHALL appear, and the page SHALL navigate back

#### Scenario: Successful edit
- **WHEN** the user modifies fields and taps Save in edit mode
- **THEN** `ILocaleService.GetLocaleInfoAsync()` SHALL be called, `UpdateListingAsync` SHALL be called with `ContentLocale` set to the resolved locale, a success toast SHALL appear, and the page SHALL navigate back

#### Scenario: Validation error on save
- **WHEN** the user taps Save with missing required fields (title, category)
- **THEN** validation errors SHALL be displayed below the respective fields and the API SHALL NOT be called

#### Scenario: API error on save
- **WHEN** the API returns an error (e.g., LISTING_LIMIT_EXCEEDED)
- **THEN** the error message SHALL be displayed via toast and the user SHALL remain on the form
