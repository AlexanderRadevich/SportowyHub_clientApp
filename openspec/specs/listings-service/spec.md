## ADDED Requirements

### Requirement: IListingsService interface
The system SHALL define an `IListingsService` interface with three methods:
- `GetListingsAsync(int limit = 20, int offset = 0, CancellationToken ct = default)` returning `Task<ListingsResponse>`
- `GetListingAsync(string id, CancellationToken ct = default)` returning `Task<ListingDetail>`
- `SearchAsync(string? query = null, int? categoryId = null, string? sport = null, string? city = null, float? priceMin = null, float? priceMax = null, string? sort = null, int limit = 30, int offset = 0, CancellationToken ct = default)` returning `Task<SearchResponse>`

#### Scenario: Interface is injectable
- **WHEN** a ViewModel declares `IListingsService` as a constructor parameter
- **THEN** the DI container SHALL resolve and inject the registered implementation

### Requirement: ListingsService implementation
The system SHALL implement `ListingsService` using a primary constructor accepting `IRequestProvider`. It SHALL call the three public backend endpoints using `IRequestProvider` methods.

#### Scenario: Fetch listings page
- **WHEN** `GetListingsAsync(limit: 10, offset: 20)` is called
- **THEN** it SHALL call `GET /api/v1/listings?limit=10&offset=20` via `IRequestProvider.GetAsync<ListingsResponse>` and return the result

#### Scenario: Fetch single listing
- **WHEN** `GetListingAsync("abc-123")` is called
- **THEN** it SHALL call `GET /api/v1/listings/abc-123` via `IRequestProvider.GetAsync<ListingDetail>` and return the result

#### Scenario: Search with filters
- **WHEN** `SearchAsync(query: "hockey", priceMin: 50, sort: "newest", limit: 30, offset: 0)` is called
- **THEN** it SHALL call `GET /api/v1/search?q=hockey&price_min=50&sort=newest&limit=30&offset=0` via `IRequestProvider.GetAsync<SearchResponse>`, omitting null parameters from the query string

#### Scenario: CancellationToken propagation
- **WHEN** any method is called with a `CancellationToken`
- **THEN** it SHALL propagate the token to the `IRequestProvider` call

### Requirement: DI registration
`IListingsService` SHALL be registered as singleton in `MauiProgram.cs`, following the same pattern as `IAuthService`.

#### Scenario: Singleton registration
- **WHEN** the app starts
- **THEN** `IListingsService` SHALL be resolvable from the DI container as a singleton `ListingsService`
