## Context

The search bar icons (`icon_back.svg`, `icon_clear.svg` on SearchPage, `icon_search_bar.svg` on HomePage) are SVGs rendered without a tint color. In dark theme the icons remain black and are invisible against the `#1E1E1E` background.

MAUI `Image` and `ImageButton` don't have a built-in `TintColor` property. The standard solution is `IconTintColorBehavior` from `CommunityToolkit.Maui`.

## Goals / Non-Goals

**Goals:**
- Make search bar icons visible in both light and dark themes using theme-aware tint colors

**Non-Goals:**
- Changing icon SVG files or creating dark variants
- Tinting icons elsewhere in the app (only search bar affected)

## Decisions

### 1. Use CommunityToolkit.Maui IconTintColorBehavior

**Decision:** Add `CommunityToolkit.Maui` NuGet package and use `IconTintColorBehavior` with `AppThemeBinding` on all search bar icons.

**Rationale:** This is the standard, recommended approach for icon tinting in MAUI. The project already uses `CommunityToolkit.Mvvm`, so the Maui toolkit is a natural companion. One-liner per icon with `AppThemeBinding` for theme-aware colors.

**Alternative considered:** Swapping between light/dark SVG variants via `AppThemeBinding` on `Source`. Rejected — doubles the icon assets and doesn't scale.

### 2. Icon color values

**Decision:** Use `Secondary` (#39485F) in light theme (matches existing search bar text color) and `White` (#FFFFFF) in dark theme (matches tab bar icon behavior).

**Rationale:** Consistent with existing theming — the search bar spec already defines text as `#39485F` light / `#FFFFFF` dark, icons should match.

## Risks / Trade-offs

**[New NuGet dependency]** → `CommunityToolkit.Maui` is widely used and maintained by Microsoft. Minimal risk, adds useful utilities beyond just this fix.
