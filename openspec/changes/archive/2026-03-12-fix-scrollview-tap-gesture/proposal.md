# Fix ScrollView Tap Gesture

## Why

`SearchPage.xaml` and `HomePage.xaml` use `Label` + `TapGestureRecognizer` inside `ScrollView` for tappable text links. This is a known MAUI bug where tap detection is unreliable inside ScrollView, causing missed taps.

## What Changes

Replace `Label`+`TapGestureRecognizer` with `Button` styled with a transparent background. Add a reusable `LinkButton` style to `Styles.xaml`.

## Capabilities

### New
- `LinkButton` implicit or explicit style in `Styles.xaml`

### Modified
- `SearchPage.xaml` — replace Label+TapGestureRecognizer with styled Button
- `HomePage.xaml` — replace Label+TapGestureRecognizer with styled Button

## Impact

- **UX** — reliable tap detection for "Clear recent" and similar links inside scrollable areas
- **Consistency** — reusable LinkButton style for future tappable text elements
