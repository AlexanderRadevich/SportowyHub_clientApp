## Why

The search bar icons (back arrow, clear/X) on the Search page are black SVGs with no theme-aware tint. In dark mode, they are nearly invisible against the dark `#1E1E1E` search bar background. The bottom tab bar icons already adapt their color to the active theme — the search bar icons should behave the same way.

## What Changes

- Add `AppThemeBinding` tint colors to the back and clear `ImageButton` icons in the search bar on `SearchPage.xaml`
- Also apply the same fix to the Home page search bar magnifying glass icon if it has the same problem

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `search-ui`: The dark theme styling scenario for the search bar needs to specify icon color alongside background and text color.

## Impact

- **Views/Search/SearchPage.xaml** — add tint color bindings to `ImageButton` icons
- **Views/Home/HomePage.xaml** — add tint color binding to search bar icon if affected
- **openspec/specs/search-ui/spec.md** — update dark theme scenario to include icon color requirement
