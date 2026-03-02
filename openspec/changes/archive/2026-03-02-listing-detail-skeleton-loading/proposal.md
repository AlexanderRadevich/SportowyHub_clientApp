## Why

When a user taps a listing from the feed, search results, or favorites, the detail page shows a blank screen with a spinner for 1–5 seconds while the API responds. The source screens already have key data (title, price, city) that could be displayed immediately, making the transition feel instant and the load time less noticeable.

## What Changes

- Pass known listing data (title, price, currency, city) through navigation parameters so the detail page can render them immediately on arrival
- Replace the full-screen `ActivityIndicator` with a progressive loading pattern: show pre-populated fields right away, display shimmer/placeholder elements for fields still loading (description, region, published date)
- Once the API response arrives, swap placeholders with real content seamlessly

## Capabilities

### New Capabilities

- `skeleton-loading`: Reusable skeleton/shimmer placeholder controls and patterns for indicating loading content across the app

### Modified Capabilities

- `listing-detail`: Change from all-or-nothing loading (spinner → full content) to progressive loading (instant header with known data + placeholders → full content on API response)

## Impact

- **ViewModels**: `ListingDetailViewModel` gains pre-populated properties set from navigation params; `HomeViewModel`, `SearchViewModel`, `FavoritesViewModel` pass additional data during navigation
- **Views**: `ListingDetailPage.xaml` restructured to show partial content immediately with placeholder elements for loading fields
- **Navigation**: Query parameters expand from `?id={id}` to include `title`, `price`, `currency`, `city`
- **New controls**: Skeleton/shimmer placeholder controls added under `Controls/`
- **No API changes**: The same `GET /api/v1/listings/{id}` call is used; this is purely a client-side UX improvement
