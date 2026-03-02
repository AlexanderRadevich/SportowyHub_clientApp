## Context

The listing detail page currently receives only the listing `id` via Shell query parameters. It shows a centered `ActivityIndicator` while the full `ListingDetail` is fetched from `GET /api/v1/listings/{id}`, which takes 1–5 seconds. All three navigation sources (Home feed, Search, Favorites) already hold summary data for the tapped listing — title, price, currency, and city — but none of this is forwarded.

Common fields available across all source models (`ListingSummary`, `SearchResultItem`, `FavoriteItem`):

| Field      | ListingSummary | SearchResultItem | FavoriteItem |
|------------|:-:|:-:|:-:|
| `Id`       | ✓ | ✓ | ✓ |
| `Title`    | ✓ | ✓ | ✓ |
| `Price`    | ✓ (string?) | ✓ (float?) | ✓ (string?) |
| `Currency` | ✓ | ✓ | ✓ |
| `City`     | ✓ | ✓ | ✓ |

Fields only available after API fetch: `Description`, `Region`, `PublishedAt`, `LastModeratorComment`.

## Goals / Non-Goals

**Goals:**

- Show title, price, and city immediately when the detail page opens, using data passed from the source screen
- Display skeleton placeholders for description and published date while the API loads
- Keep the transition seamless — no visual jump when API data replaces placeholders
- Make the skeleton placeholder pattern reusable for future pages

**Non-Goals:**

- Caching listing data on-device (future concern)
- Shimmer animation on skeleton elements (can be added later; static placeholders are sufficient for now)
- Changing the API contract or adding a lightweight detail endpoint
- Preloading detail data before the user taps

## Decisions

### 1. Pass preview data via Shell query parameters

Pass `title`, `price`, `currency`, and `city` as additional query parameters alongside `id`.

**Why**: This is the simplest approach given the existing navigation pattern. All three source view models already have access to these fields. Shell query params are strings, which matches the data types. URL encoding handles special characters.

**Alternative considered**: Use `WeakReferenceMessenger` to send a preview DTO before navigation. Rejected because it couples the navigation flow to messaging order, and the data is small enough for query params.

**Alternative considered**: Shared in-memory cache keyed by listing ID. Rejected as over-engineered for 4 string fields. Adds a service dependency and cache lifetime management for minimal benefit.

### 2. Dual-source view model properties

The view model will expose preview properties (`PreviewTitle`, `PreviewPrice`, `PreviewCurrency`, `PreviewCity`) populated from query params in `ApplyQueryAttributes`. The existing `FormattedPrice` and `FormattedLocation` computed properties will read from `Listing` when available, falling back to preview fields.

**Why**: This avoids constructing a partial `ListingDetail` record (which has required fields like `Status`, `CategoryId`) and keeps the preview vs. loaded-data distinction clear.

**Alternative considered**: Construct a partial `ListingDetail` from query params with placeholder values for missing fields. Rejected because it conflates "partially known" with "fully loaded" and could mask missing data bugs.

### 3. Static skeleton placeholders using BoxView

Create a reusable `SkeletonBox` control — a rounded `BoxView` with a muted background color that matches the theme. Use it in place of description and published-date labels while `IsLoading` is true.

**Why**: Simple, no external dependencies, theme-aware via `AppThemeBinding`. Shimmer animation can be layered on later without changing the control API.

**Alternative considered**: Use `Border` with `StrokeShape` for rounded placeholders. `BoxView` with `CornerRadius` is simpler and sufficient.

### 4. Layout restructuring — always-visible header, conditional body

Split the detail page into two zones:
- **Header zone** (title, price, city, favorite button): always visible once query params arrive
- **Body zone** (description, date): shows skeleton placeholders while loading, real content when loaded

The `ScrollView` wraps both zones. The `ActivityIndicator` overlay is removed entirely.

**Why**: The current pattern hides all content behind `IsLoading`. The new pattern shows the header immediately and only placeholders for the body. This eliminates the blank screen.

### 5. Price normalization for SearchResultItem

`SearchResultItem.Price` is `float?` while the other models use `string?`. The navigation source (SearchViewModel) will convert `float?` to string before passing: `item.Price?.ToString(CultureInfo.InvariantCulture)`.

**Why**: Keeps the detail page agnostic to source model types — it always receives strings via query params.

## Risks / Trade-offs

**[Risk]** Query parameter URL length with long titles → **Mitigation**: Shell handles URL encoding internally; MAUI query params don't have practical length limits since they're in-process, not over HTTP.

**[Risk]** Preview data could be stale if the listing was modified between feed load and detail tap → **Mitigation**: The API response always overwrites preview data, so staleness is limited to the 1–5 second loading window. Acceptable for title/price/city.

**[Risk]** Visual jump when API data replaces preview data (e.g., title slightly different due to edit) → **Mitigation**: Extremely unlikely in the short loading window. If it happens, the jump is small and expected — the user just tapped this item and knows what they expect to see.

**[Trade-off]** Passing 4 extra query params increases navigation call verbosity in 3 view models → Acceptable; the params are simple strings and the pattern is standard Shell navigation.
