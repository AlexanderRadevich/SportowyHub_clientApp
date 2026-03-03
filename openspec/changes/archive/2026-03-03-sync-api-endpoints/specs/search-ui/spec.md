## MODIFIED Requirements

### Requirement: Text input triggers search
- **WHEN** the user types text into the search entry
- **THEN** the suggestions SHALL be hidden
- **THEN** the search results CollectionView SHALL be visible
- **THEN** after a 400ms debounce, the app SHALL call `IListingsService.SearchAsync` with `cityId` and `voivodeshipId` integer parameters instead of a `city` string parameter

#### Scenario: Search with city filter uses city ID
- **WHEN** the user selects a city filter and types a search query
- **THEN** the `SearchAsync` call SHALL pass the selected city's integer ID as `cityId` parameter and the voivodeship's integer ID as `voivodeshipId`, not a city name string

#### Scenario: Search without city filter omits location parameters
- **WHEN** the user searches without selecting a city filter
- **THEN** the `SearchAsync` call SHALL pass `cityId: null` and `voivodeshipId: null`
