## Context

MyListingsPage renders a list of the user's own listings. Each item template includes 4 action buttons that are permanently hidden. These were likely scaffolded for future use but never wired up, adding unnecessary overhead to the visual tree.

## Goals / Non-Goals

**Goals:**
- Determine whether status-based actions are needed now
- If yes: add DataTriggers binding to listing Status property
- If no: remove the dead buttons to reduce visual tree weight

**Non-Goals:**
- Implementing the full listing status management workflow (that would be a separate feature)

## Decisions

- Prefer removal if the backend API does not yet support status transitions
- If DataTriggers are added, bind visibility to a `Status` property on the listing model
- Each button shows only for the relevant status (e.g., Publish for draft, Hide for published)

## Risks / Trade-offs

- Removing buttons means re-adding them later when the feature is needed
- Adding DataTriggers requires the listing model to expose a Status property
- Either path is low risk for the existing functionality
