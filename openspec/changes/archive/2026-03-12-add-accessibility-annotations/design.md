## Context

The app has no semantic annotations for accessibility. Screen readers cannot convey the purpose of icon-only buttons, toggles, or custom interactive areas. This affects users relying on assistive technology on both Android (TalkBack) and iOS (VoiceOver).

## Goals / Non-Goals

**Goals:**
- Add `SemanticProperties.Description` to all icon-only buttons and images with actions
- Add `SemanticProperties.Hint` to interactive elements where the action is not obvious
- Ensure localized descriptions using AppResources where appropriate

**Non-Goals:**
- Full WCAG 2.1 AA compliance audit
- Adding automation IDs for UI testing (separate concern)

## Decisions

- Use `SemanticProperties.Description` for elements that need a label (icon buttons, images)
- Use `SemanticProperties.Hint` for elements that need action description (toggles, tappable areas)
- Localize descriptions via `AppResources` for the 4 supported languages
- Prioritize: icon-only buttons > toggles > custom tap areas > decorative images

## Risks / Trade-offs

- Touches many XAML files, increasing merge conflict surface
- Localization strings need translation for all 4 languages
- Screen reader behavior varies between Android TalkBack and iOS VoiceOver — test on both
