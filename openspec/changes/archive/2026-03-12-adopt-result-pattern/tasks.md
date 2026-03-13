# Adopt Result Pattern -- Tasks

## Tasks

- [x] Create `Result<T>` type with `Success`/`Failure` factory methods in `Models/`
- [x] Refactor `IListingsService` and `ListingsService` to return `Result<T>` as pilot
- [x] Update `HomeViewModel`, `SearchViewModel`, and other `ListingsService` consumers
- [x] Roll out `Result<T>` to remaining service interfaces and implementations
- [x] Update all ViewModel consumers to use `Result` pattern matching
- [x] Replace `AuthResult<T>` with unified `Result<T>` in `AuthService`
- [x] Add unit tests for `Result<T>` type
- [x] Verify build and run existing test suite
