## Why

Zero pages use `SemanticProperties.Description` or `SemanticProperties.Hint` on interactive elements. Heart buttons, filter chips, icon buttons, and the theme toggle are all invisible to screen readers. This is a basic accessibility gap.

## What Changes

Add `SemanticProperties` to all interactive elements across all pages, prioritizing icon-only buttons (no visible text), toggle controls, and custom tap areas.

## Capabilities

### New

- Screen reader support for all interactive elements across the app

### Modified

- ListingCardView heart button gets semantic description
- HomePage header icon buttons get semantic descriptions
- SearchPage filter chips and interactive elements get semantic hints
- FavoritesPage, auth pages, profile pages, MyListingsPage all annotated

## Impact

- Significant accessibility improvement for visually impaired users
- No visual or behavioral changes for sighted users
- Medium effort — touches many XAML files but changes are mechanical (adding attributes)
