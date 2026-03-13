## Context

The login page displays "Forgot Password" text styled as a tappable link, but no interaction handler is attached. This is a UX defect where the visual affordance does not match behavior.

## Goals / Non-Goals

**Goals:**
- Make the "Forgot Password" element either functional or visually non-interactive
- If the backend supports password reset, wire up navigation

**Non-Goals:**
- Implementing a full password reset flow from scratch (that would be a separate feature)

## Decisions

- Check if the backend has a password reset endpoint or web page
- If yes: add a `ForgotPasswordCommand` to `LoginViewModel` that navigates to the reset page or opens the browser
- If no: restyle the text to remove link appearance (remove colored text, remove underline) and optionally add a "Coming soon" label
- Per CLAUDE.md, use `Button` with transparent background instead of `Label` + `TapGestureRecognizer` for reliability

## Risks / Trade-offs

- **Backend dependency:** Full forgot-password flow requires backend support. If unavailable, the fix is purely cosmetic.
- **Platform behavior:** Browser launch via `Launcher.OpenAsync` may behave differently across platforms; test on Android and iOS.
