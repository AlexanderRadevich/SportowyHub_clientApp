## Why

`HomePage.xaml.cs:54-81` and `SearchPage.xaml.cs:34-90` contain identical card-width calculation logic. Any bug fix or adjustment must be applied in both places, creating a maintenance risk.

**Note:** This may be superseded by C2 (virtualize-listing-grids). If C2 is implemented first, this change can be skipped.

## What Changes

Extract the duplicated card-width calculation into a shared behavior or static helper method. Both pages will reference the shared implementation.

## Capabilities

### New

- Shared CardWidthHelper or CardWidthBehavior for reusable card-width calculation

### Modified

- HomePage.xaml.cs uses shared helper instead of inline logic
- SearchPage.xaml.cs uses shared helper instead of inline logic

## Impact

- Eliminates duplicated code and single-point-of-change maintenance risk
- No visual or behavioral change to users
- Medium risk — must verify card widths render identically after refactor on all screen sizes
