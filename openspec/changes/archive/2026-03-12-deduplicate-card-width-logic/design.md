## Context

Both HomePage and SearchPage display listing cards in a grid layout and compute card widths based on screen width, spacing, and column count. The logic is copy-pasted between the two code-behind files.

## Goals / Non-Goals

**Goals:**
- Extract card-width calculation to a single shared location
- Keep the same visual output on all screen sizes

**Non-Goals:**
- Changing the card layout or column count logic
- Migrating to CollectionView (that is C2's scope)

## Decisions

- Create a static helper class (e.g., `CardLayoutHelper`) in the `Controls/` or `Helpers/` folder
- The helper takes screen width, spacing, and column count as parameters and returns card width
- Both pages call the helper instead of computing inline
- If C2 (virtualize-listing-grids) lands first, this change becomes unnecessary

## Risks / Trade-offs

- Low risk if the extracted logic is a pure function with no side effects
- Must test on narrow (phone) and wide (tablet/desktop) screen sizes to verify no rounding differences
