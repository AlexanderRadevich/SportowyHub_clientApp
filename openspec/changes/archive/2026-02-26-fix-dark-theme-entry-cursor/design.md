## Context

The implicit `Entry` style in `Styles.xaml` sets `TextColor`, `BackgroundColor`, `PlaceholderColor`, and `FontFamily` with `AppThemeBinding` for light/dark support. However, no `CursorColor` is set, so the cursor uses the platform default (dark) which is invisible against the dark surface background (`#1E1E1E`).

## Goals / Non-Goals

**Goals:**
- Make the Entry cursor visible in both light and dark themes

**Non-Goals:**
- Changing cursor color on Editor or other input controls (Entry only for now)
- Custom handler or platform-specific code

## Decisions

### Use `CursorColor` with `AppThemeBinding` on the implicit Entry style

Add a single `CursorColor` setter using `Primary` (light) / `PrimaryDark` (dark) to match the brand accent. This uses the built-in MAUI `CursorColor` property available since .NET 9 on `InputView`.

**Alternative considered:** Platform handler to set Android `textCursorDrawable`. Rejected — unnecessary complexity when the built-in property exists.

**Alternative considered:** Using `TextPrimary`/`TextPrimaryDark` for the cursor. Rejected — brand accent (`Primary`/`PrimaryDark`) is more visible and consistent with typical cursor styling.

## Risks / Trade-offs

- **Minimal risk** — single XAML setter addition, no behavioral change, no code modifications
