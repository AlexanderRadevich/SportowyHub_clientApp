## Context

Active filter chips on the search page currently use a single `Border` containing a `HorizontalStackLayout` with label + 10×10 close icon. The entire chip is one tap target — there is no separation between the label area and the remove action. The close icon is small and hard to tap accurately on mobile devices.

The redesign introduces a split-pill layout where the label zone and remove zone are visually and functionally separated, each with its own tap target.

## Goals / Non-Goals

**Goals:**
- Split the chip into two distinct zones: label (left) and remove (right)
- Add a colored dot indicator before the label text
- Provide a comfortable tap target for the remove zone (full right section, not just the icon)
- Maintain theme support (light/dark) using existing color resources
- Keep the same data binding contract (`ActiveFilterChip` model, `RemoveFilterCommand`)

**Non-Goals:**
- Changing the `ActiveFilterChip` model or adding new properties
- Modifying ViewModel logic or command signatures
- Redesigning home page condition chips (separate concern)
- Adding chip animations or transitions

## Decisions

### 1. Two-zone layout using Grid columns inside a single Border

Use a `Grid` with two columns inside the existing `Border` wrapper. The left column holds the dot + label, the right column holds the "×" icon. A `BoxView` acts as a vertical divider between zones.

**Why not two separate Borders?** A single `Border` with `CornerRadius` gives the unified pill shape. Two borders would require matching left/right corner radii and aligning them perfectly, which is fragile across platforms.

**Why Grid over HorizontalStackLayout?** Grid allows precise column sizing — `*` for the label zone (flexible) and `Auto` for the remove zone (fixed width). This ensures the remove zone has a consistent, predictable tap target width.

### 2. Separate TapGestureRecognizer on the remove zone only

The current chip has one `TapGestureRecognizer` on the entire `HorizontalStackLayout`. The new design places the gesture recognizer only on the remove zone column content. The label zone has no tap action (tapping a filter label does nothing useful).

### 3. Colored dot as a small Ellipse

Use a `Border` with `StrokeShape="RoundRectangle"` and a fixed size (6×6) filled with the accent color to create the dot indicator. This avoids needing an additional SVG asset and uses the same theming approach as the rest of the chip.

### 4. Vertical divider as a BoxView

A `BoxView` with `WidthRequest="1"` and themed color provides the subtle vertical separator. This is lightweight and straightforward.

### 5. Remove zone sizing

The remove "×" zone should be at least 44×full-height logical pixels to meet accessibility tap target guidelines. The icon itself stays small (12×12) but the tappable area fills the entire right column with padding.

## Risks / Trade-offs

- **[Tap target overlap on small chips]** → Short labels (e.g., "New") may result in a chip where the remove zone is disproportionately large. Acceptable — consistent remove zone size is more important than visual proportion.
- **[Platform rendering differences]** → `BoxView` divider rendering may vary slightly across Android/iOS/Windows. Mitigated by using a 1px width with explicit color — minimal cross-platform risk.
- **[ScrollView clipping]** → The chip row is in a horizontal `ScrollView`. The slightly larger chip size (due to split layout) may show fewer chips before scrolling. Acceptable trade-off for better usability.
