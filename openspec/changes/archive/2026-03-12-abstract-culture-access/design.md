# Abstract Culture Access -- Design

## Context

The app has an `ILocaleService` abstraction for locale access, but five call sites bypass it and read `Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName` directly. This creates a testing burden and inconsistency.

## Goals / Non-Goals

### Goals

- All locale access goes through `ILocaleService`
- No direct `Thread.CurrentThread.CurrentUICulture` access in ViewModels

### Non-Goals

- Changing `ILocaleService` behavior or API
- Adding new locale features

## Decisions

- Inject `ILocaleService` via primary constructor in all three ViewModels
- Replace `Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName` with the equivalent `ILocaleService` method/property
- If `ILocaleService` does not expose a `TwoLetterISOLanguageName` equivalent, add one

## Risks / Trade-offs

- **Minimal risk:** Straightforward dependency swap
- **Constructor change:** Adding a parameter to ViewModel constructors requires updating DI and any tests that construct them manually
