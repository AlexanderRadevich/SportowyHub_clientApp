## MODIFIED Requirements

### Requirement: IListingsService interface
The system SHALL define an `IListingsService` interface with three methods:
- `GetListingsAsync(int limit = 20, int offset = 0, CancellationToken ct = default)` returning `Task<ListingsResponse>`
- `GetListingAsync(string id, CancellationToken ct = default)` returning `Task<ListingDetail>`
- `SearchAsync(string? query = null, int? categoryId = null, string? sport = null, int? cityId = null, int? voivodeshipId = null, float? priceMin = null, float? priceMax = null, string? sort = null, int limit = 30, int offset = 0, CancellationToken ct = default)` returning `Task<SearchResponse>`

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

#### Scenario: Search with city ID and voivodeship ID
- **WHEN** `SearchAsync(query: "hockey", cityId: 42, voivodeshipId: 14, priceMin: 50, sort: "newest", limit: 30, offset: 0)` is called
- **THEN** it SHALL call `GET /api/v1/search?q=hockey&city_id=42&voivodeship_id=14&price_min=50&sort=newest&limit=30&offset=0` via `IRequestProvider.GetAsync<SearchResponse>`, omitting null parameters from the query string

#### Scenario: Search without location filters
- **WHEN** `SearchAsync(query: "tennis")` is called with cityId=null and voivodeshipId=null
- **THEN** the query string SHALL NOT include `city_id` or `voivodeship_id` parameters

#### Scenario: CancellationToken propagation
- **WHEN** any method is called with a `CancellationToken`
- **THEN** it SHALL propagate the token to the `IRequestProvider` call
