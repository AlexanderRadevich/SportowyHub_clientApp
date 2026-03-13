## Why

`CreateEditListingViewModel.cs:251` shows `AppResources.ListingCreateSuccess` as an error toast when the listing is not in edit mode. This is a copy-paste bug — the success message is displayed in an error context, confusing users when an operation actually fails.

## What Changes

Replace the incorrect resource string with the correct error resource string in the non-edit-mode error path.

## Capabilities

### Modified

- CreateEditListingViewModel error toast displays the correct error message

## Impact

- Fixes misleading user feedback on listing creation failure
- No logic changes — only the displayed string is corrected
- Low risk, isolated one-line fix
