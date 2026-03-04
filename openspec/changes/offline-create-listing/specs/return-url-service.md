## Requirement: Return URL service

The app SHALL provide an `IReturnUrlService` singleton that stores a single return route string for post-login navigation. The service SHALL be registered in the DI container as a singleton.

### Interface

```csharp
public interface IReturnUrlService
{
    void SetReturnUrl(string route);
    string? ConsumeReturnUrl();
    bool HasReturnUrl { get; }
}
```

### Scenario: Setting a return URL
- **GIVEN** the return URL service has no stored URL
- **WHEN** `SetReturnUrl("//favorites")` is called
- **THEN** `HasReturnUrl` SHALL return true

### Scenario: Consuming a return URL
- **GIVEN** `SetReturnUrl("//favorites")` was called
- **WHEN** `ConsumeReturnUrl()` is called
- **THEN** it SHALL return `"//favorites"`
- **AND** `HasReturnUrl` SHALL return false after consumption

### Scenario: Consuming when no return URL is set
- **GIVEN** no return URL has been set (or it was already consumed)
- **WHEN** `ConsumeReturnUrl()` is called
- **THEN** it SHALL return null

### Scenario: Setting a return URL overwrites the previous one
- **GIVEN** `SetReturnUrl("//favorites")` was called
- **WHEN** `SetReturnUrl("//profile")` is called
- **THEN** `ConsumeReturnUrl()` SHALL return `"//profile"`

### Scenario: Return URL is cleared after consumption
- **GIVEN** `SetReturnUrl("//home")` was called
- **AND** `ConsumeReturnUrl()` was called (returned `"//home"`)
- **WHEN** `ConsumeReturnUrl()` is called again
- **THEN** it SHALL return null

### Scenario: Registered as singleton in DI
- **GIVEN** the `MauiProgram.cs` service registration
- **WHEN** `IReturnUrlService` is registered
- **THEN** it SHALL be registered as a singleton (`AddSingleton<IReturnUrlService, ReturnUrlService>()`)

## Requirement: NavigateToLoginWithReturnUrlAsync on INavigationService

The `INavigationService` SHALL expose a `NavigateToLoginWithReturnUrlAsync(string returnUrl)` method. The `ShellNavigationService` implementation SHALL store the return URL via `IReturnUrlService` and then navigate to the `login` route.

### Scenario: NavigateToLoginWithReturnUrlAsync stores URL and navigates
- **GIVEN** the navigation service has an `IReturnUrlService` dependency
- **WHEN** `NavigateToLoginWithReturnUrlAsync("//favorites")` is called
- **THEN** it SHALL call `IReturnUrlService.SetReturnUrl("//favorites")`
- **AND** it SHALL call `GoToAsync("login")`

### Scenario: ShellNavigationService constructor accepts IReturnUrlService
- **GIVEN** the `ShellNavigationService` class
- **WHEN** it is constructed
- **THEN** it SHALL accept an `IReturnUrlService` parameter via primary constructor or constructor injection
