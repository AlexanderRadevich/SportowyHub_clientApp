## Context

The Search page currently shows active filters only as a badge count on the filter icon. Users must reopen the full filter popup to see which filters are applied or to remove one. When navigating from the Home page sport categories, the filter is applied silently with no visual feedback.

Existing components:
- `SearchFilterState` — ObservableObject with all filter properties and `ActiveFilterCount`
- `SearchViewModel` — owns `FilterState`, `ActiveFilterCount`, `HasActiveFilters`, and `UpdateFilterCount()`
- `SearchPage.xaml` — 3-row Grid (search bar, activity indicator, content area)

## Goals / Non-Goals

**Goals:**
- Show each active filter as a dismissible chip below the search bar
- Allow removing individual filters by tapping the chip's close button
- Re-execute search automatically when a filter is removed
- Horizontally scrollable chip row when filters overflow

**Non-Goals:**
- Tapping a chip to edit that specific filter (opens full popup instead)
- Reordering or grouping chips by filter type
- Animated chip add/remove transitions

## Decisions

### 1. Chip data model: `ActiveFilterChip` record

Introduce a simple `record ActiveFilterChip(string Key, string Label)` where `Key` identifies which filter property to clear (e.g., `"sport"`, `"category"`, `"location"`, `"priceMin"`, `"priceMax"`, `"condition"`, `"sort"`) and `Label` is the user-visible localized text (e.g., "Hockey", "New", "Price ↑").

**Why record over full ViewModel per chip:** Chips are read-only display items rebuilt from `SearchFilterState` on every filter change. No mutable state needed.

### 2. Chip collection lives in `SearchViewModel`

Add `ObservableCollection<ActiveFilterChip> ActiveFilterChips` to `SearchViewModel`. Rebuild it in `UpdateFilterCount()` which already runs after every filter change. This keeps all filter logic centralized.

**Alternative considered:** Deriving chips from `SearchFilterState` directly via computed property — rejected because `ObservableCollection` change notifications are needed for the CollectionView binding.

### 3. Remove individual filter via `RemoveFilterCommand(string key)`

A single `RelayCommand<string>` that switches on the key to null out the correct `FilterState` property, then calls `UpdateFilterCount()` and re-executes search. This avoids one command per filter type.

### 4. Chip labels use existing localized filter names

Chip labels combine the filter value name (e.g., section name, location label) rather than duplicating with a "type: value" prefix. The chip's visual context (position after search bar, dismissible style) makes the type obvious. For price filters, show "≥ {value}" / "≤ {value}". For sort, show the sort option label.

No new localization strings needed — reuse `FilterConditionNew`, `FilterConditionUsed`, sort option labels, and entity names from the API.

### 5. UI: horizontal ScrollView with FlexLayout

Use a `ScrollView Orientation="Horizontal"` containing a `FlexLayout Wrap="NoWrap"` of chip templates. Each chip is a `Border` with rounded corners containing a `HorizontalStackLayout` (label + close ImageButton).

**Alternative considered:** Horizontal `CollectionView` — rejected because CollectionView has known sizing issues with dynamic-width items on Android in MAUI, and the chip count is small (max ~7 items), so virtualization is unnecessary.

## Risks / Trade-offs

- [Chip row takes vertical space] → Only visible when filters are active; compact 32px height with small font
- [Rebuilding chip list on every filter change] → Max 7 chips, negligible cost; simpler than diffing
