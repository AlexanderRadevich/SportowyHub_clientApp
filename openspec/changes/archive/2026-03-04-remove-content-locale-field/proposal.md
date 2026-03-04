## Why

The create/edit listing screen exposes a free-text `Entry` field for "Content Locale" that the user must fill in manually (defaults to `"pl"`). This is poor UX — the app already knows its current language via `ILocaleService`, and the backend only accepts `['ru', 'pl', 'en', 'uk']`. The field should be removed from the UI and auto-populated from the current app language.

## What Changes

- **Remove** the Content Locale `Entry` field from `CreateEditListingPage.xaml`
- **Remove** the `ContentLocale` observable property from `CreateEditListingViewModel`
- **Auto-populate** `content_locale` in `CreateListingRequest` and `UpdateListingRequest` using the current app locale from `ILocaleService`
- **Inject** `ILocaleService` into `CreateEditListingViewModel` to resolve the current locale at save time

## Capabilities

### New Capabilities

_(none)_

### Modified Capabilities

- `create-edit-listing-page`: Remove Content Locale UI field; auto-set from app language at save time
- `listing-management`: `CreateListingRequest` and `UpdateListingRequest` continue to send `content_locale`, but the value is sourced from `ILocaleService.GetLocaleInfoAsync().Locale` instead of user input

## Impact

- `SportowyHub.App/Views/MyListings/CreateEditListingPage.xaml` — remove ContentLocale field
- `SportowyHub.App/ViewModels/CreateEditListingViewModel.cs` — remove property, inject `ILocaleService`, auto-resolve locale in `Save`
- Existing tests for `CreateEditListingViewModel` may need updating to mock `ILocaleService`
- No backend changes required — `content_locale` continues to be sent with a valid value
