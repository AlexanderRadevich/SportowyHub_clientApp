## Context

All SVG icons are invisible on iOS — both Shell tab bar icons and Image controls in pages. The app has 24 SVG icon files in `Resources/Images/`. Of these, 13 use `stroke="currentColor"` and 11 use explicit colors (`#000000` or `#FFFFFF`). From the iOS simulator screenshots, even icons with explicit `#000000` strokes (cart, theme toggle) are invisible, indicating the problem extends beyond `currentColor`.

MAUI's build pipeline converts SVGs to PNGs at build time via SkiaSharp. On iOS, these PNGs are placed into asset catalogs and resolved at runtime.

## Goals / Non-Goals

**Goals:**
- Make all icons visible on iOS in both light and dark themes
- Maintain existing icon appearance on Android, Windows, and macOS

**Non-Goals:**
- Redesigning the icon system (e.g., switching to font icons)
- Adding new icons or changing icon visual design

## Decisions

**Decision 1: Replace `stroke="currentColor"` with `#000000` in all light-mode SVGs**

`currentColor` is a CSS concept that SkiaSharp's SVG renderer does not reliably support. Replacing it with an explicit black color ensures the SVG-to-PNG conversion produces visible strokes.

**Decision 2: Keep `.svg` extension in XAML references**

The existing `Source="icon_home.svg"` pattern is the standard MAUI convention. MAUI's build pipeline handles extension stripping.

## Risks / Trade-offs

- [Risk] Icons with explicit `#000000` are also invisible → Mitigation: If fixing `currentColor` alone doesn't resolve, investigate SVG structure and test with simplified SVGs
- [Risk] Hardcoded black strokes lose tintability → Mitigation: Tab bar icons are tinted by Shell; header icons use `AppThemeBinding` with separate light/dark SVG variants; `IconTintColorBehavior` overrides source colors where used
