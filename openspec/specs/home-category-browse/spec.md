# Home Category Browse

## Purpose

Defines the sport-section browsing experience on the Home page, allowing users to browse listings by sport category directly from the home screen.

## Requirements

### Requirement: Sport sections horizontal list on Home page
The Home page SHALL display a horizontal `CollectionView` of sport-section cards between the search bar and the listings feed. The sections SHALL be loaded from `ISectionsService.GetSectionsAsync` with the current app locale via `ILocaleService`.

#### Scenario: Sections load on page appearance
- **WHEN** the Home page appears
- **THEN** `ISectionsService.GetSectionsAsync` SHALL be called with the current locale
- **THEN** a horizontal scrollable row of section cards SHALL be displayed

#### Scenario: Sections display format
- **WHEN** sections are loaded
- **THEN** each card SHALL display the section name
- **THEN** cards SHALL be arranged in a horizontal `CollectionView` with `ItemsLayout` set to `HorizontalList`
- **THEN** the row height SHALL be compact (~60-80px) to not push the listings feed down significantly

#### Scenario: Sections load error
- **WHEN** `GetSectionsAsync` fails (network error)
- **THEN** the sections row SHALL be hidden
- **THEN** the listings feed SHALL still be visible and functional

#### Scenario: Empty sections
- **WHEN** `GetSectionsAsync` returns an empty list
- **THEN** the sections row SHALL be hidden

### Requirement: Navigate to filtered search from sport card
Tapping a sport-section card SHALL navigate to the Search tab with the sport pre-selected as a filter. The navigation SHALL pass the sport slug as a query parameter.

#### Scenario: Tap sport card
- **WHEN** the user taps the "Tennis" section card with slug "tennis"
- **THEN** the app SHALL switch to the Search tab
- **THEN** the search SHALL auto-execute with `sport="tennis"` filter applied
- **THEN** the filter badge SHALL show "1" indicating one active filter

#### Scenario: Search page receives sport parameter
- **WHEN** `SearchViewModel` receives a `sport` query attribute
- **THEN** it SHALL set the sport filter in `SearchFilterState`
- **THEN** it SHALL execute `SearchAsync` with `sport` parameter
- **THEN** the search text entry SHALL remain empty (browsing by category, not searching)

### Requirement: HomeViewModel sections state
`HomeViewModel` SHALL inject `ISectionsService` and `ILocaleService`. It SHALL expose `Sections` as an `ObservableCollection<Section>` and `HasSections` as a boolean property. Sections SHALL be loaded alongside listings on page appearance.

#### Scenario: Sections loaded successfully
- **WHEN** the Home page loads
- **THEN** `Sections` SHALL contain the loaded section items
- **THEN** `HasSections` SHALL be true

#### Scenario: Sections loading fails gracefully
- **WHEN** `GetSectionsAsync` throws an exception
- **THEN** `Sections` SHALL remain empty
- **THEN** `HasSections` SHALL be false
- **THEN** no error toast SHALL be shown for sections failure (non-critical feature)
