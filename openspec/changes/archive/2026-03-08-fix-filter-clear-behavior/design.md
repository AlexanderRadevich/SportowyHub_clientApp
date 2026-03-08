## Context

The search page supports filter chips (sport, category, location, price, condition, sort) displayed below the search bar. Two flows have broken UX:

1. **Remove last chip**: `SearchViewModel.RemoveFilter` clears the filter and calls `ExecuteSearch`, but `OnSearchTextChanged` fires when `SearchText` is empty and `HasActiveFilters` becomes false — clearing results and focusing the search bar instead of showing unfiltered listings.
2. **Clear Filters button in popup**: `SearchFilterPopupViewModel.Reset` only resets the local popup state. It does not fire the `Applied` event, so the popup stays open and results are not refreshed.

## Goals / Non-Goals

**Goals:**
- Removing the last filter chip shows all listings (unfiltered search)
- "Clear Filters" in popup resets state, closes popup, and refreshes results
- Both flows produce identical end state: empty filters + fresh unfiltered results

**Non-Goals:**
- Changing the debounce/search logic beyond this fix
- Modifying how filters are applied from the popup's "Apply" button (already works)

## Decisions

**Decision 1: Make Reset invoke the Applied event with empty state**

The `Reset` command in `SearchFilterPopupViewModel` will call `Reset()` to clear local state, then invoke `Applied` with a default `SearchFilterState`. This reuses the existing `Applied` event → close popup → apply state → refresh search flow.

Alternative: Add a separate `Cleared` event. Rejected — adds complexity for identical behavior to `Applied` with empty state.

**Decision 2: Prevent OnSearchTextChanged from clearing results when transitioning from filtered to unfiltered**

The issue is that `OnSearchTextChanged` checks `string.IsNullOrWhiteSpace(value) && !HasActiveFilters` — but by the time this fires, `HasActiveFilters` is already false because `UpdateFilterCount()` ran first. The fix: after removing a filter and `HasActiveFilters` becomes false, `ExecuteSearch` still runs (it already does). The real problem is that `OnSearchTextChanged` races with the removal flow. The simplest fix is to not early-return when search text is empty — instead, always execute the search. But this would fire on every keystroke clearing.

Better approach: in `RemoveFilter`, after `UpdateFilterCount()`, if `HasActiveFilters` is false and `SearchText` is empty, still call `ExecuteSearch` with empty query. This already happens — the actual issue is that `OnSearchTextChanged` doesn't fire at all here. The `RemoveFilter` already calls `ExecuteSearch`. The problem is only if `SearchText` setter triggers side effects. Since `RemoveFilter` doesn't modify `SearchText`, the flow is: clear filter → `UpdateFilterCount` (sets `HasActiveFilters=false`) → `ExecuteSearch(SearchText, 0, ...)` which should work. Need to verify the actual race condition by checking if `ExecuteSearch` with empty query and no filters returns all items or short-circuits.

Looking at `ExecuteSearch`: it passes `query: string.IsNullOrWhiteSpace(query) ? null : query` — so empty text becomes `null` query, which the API treats as "no text filter". This should return all items. The search should work correctly already.

The actual bug is likely in `OnSearchTextChanged` — when `SearchText` is empty and `!HasActiveFilters`, it clears results. But this only fires when `SearchText` changes. Since `RemoveFilter` doesn't change `SearchText`, this shouldn't be an issue. The real culprit may be that the search page focuses the entry on appearing, and the focus behavior clears/resets something. Need to verify in `SearchPage.xaml.cs` — `OnAppearing` calls `SearchEntry.Focus()`. This fires every time the page appears, but after `RemoveFilter` the page doesn't re-appear — the user is already on it.

Simplest fix: in `RemoveFilter`, the `ExecuteSearch` call already handles showing all items. If the user sees empty results, the issue is likely that `ExecuteSearch` with empty query and no filters short-circuits somewhere. Verify and fix that path.

## Risks / Trade-offs

- [Risk] `Reset` calling `Applied` with empty state may surprise callers expecting `Applied` to mean "user explicitly chose filters" → Mitigation: This is the intended behavior — "clear all" is equivalent to "apply no filters". The popup close/apply flow handles both identically.
- [Risk] Searching with no text and no filters could return large result sets → Mitigation: Already paginated with `PageSize = 30` and lazy loading via `LoadMoreSearchResults`.
