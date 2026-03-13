## Why

`ToastService.cs:142-145` wraps page content in a new `Grid` when the content is neither Grid nor Layout. `RemoveOverlayFromPage` removes the toast overlay but never restores the original `Content`. The page is left with an orphaned wrapper Grid instead of its original content structure.

## What Changes

Track the original content reference before wrapping, and restore it when removing the overlay.

## Capabilities

### Modified

- ToastService correctly restores original page content after overlay removal

## Impact

- Fixes a subtle bug where pages may retain an unexpected wrapper Grid after toast dismissal
- Prevents potential layout issues on pages that rely on their specific content structure
- Medium risk — must handle edge cases (overlay already removed, rapid show/hide)
