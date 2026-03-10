## Context

Active filter chips on the search screen use an `ImageButton` (14×14) with `icon_clear.svg` as the close/remove button. The button feels visually heavy relative to the chip's compact layout (13px font, 8,4 padding).

## Goals / Non-Goals

**Goals:**
- Reduce the close button to 10×10 for better visual proportion with the chip label
- Maintain adequate touch target through the chip's existing padding

**Non-Goals:**
- Changing chip padding, font size, or overall chip dimensions
- Adding a dedicated tap target enlargement (the chip border padding already provides sufficient touch area)

## Decisions

- **10×10 size**: Proportional to the 13px label font. Smaller than 10 would be hard to see; larger than 12 wouldn't meaningfully improve the current 14×14.
- **Reduce inner spacing from 6 to 4**: The 6px gap between label and icon was balanced for 14×14. At 10×10, 4px maintains the same visual rhythm.
- **XAML-only change**: No new styles, no code-behind, no custom control needed. Inline attribute edits in the existing `DataTemplate`.

## Risks / Trade-offs

- [Touch target slightly smaller] → Mitigated by the chip `Border` padding (8,4) which extends the tappable area beyond the icon itself
