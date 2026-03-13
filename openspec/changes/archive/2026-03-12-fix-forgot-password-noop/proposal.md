## Why

`LoginPage.xaml` lines 67-71 render "Forgot Password" text styled as a clickable link (colored text) but it has no `TapGestureRecognizer` or command attached. Users perceive it as interactive but nothing happens on tap.

## What Changes

Either wire up forgot-password functionality (navigate to a reset page or open the web reset URL) if the backend supports it, or remove the link styling to clearly indicate it is not yet interactive.

## Capabilities

### New
- Forgot password navigation command (if backend flow exists)

### Modified
- `LoginPage.xaml` — add tap handler or remove link styling
- `LoginViewModel.cs` — add forgot password command (if implementing the flow)

## Impact

- **UX:** Eliminates a misleading interactive element
- **Trust:** Users no longer tap a non-functional link, reducing frustration
- **Risk:** Low — either a simple navigation addition or a styling change
