## Context

The app has a Home page with a placeholder "Coming Soon" message and a Search page with hardcoded suggestion data. The backend exposes three public endpoints: `/api/v1/listings` (PostgreSQL, paginated browse), `/api/v1/listings/{id}` (single listing by UUID or slug), and `/api/v1/search` (Elasticsearch full-text search with filters). The existing `IRequestProvider` + `SportowyHubJsonContext` infrastructure handles HTTP calls and JSON serialization. The `INavigationService` abstraction handles navigation.

Key backend details:
- `/api/v1/listings` returns `{ "listings": [...], "total": N }` where `total` is page count (not grand total)
- `/api/v1/listings/{id}` returns a flat listing object; accepts UUID or slug
- `/api/v1/search` returns `{ "items": [...], "total": N, "limit": N, "offset": N }` where `total` is grand total from Elasticsearch
- Search supports `q`, `category_id`, `sport`, `city`, `price_min`, `price_max`, `sort` (newest/price_asc/price_desc)
- Price is `string` in listings, `float` in search results; `category_id` is `int` in listings, `string` in search

## Goals / Non-Goals

**Goals:**
- Browse published listings on the Home page with pull-to-refresh and infinite scroll
- View full listing details on a dedicated page
- Search with real Elasticsearch queries, displaying results in the existing Search page
- Follow established MVVM patterns: primary constructors, CancellationToken propagation, INavigationService, source-generated JSON

**Non-Goals:**
- Filter UI (chips, bottom sheet) — search filters will be passed programmatically; filter UI is a future change
- Image/media display — listings may have images in the future but are not part of this change
- Private listing endpoints (create, edit, delete) — only public read endpoints
- Offline caching or local storage of listings
- Category/section dictionary endpoints

## Decisions

### 1. Separate models for listings vs search responses

The `/api/v1/listings` and `/api/v1/search` endpoints return different field sets and types (e.g., price is `string` vs `float`, category_id is `int` vs `string`). Rather than a single polymorphic model, use separate records: `ListingSummary` (from listings endpoint), `SearchResultItem` (from search endpoint), and `ListingDetail` (from detail endpoint). A shared `IListingCard` interface is unnecessary — the XAML DataTemplates will bind to concrete types.

**Alternative considered:** Single unified model with nullable fields. Rejected because the type mismatches (string vs float price) would require runtime parsing and lose source-gen benefits.

### 2. Home page: CollectionView with RemainingItemsThreshold

Use `CollectionView` with `RemainingItemsThreshold="5"` and `RemainingItemsThresholdReachedCommand` for incremental loading. The `/api/v1/listings` endpoint uses offset pagination. The Home VM tracks the current offset and whether more items are available (when returned count < limit, we've reached the end).

**Alternative considered:** ListView with infinite scroll behavior. Rejected because CollectionView is the modern MAUI standard with better performance and layout options.

### 3. Search page: debounced query with CancellationToken

When the user types in the search entry, debounce for 400ms then call `/api/v1/search`. Each new keystroke cancels the previous in-flight request via `CancellationTokenSource`. The existing suggestions UI (Recent/Popular) shows when search text is empty; search results replace suggestions when text is present.

**Alternative considered:** Explicit search button (no debounce). Rejected because real-time search is the expected UX for a search page and the backend already handles empty queries efficiently.

### 4. Listing detail navigation: pass ID via query parameter

Navigate with `listing-detail?id={id}`. The `ListingDetailViewModel` implements `IQueryAttributable`, receives the ID, and calls `/api/v1/listings/{id}`. This follows the exact pattern used by `EditProfileViewModel`.

### 5. ListingsService as singleton

`IListingsService` / `ListingsService` will be registered as singleton (like `IAuthService`). It takes `IRequestProvider` and calls the three endpoints. No local state is cached — each call goes to the API.

### 6. Home page gets a ViewModel

The Home page currently has no ViewModel (code-behind handles search bar tap). It will get a `HomeViewModel` with `LoadListingsCommand` (initial load + pull-to-refresh) and `LoadMoreListingsCommand` (incremental). The search bar tap navigation moves to the ViewModel via `INavigationService`.

## Risks / Trade-offs

- **[Price type mismatch]** → The listings endpoint returns price as `string` ("250.00") while search returns `float` (250.0). Models use the exact backend types; display formatting happens in the ViewModel/converter. No runtime conversion in the model layer.
- **[No grand total from listings endpoint]** → The `/api/v1/listings` `total` field is page count, not grand total. Infinite scroll will use "returned < limit" heuristic to detect end-of-list. This is reliable but means we cannot show "X results found" on the Home feed.
- **[Elasticsearch downtime]** → Search endpoint returns 503 when ES is down. The SearchViewModel will catch this and show a user-friendly error via `IToastService`.
- **[Search debounce during fast typing]** → Rapid typing could queue multiple requests. Mitigated by cancelling the previous `CancellationTokenSource` on each keystroke.
