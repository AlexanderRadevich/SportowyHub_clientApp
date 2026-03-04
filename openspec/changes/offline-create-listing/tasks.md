## 1. Return URL Service

- [ ] 1.1 Create `IReturnUrlService` interface in `SportowyHub.App/Services/Navigation/IReturnUrlService.cs` with `SetReturnUrl(string route)`, `string? ConsumeReturnUrl()`, and `bool HasReturnUrl` members
- [ ] 1.2 Create `ReturnUrlService` implementation in `SportowyHub.App/Services/Navigation/ReturnUrlService.cs` as a simple class holding a `string?` field; `ConsumeReturnUrl` returns the value and clears it
- [ ] 1.3 Register `IReturnUrlService` as singleton in `SportowyHub.App/MauiProgram.cs`: `builder.Services.AddSingleton<IReturnUrlService, ReturnUrlService>()`

## 2. Navigation Service Update

- [ ] 2.1 Add `Task NavigateToLoginWithReturnUrlAsync(string returnUrl)` to `SportowyHub.App/Services/Navigation/INavigationService.cs`
- [ ] 2.2 Update `ShellNavigationService` in `SportowyHub.App/Services/Navigation/ShellNavigationService.cs`: add `IReturnUrlService` as a constructor dependency; implement `NavigateToLoginWithReturnUrlAsync` to call `SetReturnUrl` then `GoToAsync("login")`

## 3. FAB Visibility

- [ ] 3.1 Remove `IsVisible="{Binding IsLoggedIn}"` from the FAB `Button` in `SportowyHub.App/Views/Home/HomePage.xaml` (line 110)

## 4. HomeViewModel Auth Redirect

- [ ] 4.1 Add `INavigationService nav` usage for `NavigateToLoginWithReturnUrlAsync` in `SportowyHub.App/ViewModels/HomeViewModel.cs`
- [ ] 4.2 Update `GoToCreateListing` method: check `authService.IsLoggedInAsync()` first; if not logged in, call `await nav.NavigateToLoginWithReturnUrlAsync("//home")` and return; otherwise proceed with existing trust level check

## 5. LoginViewModel Return URL Navigation

- [ ] 5.1 Add `IReturnUrlService` as a constructor dependency to `LoginViewModel` in `SportowyHub.App/ViewModels/LoginViewModel.cs`
- [ ] 5.2 Update `Login` method: after successful login, call `ConsumeReturnUrl()`; if non-null, navigate to the return URL instead of `//home`
- [ ] 5.3 Update `OAuthLoginWithGoogle` method: after successful Google login, call `ConsumeReturnUrl()`; if non-null, navigate to the return URL instead of `//home`

## 6. FavoritesViewModel Auth Redirect

- [ ] 6.1 Update `SignIn` command in `SportowyHub.App/ViewModels/FavoritesViewModel.cs`: replace `nav.GoToAsync("login")` with `nav.NavigateToLoginWithReturnUrlAsync("//favorites")`

## 7. ProfileViewModel Auth Redirect

- [ ] 7.1 Update `SignInAsync` command in `SportowyHub.App/ViewModels/ProfileViewModel.cs`: replace `_nav.GoToAsync("login")` with `_nav.NavigateToLoginWithReturnUrlAsync("//profile")`

## 8. Build Verification

- [ ] 8.1 Build the solution (`dotnet build`) and fix any compilation errors
- [ ] 8.2 Run existing tests (`dotnet test`) and verify no regressions
