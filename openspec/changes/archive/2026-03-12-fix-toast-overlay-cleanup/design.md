## Context

ToastService shows overlay toasts by wrapping the current page's `Content` in a Grid (if it isn't already a Grid or Layout). When the toast is removed, the overlay is taken out of the Grid, but the original content is not unwrapped — it stays inside the wrapper Grid.

## Goals / Non-Goals

**Goals:**
- Store a reference to the original `Content` before wrapping
- Restore the original `Content` in `RemoveOverlayFromPage`
- Handle edge cases: overlay already removed, rapid show/hide cycles

**Non-Goals:**
- Redesigning the toast overlay mechanism
- Switching to CommunityToolkit.Maui popups

## Decisions

- Store the original content reference in a dictionary keyed by page (or a field if single-page)
- In `RemoveOverlayFromPage`, after removing the overlay, restore `page.Content` to the stored reference
- Clear the stored reference after restoration to avoid memory leaks
- If the wrapper Grid is already gone (edge case), skip restoration gracefully

## Risks / Trade-offs

- Must handle thread safety if toasts can be shown/removed from different threads
- Rapid show/hide may cause race conditions — consider a lock or MainThread dispatch
- The stored reference must use a weak pattern or be cleaned up to avoid holding pages in memory
