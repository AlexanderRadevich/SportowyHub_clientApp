## Tasks

- [x] Check if backend has a password reset endpoint or web page
- [x] If reset flow exists: add `ForgotPasswordCommand` to `LoginViewModel` that navigates to reset page or opens browser URL
- [x] If reset flow does not exist: remove link styling from "Forgot Password" text or add "Coming soon" indicator
- [x] Use `Button` with transparent background instead of `Label` + `TapGestureRecognizer` (per known MAUI issue)
- [x] Test forgot password interaction on Android and iOS
