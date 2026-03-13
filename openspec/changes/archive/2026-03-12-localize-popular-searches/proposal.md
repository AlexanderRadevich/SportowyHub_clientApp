## Why

`SearchViewModel.cs` lines 59-66 contain hardcoded English strings for popular searches. The app supports 4 languages (pl, en, uk, ru), so these strings are only correct for English users.

## What Changes

Replace hardcoded popular search strings with localized resources from `.resx` files, or alternatively fetch them from the backend API.

## Capabilities

### New
- Localized popular search strings in all 4 language .resx files

### Modified
- `SearchViewModel.cs` — load popular searches from localized resources instead of hardcoded list

## Impact

- **i18n:** Popular searches display correctly in all 4 supported languages
- **UX:** Non-English users see relevant search suggestions in their language
- **Risk:** Low — string replacement with no logic changes
