## Context

The search feature displays popular/suggested searches to help users discover content. These are currently hardcoded in English in the ViewModel.

## Goals / Non-Goals

**Goals:**
- Provide popular search terms in all 4 supported languages (pl, en, uk, ru)
- Maintain the same UX flow for displaying suggestions

**Non-Goals:**
- Building a dynamic trending searches feature from analytics
- Changing the search UI layout

## Decisions

- Use `.resx` localization files to store popular search strings (keyed as `PopularSearch_1` through `PopularSearch_N`)
- Load from `AppResources` at runtime so the correct language is used automatically
- If the backend later provides a trending searches endpoint, it can replace the static list without UI changes

## Risks / Trade-offs

- **Static vs dynamic:** Static .resx strings are simple but cannot adapt to actual user trends. Acceptable for now; backend endpoint can be added later.
- **Translation quality:** Popular searches should be culturally relevant per language, not direct translations. Requires review from native speakers.
