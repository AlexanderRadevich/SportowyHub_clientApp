## Why

All SVG icons are invisible on iOS — both Shell tab bar icons and Image controls in pages (header action buttons, search bar icons). The root cause is that SVG files use `stroke="currentColor"`, a CSS concept that MAUI's SkiaSharp-based SVG-to-PNG build pipeline does not support, producing transparent/invisible PNGs. Even icons with explicit `#000000` strokes appear invisible, suggesting a broader SVG rendering issue on iOS.

## What Changes

- Replace `stroke="currentColor"` with `stroke="#000000"` in all light-mode SVG icon files
- Verify SVG structure compatibility with iOS build pipeline
- Test all icon locations: Shell tab bar, home page header, search bar, sort button, password toggle

## Capabilities

### New Capabilities

_(none)_

### Modified Capabilities

_(none — this is a build-time asset fix, not a behavioral/spec change)_

## Impact

- **Files changed**: 13 SVG files in `SportowyHub.App/Resources/Images/` that use `stroke="currentColor"`
- **Platforms**: Fix targets iOS but is cross-platform safe
- **No API or dependency changes**
