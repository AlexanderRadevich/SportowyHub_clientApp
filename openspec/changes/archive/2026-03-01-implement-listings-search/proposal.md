## Why

The app currently has no way to browse or view sports equipment listings — the core value proposition of SportowyHub. The backend already exposes three public endpoints (`/api/v1/listings`, `/api/v1/listings/{id}`, `/api/v1/search`) that return published listings from PostgreSQL and Elasticsearch. Wiring these into the client unlocks the primary user flow: browse → search → view detail.

## What Changes

- Add C# record models for listing list responses, listing detail responses, and search responses (including pagination metadata, geo-location, and dynamic attributes)
- Register all new models in `SportowyHubJsonContext` for source-generated JSON serialization
- Create `IListingsService` / `ListingsService` with methods for fetching listings (paginated), fetching a single listing by ID or slug, and searching with filters (query, category, sport, city, price range, sort)
- Build a listings feed on the Home page showing published listings with pull-to-refresh and incremental loading
- Build a Listing Detail page navigable from the feed or search results, showing full listing info (title, description, price, city, region, category, published date)
- Integrate the search API into the existing Search page, replacing hardcoded placeholder data with real Elasticsearch-powered results and filter support

## Capabilities

### New Capabilities
- `listings-api-models`: C# records for listings list, listing detail, and search API responses; pagination metadata; `SportowyHubJsonContext` registration
- `listings-service`: `IListingsService` with `GetListingsAsync`, `GetListingAsync`, `SearchAsync` methods calling the three public endpoints via `IRequestProvider`
- `listings-feed`: Home page listings feed with `CollectionView`, pull-to-refresh, incremental loading (offset pagination), and navigation to detail
- `listing-detail`: Listing detail page and view model receiving listing ID/slug via query parameter, showing full listing information

### Modified Capabilities
- `search-ui`: Replace hardcoded recent/popular placeholder data with real search API integration — search input triggers `/api/v1/search` with debounce, results displayed in a `CollectionView`, tapping a result navigates to listing detail

## Impact

- **Models**: New files in `Models/Api/` (5-8 records), updated `SportowyHubJsonContext`
- **Services**: New `Services/Listings/IListingsService.cs` + `ListingsService.cs`, registered in `MauiProgram.cs`
- **ViewModels**: New `HomeViewModel`, `ListingDetailViewModel`; modified `SearchViewModel`
- **Pages**: New `ListingDetailPage.xaml/.cs`; modified `HomePage.xaml/.cs`, `SearchPage.xaml/.cs`
- **Navigation**: New Shell route `listing-detail` registered in `AppShell.xaml.cs`
- **Dependencies**: No new NuGet packages required — uses existing `IRequestProvider`, `INavigationService`, `IToastService`
