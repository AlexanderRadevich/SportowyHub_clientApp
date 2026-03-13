# Tasks: Replace Static ViewModel State

## Tasks

- [x] Create `NavigateToSearchMessage` record in `Messages/` folder
- [x] Update `HomeViewModel` to send `NavigateToSearchMessage` via `WeakReferenceMessenger`
- [x] Update `SearchViewModel` to receive `NavigateToSearchMessage` and apply the section
- [x] Remove `internal static Section? PendingSportSection` from `SearchViewModel`
- [x] Ensure message registration/unregistration lifecycle is correct
- [x] Unit test: HomeViewModel sends message with correct section
- [x] Unit test: SearchViewModel receives message and applies section filter
