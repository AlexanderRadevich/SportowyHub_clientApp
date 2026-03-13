# Design: Fix ScrollView Tap Gesture

## Context

`SearchPage.xaml` and `HomePage.xaml` use `Label` + `TapGestureRecognizer` inside `ScrollView` for tappable text like "Clear recent". This is a known MAUI bug where the ScrollView intercepts touch events, making tap detection unreliable.

## Goals / Non-Goals

### Goals
- Make tappable text links work reliably inside ScrollView
- Create a reusable LinkButton style for consistency

### Non-Goals
- Fix the underlying MAUI ScrollView gesture bug
- Replace all Label+Tap patterns across the app (only the ones inside ScrollView)

## Decisions

1. **Button with transparent background** — Replace `Label`+`TapGestureRecognizer` with `Button` styled to look like a text link. Buttons have their own touch handling that is not intercepted by ScrollView.
2. **LinkButton style** — Define a reusable style in `Styles.xaml` with transparent background, no border, and text-matching font properties. This ensures visual consistency and reuse.
3. **Command binding** — Button supports `Command` binding directly, eliminating the need for `TapGestureRecognizer` command wiring.

## Risks / Trade-offs

- **Visual differences** — Button has default padding and minimum size that may differ from Label. Must adjust `Padding="0"`, `MinimumHeightRequest`, and `MinimumWidthRequest` to match the original Label appearance.
- **Accessibility** — Button is inherently more accessible than Label+Tap (proper role announcement). This is a positive side effect.
