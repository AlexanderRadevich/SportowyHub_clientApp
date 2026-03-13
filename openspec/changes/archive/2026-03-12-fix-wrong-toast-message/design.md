## Context

The CreateEditListingViewModel has branching logic for create vs. edit mode. In the error path for the non-edit branch, a copy-paste error causes the success resource string to be shown instead of an error string.

## Goals / Non-Goals

**Goals:**
- Fix the incorrect resource string reference
- Verify all other toast calls in the file use correct strings

**Non-Goals:**
- Refactoring the create/edit branching logic
- Adding new error resource strings (use existing ones)

## Decisions

- Replace `AppResources.ListingCreateSuccess` with the appropriate error resource string
- Audit all toast calls in CreateEditListingViewModel for similar copy-paste issues

## Risks / Trade-offs

- Extremely low risk — single string reference change
- Must verify the correct error resource exists in all 4 localization files (pl, en, uk, ru)
