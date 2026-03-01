### Requirement: Recent searches persistence service

The app SHALL provide an `IRecentSearchesService` registered as a singleton in DI, backed by `Preferences` storage under the key `"recent_searches"`. The stored format SHALL be a JSON array of strings serialized with `System.Text.Json`.

#### Scenario: Load recent searches on first access
- **WHEN** `GetAll()` is called and no data exists in Preferences
- **THEN** the service SHALL return an empty list

#### Scenario: Load existing recent searches
- **WHEN** `GetAll()` is called and Preferences contains stored searches
- **THEN** the service SHALL return the searches in most-recent-first order

### Requirement: Add a search query to recent searches

The service SHALL add a query to the front of the list when `Add(string query)` is called. It SHALL deduplicate (case-insensitive), trim the list to a maximum of 10 entries, and persist immediately to Preferences.

#### Scenario: Add a new unique query
- **WHEN** `Add("yoga mat")` is called and "yoga mat" is not in the list
- **THEN** "yoga mat" SHALL appear at position 0
- **THEN** the list SHALL be persisted to Preferences

#### Scenario: Add a duplicate query
- **WHEN** `Add("basketball")` is called and "Basketball" already exists in the list
- **THEN** the existing entry SHALL be removed and "basketball" SHALL be inserted at position 0
- **THEN** the total count SHALL not increase

#### Scenario: Exceed maximum capacity
- **WHEN** `Add` is called and the list already contains 10 entries
- **THEN** the oldest entry (last position) SHALL be removed before adding the new entry
- **THEN** the list SHALL never exceed 10 entries

#### Scenario: Ignore empty or whitespace-only queries
- **WHEN** `Add` is called with an empty string or whitespace-only string
- **THEN** the list SHALL not change

### Requirement: Clear all recent searches

The service SHALL remove all stored recent searches when `Clear()` is called.

#### Scenario: Clear with existing entries
- **WHEN** `Clear()` is called and the list has entries
- **THEN** `GetAll()` SHALL return an empty list
- **THEN** the Preferences key SHALL be removed

#### Scenario: Clear with no entries
- **WHEN** `Clear()` is called and the list is already empty
- **THEN** no error SHALL occur

### Requirement: Record searches from SearchViewModel

The `SearchViewModel` SHALL call `IRecentSearchesService.Add(query)` after a successful initial search execution (offset == 0, no error).

#### Scenario: Successful search records query
- **WHEN** a search for "running shoes" completes successfully with results
- **THEN** "running shoes" SHALL be added to recent searches

#### Scenario: Failed search does not record
- **WHEN** a search fails with a network error
- **THEN** the query SHALL NOT be added to recent searches

#### Scenario: Pagination does not record
- **WHEN** a load-more request executes (offset > 0)
- **THEN** the query SHALL NOT be added to recent searches again
