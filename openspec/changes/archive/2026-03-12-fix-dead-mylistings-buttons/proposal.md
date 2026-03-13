## Why

`MyListingsPage.xaml:124-150` defines 4 action buttons (Publish, Hide, Resubmit, Delete) that are all `IsVisible="False"` with no DataTriggers or bindings to ever show them. These dead controls inflate the visual tree for every list item, wasting memory and layout cycles.

## What Changes

Either add DataTriggers based on listing status to conditionally show the correct buttons, or remove them entirely if the feature is not yet needed.

## Capabilities

### Modified

- MyListingsPage action buttons are either functional (with status-based visibility) or removed

## Impact

- Reduces visual tree weight per list item (4 fewer controls if removed)
- If DataTriggers are added, enables listing status management from the UI
- Low risk for removal; medium risk if adding DataTriggers (needs ViewModel support)
