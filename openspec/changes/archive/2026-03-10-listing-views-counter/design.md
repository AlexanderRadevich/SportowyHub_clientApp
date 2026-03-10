## Context

The backend stores `view_count` on the `listings` table (unsigned int, default 0, incremented per non-owner page view) but the public JSON API does not serialize it. The MAUI client uses `ListingSummary` (cards/feed), `ListingDetail` (detail page), and `SearchResultItem` (search results) — none currently have a view count property. Icons are SVG-based with theme-aware tinting via `IconTintColorBehavior`. JSON deserialization uses `SportowyHubJsonContext` with `SnakeCaseLower` naming policy.

## Goals / Non-Goals

**Goals:**
- Display view count on listing cards and the listing detail page
- Format large numbers for readability (e.g., 1 234 → "1.2k")
- Support light and dark themes

**Non-Goals:**
- Tracking/incrementing views from the MAUI client (backend handles this)
- Sorting or filtering by view count
- Real-time view count updates (stale counts from API response are acceptable)
- Backend API changes (coordinated separately; client will gracefully handle missing field)

## Decisions

### 1. Add `ViewCount` property to all listing models

Add `int ViewCount` to `ListingSummary`, `ListingDetail`, `MyListingSummary`, and `SearchResultItem`. The `SnakeCaseLower` JSON policy automatically maps `view_count` → `ViewCount`. Default value is `0` so the field is safe if the backend hasn't deployed the API change yet.

**Alternative considered:** A separate "enrichment" DTO wrapping listing models. Rejected — adds unnecessary complexity for a single field.

### 2. Use an eye icon (`icon_eye.svg`) for the view count indicator

The codebase already has `icon_eye.svg` (currently used by `BoolToEyeIconConverter` for password visibility). Reuse it as the views indicator on cards and detail page. The icon uses `currentColor` fill, so it works with `IconTintColorBehavior` for theme support.

**Alternative considered:** A text-only display ("123 views"). Rejected — the icon makes it instantly recognizable and consistent with other card metadata (heart icon for favorites).

### 3. Format view counts with a `ViewCountFormatConverter`

Create an `IValueConverter` that formats:
- `0–999` → displayed as-is (e.g., `"42"`)
- `1 000–999 999` → `"1.2k"` format
- `1 000 000+` → `"1.2M"` format

Use `CultureInfo.InvariantCulture` for the decimal separator to keep display consistent across locales.

**Alternative considered:** Display raw numbers everywhere. Rejected — `"12 847"` takes too much horizontal space on cards. Formatted numbers are compact and scannable.

### 4. Card layout: view count below the price, aligned left

Add a small row below the price in `ListingCardView` with `icon_eye.svg` (12×12) + formatted count in secondary text style (`FontSize="11"`, `TextSecondary` color). This mirrors how the favorite heart icon sits in the image area — metadata is visually grouped.

**Alternative considered:** Place views in the image overlay area (like the condition badge). Rejected — the image area is already dense with the condition badge and favorite button.

### 5. Detail page: view count in the info section

Add the view count with the eye icon below the location line, before the divider. Uses the same `ViewCountFormatConverter` but with full-size styling (`FontSize="14"`).

### 6. Graceful degradation when backend hasn't deployed

`ViewCount` defaults to `0`. When `0`, hide the view count indicator entirely (both on cards and detail). This means no UI change until the backend adds `view_count` to API responses.

## Risks / Trade-offs

**[Risk] Backend API not yet returning `view_count`** → Default to `0` and hide the indicator. No client-side errors. The feature activates automatically once the backend deploys.

**[Risk] View counts may be stale** → Acceptable. Counts refresh on next API fetch. No WebSocket or polling needed for a popularity indicator.

**[Trade-off] Reusing `icon_eye.svg` from password toggle** → The icon semantically fits both use cases. If a distinct icon is needed later, it can be swapped without layout changes.
