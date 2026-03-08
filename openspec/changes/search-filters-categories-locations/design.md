## Context

The Search page currently supports only free-text queries with debounce. `IListingsService.SearchAsync` already accepts `categoryId`, `sport`, `cityId`, `voivodeshipId`, `priceMin`, `priceMax`, `sort`, and `condition` parameters, but the `SearchViewModel` never passes them. `ISectionsService` and `IGeographyService` are registered singletons with working endpoints. The Home page shows a flat listing feed with no category browsing.

## Goals / Non-Goals

**Goals:**
- Add a filter panel to the Search page that passes all supported filter parameters to `SearchAsync`
- Add horizontal sport-section cards on the Home page that navigate to pre-filtered search
- Keep filter state in the ViewModel so it survives back-navigation within the same session
- Reuse existing services (`ISectionsService`, `IGeographyService`) — no new API calls needed

**Non-Goals:**
- Saved/persistent filter presets across sessions
- Full-screen filter page (keep it lightweight as a bottom sheet)
- Map-based location browsing
- Adding the `GET /api/v1/listings/statuses` endpoint (not useful for end-user search filtering)
- Changing the backend search API or adding new endpoints

## Decisions

### 1. Filter UI as Bottom Sheet (CommunityToolkit.Maui Popup)

Use a `Popup` from CommunityToolkit.Maui displayed as a bottom sheet for the filter panel. The filter button sits in the search bar row.

**Why not inline expanding panel:** Takes too much vertical space on the search page, pushes results down, awkward with keyboard open.
**Why not a separate page:** Over-engineered for 5-6 filter controls; loses search context.
**Why Popup:** Already have CommunityToolkit.Maui in the project; lightweight, doesn't break navigation stack, can return filter state on dismiss.

### 2. Filter State as ObservableObject in SearchViewModel

Create a `SearchFilterState` class (not a separate ViewModel) holding all filter properties. `SearchViewModel` owns it directly. When filters change and the user taps "Apply", the search re-executes with the new parameters.

**Why not a separate service:** Filter state is transient and page-scoped — no need for a singleton. The ViewModel is already transient.
**Why ObservableObject:** Enables two-way binding in the filter popup without manual property change plumbing.

### 3. Cascading Sport → Category Picker

The filter panel shows sports (sections) as a picker/list. Selecting a sport loads its categories via `ISectionsService.GetCategoriesAsync`. Category picker is disabled until a sport is selected.

**Why cascade:** Categories belong to sports in the backend data model. Showing all categories flat would be a huge unstructured list.

### 4. Location Filter via Geography Autocomplete

Reuse the existing `IGeographyService.AutocompleteAsync` with an inline `Entry` in the filter panel. When user selects a result, store the `voivodeshipId` and optional `cityId` for the search query. Display the selected label as a chip/tag.

**Why autocomplete over dual pickers:** The autocomplete endpoint already handles both voivodeships and cities in one search, matching the UX pattern users expect. Two separate pickers (voivodeship then city) adds friction.

### 5. Home Category Browse as Horizontal ScrollView

Add a horizontal `CollectionView` of sport-section cards between the search bar and the listings feed on the Home page. Tapping a card navigates to the Search tab with that sport pre-selected as a filter.

**Why horizontal scroll:** Compact, doesn't push the feed down much (~80px). Familiar pattern (app stores, e-commerce apps).
**Why navigate to Search tab:** Reuses the existing search infrastructure instead of building a parallel filtered-listings view.

### 6. Filter-to-Search Navigation Contract

When navigating from Home category cards to Search, pass the sport slug as a Shell query parameter. `SearchViewModel` reads it in `ApplyQueryAttributes` and pre-populates the sport filter, then auto-executes a search.

## Risks / Trade-offs

**[Risk] Geography autocomplete requires Elasticsearch on backend** → The autocomplete endpoint already works in production. If ES is down, the location filter gracefully returns empty results (existing `GeographyService` behavior).

**[Risk] Filter popup may not render well on small screens** → Keep the filter layout scrollable inside the popup. Test on smallest target (5" Android).

**[Trade-off] No saved filters** → Filters reset when leaving the search page. This is acceptable for v1; saved filters can be added later without architectural changes.

**[Trade-off] Sections loaded on demand, not cached** → `ISectionsService.GetSectionsAsync` is called each time the filter opens and each time the Home page loads. The response is small (~10 items) and the endpoint is fast. Add caching later if needed.

## Open Questions

- Should the filter button show an active-filter count badge, or just change color when filters are active? (Recommend: count badge — more informative)
- Should "condition" (new/used) be a toggle, segmented control, or chips? (Recommend: two chips — matches mobile patterns)
