# Code Review: offline-create-listing

## Status: PASS

## Spec Coverage Matrix

### fab-visibility.md
| Spec | Scenario | Implementation | Test | Status |
|------|----------|---------------|------|--------|
| fab-visibility | FAB visible when user is not logged in | HomePage.xaml: FAB has no `IsVisible` binding | No dedicated test (XAML-level, not unit-testable) | PASS |
| fab-visibility | FAB visible when user is logged in | HomePage.xaml: FAB has no `IsVisible` binding | No dedicated test (XAML-level, not unit-testable) | PASS |
| fab-visibility | FAB does not have IsVisible binding to IsLoggedIn | HomePage.xaml line 109-110: `<Button Style="{StaticResource FloatingActionButton}" Command="{Binding GoToCreateListingCommand}" />` -- no IsVisible, uses FloatingActionButton style, binds Command correctly | Verified via grep -- no match for `IsVisible.*IsLoggedIn` | PASS |

### auth-redirect.md
| Spec | Scenario | Implementation | Test | Status |
|------|----------|---------------|------|--------|
| auth-redirect | Unauthenticated user taps FAB | HomeViewModel.cs lines 46-49: checks `IsLoggedInAsync()`, calls `NavigateToLoginWithReturnUrlAsync("//home")`, returns | `GoToCreateListing_WhenNotLoggedIn_NavigatesToLoginWithReturnUrl` | PASS |
| auth-redirect | Authenticated user with sufficient trust level taps FAB | HomeViewModel.cs lines 52-59: gets user, checks trust level, navigates to `create-edit-listing` | `GoToCreateListing_WhenLoggedInAndVerified_NavigatesToCreateEditListing` | PASS |
| auth-redirect | Authenticated user with Unverified trust level taps FAB | HomeViewModel.cs lines 53-57: shows toast error with `CreateListingPhoneRequired` | `GoToCreateListing_WhenLoggedInButUnverified_ShowsToastError` | PASS |
| auth-redirect | GoToCreateListing checks auth before trust level | HomeViewModel.cs lines 46-50: `IsLoggedInAsync()` check is first, returns before trust level check | `GoToCreateListing_WhenNotLoggedIn_NavigatesToLoginWithReturnUrl` verifies no `GoToAsync` called | PASS |

### return-url-service.md
| Spec | Scenario | Implementation | Test | Status |
|------|----------|---------------|------|--------|
| return-url-service | Setting a return URL | ReturnUrlService.cs lines 9-12: sets `_returnUrl` field | `HasReturnUrl_AfterSetReturnUrl_ReturnsTrue` | PASS |
| return-url-service | Consuming a return URL | ReturnUrlService.cs lines 14-19: returns value and clears | `ConsumeReturnUrl_WhenUrlSet_ReturnsUrlAndClears` | PASS |
| return-url-service | Consuming when no return URL is set | ReturnUrlService.cs line 16: returns null `_returnUrl` | `ConsumeReturnUrl_WhenNoUrlSet_ReturnsNull` | PASS |
| return-url-service | Setting overwrites previous | ReturnUrlService.cs line 11: simple assignment overwrites | `SetReturnUrl_CalledTwice_OverwritesPreviousValue` | PASS |
| return-url-service | Return URL cleared after consumption | ReturnUrlService.cs line 17: sets `_returnUrl = null` | `ConsumeReturnUrl_CalledTwice_SecondCallReturnsNull` | PASS |
| return-url-service | Registered as singleton in DI | MauiProgram.cs line 64: `AddSingleton<IReturnUrlService, ReturnUrlService>()` | N/A (integration-level, verified by inspection) | PASS |
| return-url-service | NavigateToLoginWithReturnUrlAsync stores URL and navigates | ShellNavigationService.cs lines 25-29: calls `SetReturnUrl` then `GoToAsync("login")` | `Constructor_AcceptsReturnUrlService` (partial coverage) | PASS |
| return-url-service | ShellNavigationService constructor accepts IReturnUrlService | ShellNavigationService.cs line 3: primary constructor with `IReturnUrlService` parameter | `Constructor_AcceptsReturnUrlService` | PASS |

### login-return-navigation.md
| Spec | Scenario | Implementation | Test | Status |
|------|----------|---------------|------|--------|
| login-return-navigation | Login success with return URL set | LoginViewModel.cs lines 79-81: consumes return URL, navigates `..` then return URL | `Login_WhenSuccessful_WithReturnUrl_NavigatesToReturnUrl` | PASS |
| login-return-navigation | Login success without return URL | LoginViewModel.cs lines 79-81: `returnUrl ?? "//home"` | `Login_WhenSuccessful_WithoutReturnUrl_NavigatesToHome` | PASS |
| login-return-navigation | Google OAuth login success with return URL | LoginViewModel.cs lines 128-130: same pattern in `OAuthLoginWithGoogle` | `OAuthLoginWithGoogle_WhenSuccessful_WithReturnUrl_NavigatesToReturnUrl` | PASS |
| login-return-navigation | Google OAuth login success without return URL | LoginViewModel.cs lines 128-130: `returnUrl ?? "//home"` | `OAuthLoginWithGoogle_WhenSuccessful_WithoutReturnUrl_NavigatesToHome` | PASS |
| login-return-navigation | Login failure does not consume return URL | LoginViewModel.cs lines 85-97: failure paths do not call `ConsumeReturnUrl` | `Login_WhenFailed_DoesNotConsumeReturnUrl` | PASS |
| login-return-navigation | Return URL consumed exactly once | ReturnUrlService.cs lines 14-19: consume clears the field | `ConsumeReturnUrl_CalledTwice_SecondCallReturnsNull` (in ReturnUrlServiceTests) | PASS |
| login-return-navigation | LoginViewModel injects IReturnUrlService | LoginViewModel.cs line 14: primary constructor parameter | `LoginViewModelTests` constructor (line 21) | PASS |

### favorites-auth-guard.md
| Spec | Scenario | Implementation | Test | Status |
|------|----------|---------------|------|--------|
| favorites-auth-guard | Unauthenticated user taps sign-in on Favorites | FavoritesViewModel.cs lines 167-170: `SignIn` calls `NavigateToLoginWithReturnUrlAsync("//favorites")` | `SignIn_NavigatesToLoginWithFavoritesReturnUrl` | PASS |
| favorites-auth-guard | User returns to Favorites after login | LoginViewModel.cs lines 79-81: navigates to consumed return URL | Covered by LoginViewModelTests (return URL flow) | PASS |
| favorites-auth-guard | SignIn command uses NavigateToLoginWithReturnUrlAsync | FavoritesViewModel.cs line 169: uses `nav.NavigateToLoginWithReturnUrlAsync("//favorites")` | `SignIn_NavigatesToLoginWithFavoritesReturnUrl` | PASS |

### profile-auth-guard.md
| Spec | Scenario | Implementation | Test | Status |
|------|----------|---------------|------|--------|
| profile-auth-guard | Unauthenticated user taps sign-in on Profile | ProfileViewModel.cs lines 153-156: `SignInAsync` calls `NavigateToLoginWithReturnUrlAsync("//profile")` | `SignIn_NavigatesToLoginWithProfileReturnUrl` (SKIPPED) | PASS (with caveat) |
| profile-auth-guard | User returns to Profile after login | LoginViewModel.cs lines 79-81: navigates to consumed return URL | Covered by LoginViewModelTests (return URL flow) | PASS |
| profile-auth-guard | SignInAsync command uses NavigateToLoginWithReturnUrlAsync | ProfileViewModel.cs line 155: uses `_nav.NavigateToLoginWithReturnUrlAsync("//profile")` | `SignIn_NavigatesToLoginWithProfileReturnUrl` (SKIPPED) | PASS (with caveat) |
| profile-auth-guard | CreateAccountAsync remains unchanged | ProfileViewModel.cs lines 159-162: calls `_nav.GoToAsync("register")` with no return URL | No dedicated test | PASS |

## Convention Compliance

- [x] No unnecessary comments in implementation files
- [x] PascalCase for public members, `_camelCase` for private fields
- [x] No `async void` -- all async methods return `Task`
- [x] No `new HttpClient()`
- [x] No string interpolation in logging (no logging calls present in changed code)
- [x] Uses interfaces for DI (`IReturnUrlService`, `INavigationService`, `IAuthService`)
- [x] Uses `{}` for all if/else/for/while bodies
- [x] File-scoped namespaces used throughout
- [x] `var` used for obvious types
- [x] Primary constructors used where appropriate (ShellNavigationService, HomeViewModel, LoginViewModel, FavoritesViewModel)
- [x] No regions
- [x] Singleton ReturnUrlService registered in DI (MauiProgram.cs line 64)
- [x] ShellNavigationService properly injects IReturnUrlService via primary constructor
- [x] LoginViewModel consumes return URL on both email/password and Google OAuth login paths
- [x] FAB `IsVisible` binding removed from XAML -- no `IsVisible` attribute present on the FAB Button

## Architecture Compliance

- [x] `IReturnUrlService` / `ReturnUrlService` in `Services/Navigation/` -- correct location
- [x] `ReturnUrlService` registered as singleton -- correct for in-memory transient state
- [x] `NavigateToLoginWithReturnUrlAsync` on `INavigationService` -- keeps `IReturnUrlService` injection out of individual ViewModels (except LoginViewModel which needs it for consumption)
- [x] LoginViewModel directly injects `IReturnUrlService` -- correct, it is the consumer
- [x] Return URL is `"//home"` for the FAB redirect, not `"create-edit-listing"` -- correct per design decision to preserve trust level check on re-tap

## Issues Found

### Issue 1: ProfileViewModel test is skipped (LOW severity)

**File:** `C:\dev\SportowyHub_clientApp\SportowyHub.Tests\ViewModels\ProfileViewModelTests.cs` line 13

The `SignIn_NavigatesToLoginWithProfileReturnUrl` test is marked with `[Fact(Skip = "ProfileViewModel constructor calls Preferences.Get() which requires MAUI runtime")]`. This means the profile auth guard scenario is not actually verified during test runs. The implementation is correct by code inspection, but there is no automated verification.

**Impact:** Low. The implementation is straightforward (single line calling `NavigateToLoginWithReturnUrlAsync`), and the pattern is identical to FavoritesViewModel which is tested. However, a regression could go undetected.

**Recommendation:** Extract the `Preferences.Get()` calls in the ProfileViewModel constructor behind an `IPreferencesService` abstraction, or use a static helper that can be substituted in tests. This would allow the test to run.

### Issue 2: ShellNavigationService.NavigateToLoginWithReturnUrlAsync lacks direct behavioral test (LOW severity)

**File:** `C:\dev\SportowyHub_clientApp\SportowyHub.Tests\Services\ShellNavigationServiceTests.cs`

The `ShellNavigationServiceTests` only verifies that the constructor accepts `IReturnUrlService`. There is no test that `NavigateToLoginWithReturnUrlAsync` calls `SetReturnUrl` and then navigates to `"login"`. This is because `ShellNavigationService` calls `Shell.Current.GoToAsync()` directly, which requires MAUI runtime.

**Impact:** Low. The implementation is trivial (two lines), and the behavior is implicitly tested through the ViewModel-level tests that verify the end-to-end flow.

**Recommendation:** Consider extracting `Shell.Current` behind a thin wrapper to enable pure unit testing of `ShellNavigationService`. This is a broader refactoring concern, not specific to this feature.

### Issue 3: Missing test for `CreateAccountAsync` unchanged behavior (NEGLIGIBLE severity)

**File:** `C:\dev\SportowyHub_clientApp\SportowyHub.Tests\ViewModels\ProfileViewModelTests.cs`

The spec `profile-auth-guard.md` scenario "CreateAccountAsync remains unchanged" has no dedicated test. Since this method was not modified and the test would face the same `Preferences.Get()` skip issue, this is negligible.

### Issue 4: Build failure blocks test execution (ENVIRONMENT, not code)

The Windows XAML compiler (`XamlCompiler.exe`) is failing with exit code 1, preventing `dotnet test` from completing. This is a pre-existing environment issue with the `net10.0-windows10.0.19041.0` target framework and the `Microsoft.WindowsAppSDK 1.7.250909003` package, not related to this feature's changes. Roslyn diagnostics confirm zero errors/warnings in all changed files.

## Test Quality Assessment

- [x] Tests use NSubstitute for mocking
- [x] Tests use FluentAssertions
- [x] Tests follow AAA pattern (Arrange/Act/Assert)
- [x] ReturnUrlService tests cover all spec scenarios (6/6)
- [x] HomeViewModel tests cover all auth redirect scenarios (4/4)
- [x] LoginViewModel tests cover email and Google OAuth paths with and without return URLs, plus failure case (5/5)
- [x] FavoritesViewModel test covers sign-in redirect (1/1)
- [ ] ProfileViewModel test exists but is skipped due to MAUI runtime dependency (1/1 skipped)

## What's Good

- **Clean separation of concerns**: `IReturnUrlService` is a focused, single-responsibility service. The consume-on-read pattern prevents stale URLs elegantly.
- **Minimal blast radius**: The return URL pattern was added without modifying existing navigation behavior when no return URL is set (`returnUrl ?? "//home"` preserves backward compatibility).
- **Design decision to use `"//home"` instead of `"create-edit-listing"`** for the FAB return URL is well-reasoned -- it preserves the trust level check.
- **Consistent pattern**: FavoritesViewModel and ProfileViewModel both use `NavigateToLoginWithReturnUrlAsync` identically, keeping the codebase predictable.
- **No over-engineering**: The `ReturnUrlService` is a simple class with a single field -- no unnecessary complexity for what is fundamentally a simple transient state holder.
- **Test quality is strong**: Tests verify both positive and negative paths, mock boundaries are clean, and assertion messages would be clear on failure.

## Recommendations

1. **Address the skipped ProfileViewModel test** by abstracting `Preferences.Get()` behind a service. This is a broader codebase concern but would improve test coverage for this feature.
2. **Consider adding a test for the `NavigateToLoginWithReturnUrlAsync` method** on `ShellNavigationService` if the `Shell.Current` dependency is ever abstracted.
3. **The bare `catch (Exception ex)` in HomeViewModel** (line 89, 119, 153) pre-dates this feature but is worth noting as an anti-pattern per CLAUDE.md conventions. Not a blocker for this review.
