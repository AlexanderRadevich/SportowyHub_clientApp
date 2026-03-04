## Context

The create/edit listing form currently exposes a free-text `Entry` for Content Locale, defaulting to `"pl"`. The backend validates `content_locale` against `['ru', 'pl', 'en', 'uk']`. The app already has `ILocaleService` registered as a singleton that returns the current app locale via `GetLocaleInfoAsync().Locale`. This service is not injected into `CreateEditListingViewModel`.

## Goals / Non-Goals

**Goals:**
- Remove the Content Locale field from the create/edit listing UI
- Auto-populate `content_locale` from the current app language when creating or updating a listing
- Ensure the value sent is always valid per backend constraints

**Non-Goals:**
- Adding a locale picker for users who want to write listings in a different language than their app language (can be added later if needed)
- Changing how `ILocaleService` works or caching its result differently

## Decisions

### Decision 1: Source locale from `ILocaleService` at save time

Inject `ILocaleService` into `CreateEditListingViewModel`. At save time, call `GetLocaleInfoAsync()` and use the `Locale` property as the `content_locale` value.

**Alternative considered:** Read `CultureInfo.CurrentUICulture` directly. Rejected because `ILocaleService` is the established abstraction for locale in this app, and it returns the backend-validated locale string (e.g., `"pl"`) rather than a .NET culture name (e.g., `"pl-PL"`).

### Decision 2: Keep ContentLocale on request DTOs unchanged

`CreateListingRequest.ContentLocale` and `UpdateListingRequest.ContentLocale` remain as `string?` parameters. The ViewModel resolves the locale and passes it. No DTO changes needed.

**Alternative considered:** Remove `ContentLocale` from DTOs and have the service layer inject it. Rejected because the ViewModel already orchestrates all request parameters, and moving one field to the service layer would break the pattern.

### Decision 3: For edit mode, ignore the existing listing's content_locale

When editing, always use the current app locale rather than preserving the listing's original `content_locale`. The content language should match what the user is currently writing in.

**Alternative considered:** Preserve the original `content_locale` during edit. Rejected because if the user edits while using a different app language, the content they write is in that language, not the original one.

## Risks / Trade-offs

**[Risk]** User writes a listing in a language different from their app language (e.g., app is in Polish but they write the listing in English). → Accepted trade-off for now. A future enhancement could add an optional language picker. The current UX is still better than a raw text field.

**[Risk]** `ILocaleService.GetLocaleInfoAsync()` makes a network call at save time, adding latency. → Low risk — this endpoint is lightweight (`GET /api/v1/locale`), and the call can share the existing `CancellationToken` flow. If the locale was already fetched elsewhere in the session, HTTP caching or a cached service could optimize this later.
