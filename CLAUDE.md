# SportowyHub — .NET MAUI Application

## Coding Style & Naming Conventions
- C#: governed by root `.editorconfig` — 4-space indent, CRLF line endings, `var` preferred, PascalCase for types/members, private fields `_camelCase`, interfaces prefixed `I`.
- Braces: always use `{}` for `if`, `else`, `for`, `while`, and `foreach` bodies—even single statements. Example: `if (isValid) { Save(); }` (not `if (isValid) Save();`).
- Target-typed new: prefer target-typed `new` and collection expressions when the type is clear. Examples: `Dictionary<string,int> map = new();`, `List<int> nums = [1, 2, 3];`, `var queue = new Queue<Job>();`.
- Logging: use structured logging with message templates instead of string interpolation. Example: `_logger.LogWarning("User {UserId} failed login attempt", userId);` (not `_logger.LogWarning($"User {userId} failed login attempt");`).
- Collection checks: use `.Count` comparison instead of `.Any()` for empty/non-empty checks. Example: `list.Count == 0` (not `!list.Any()`), `list.Count > 0` (not `list.Any()`).
- Line breaks: use `Environment.NewLine` instead of `"\n"` for newline characters in strings. Example: `$"Line 1{Environment.NewLine}Line 2"` (not `"Line 1\nLine 2"`).
- **Important**: do not add ***any*** comments unless asked.


## Project Context

This is a .NET 10 MAUI application targeting [Android / iOS / Windows / macOS / all platforms]. The app uses the MVVM pattern with CommunityToolkit.Mvvm for data binding, commands, and messaging. Navigation is handled via Shell routing. [Brief description of what the app does].

## Tech Stack

- **.NET 10** / C# 14
- **.NET MAUI** — cross-platform UI framework
- **CommunityToolkit.Mvvm** — source-generated MVVM (ObservableObject, RelayCommand, ObservableProperty)
- **CommunityToolkit.Maui** — converters, behaviors, animations, popups
- **SQLite** (via sqlite-net-pcl) / **HTTP APIs** — local or remote data access
- **xUnit v3** + **Moq** — unit testing

## Architecture

```
src/
  [ProjectName]/
    App.xaml(.cs)
    AppShell.xaml(.cs)
    MauiProgram.cs
    Features/
      [Feature]/
        [Feature]Page.xaml(.cs)
        [Feature]ViewModel.cs
        [Feature]Service.cs             # Optional per-feature service
    Services/
      I[Service]Service.cs
      [Service]Service.cs
      Navigation/
        INavigationService.cs
        ShellNavigationService.cs
    Models/
      [Entity].cs
    Data/
      [Repository or HttpClient wrapper]
    Resources/
      Images/
      Fonts/
      Styles/
        Colors.xaml
        Styles.xaml
    Platforms/
      Android/
        MainActivity.cs
        MainApplication.cs
      iOS/
        AppDelegate.cs
        Program.cs
      Windows/
        App.xaml(.cs)
      MacCatalyst/
        AppDelegate.cs
        Program.cs
    Converters/
      [Name]Converter.cs
    Controls/
      [CustomControl].cs
tests/
  [ProjectName].Tests/
    ViewModels/
      [Feature]ViewModelTests.cs
    Services/
      [Service]Tests.cs
    Converters/
      [Converter]Tests.cs
```

### Feature Organization

Each feature gets its own folder under `Features/` containing the page, view model, and optionally a feature-specific service. This keeps related code together and avoids sprawling `Views/` and `ViewModels/` folders.

### MVVM with CommunityToolkit.Mvvm

Use source generators for boilerplate-free view models:

```csharp
public partial class OrdersViewModel(IOrderService orderService, INavigationService nav) : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<OrderSummary> _orders = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RefreshCommand))]
    private bool _isLoading;

    [RelayCommand(CanExecute = nameof(CanRefresh))]
    private async Task RefreshAsync(CancellationToken ct)
    {
        IsLoading = true;
        try
        {
            var result = await orderService.GetOrdersAsync(ct);
            if (result.IsSuccess)
            {
                Orders = new(result.Value);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    private bool CanRefresh() => !IsLoading;

    [RelayCommand]
    private async Task GoToDetailAsync(int orderId)
    {
        await nav.GoToAsync($"orderdetail?id={orderId}");
    }
}
```

### Shell Navigation

Register routes in `AppShell.xaml.cs` and navigate with query parameters:

```csharp
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("orderdetail", typeof(OrderDetailPage));
    }
}

// Receiving parameters
[QueryProperty(nameof(OrderId), "id")]
public partial class OrderDetailViewModel(IOrderService orderService) : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    private int _orderId;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        OrderId = Convert.ToInt32(query["id"]);
        LoadOrderCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadOrderAsync(CancellationToken ct)
    {
        var result = await orderService.GetByIdAsync(OrderId, ct);
        // ...
    }
}
```

### Dependency Injection

Register everything in `MauiProgram.cs`:

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Services
        builder.Services.AddSingleton<IOrderService, OrderService>();
        builder.Services.AddSingleton<INavigationService, ShellNavigationService>();

        // ViewModels (transient — one per navigation)
        builder.Services.AddTransient<OrdersViewModel>();
        builder.Services.AddTransient<OrderDetailViewModel>();

        // Pages (transient — one per navigation)
        builder.Services.AddTransient<OrdersPage>();
        builder.Services.AddTransient<OrderDetailPage>();

        // HTTP clients
        builder.Services.AddHttpClient("Api", client =>
        {
            client.BaseAddress = new Uri("https://api.example.com");
        });

        return builder.Build();
    }
}
```

### Service Pattern

Services encapsulate data access and business logic. View models inject services, never access data directly:

```csharp
public interface IOrderService
{
    Task<Result<List<OrderSummary>>> GetOrdersAsync(CancellationToken ct = default);
    Task<Result<OrderDetail>> GetByIdAsync(int id, CancellationToken ct = default);
}

internal class OrderService(IHttpClientFactory httpFactory) : IOrderService
{
    public async Task<Result<List<OrderSummary>>> GetOrdersAsync(CancellationToken ct = default)
    {
        var client = httpFactory.CreateClient("Api");
        var orders = await client.GetFromJsonAsync<List<OrderSummary>>("orders", ct);
        return orders is not null
            ? Result.Success(orders)
            : Result.Failure<List<OrderSummary>>("Failed to load orders");
    }
}
```

## Coding Standards

- **C# 14 features** — Use primary constructors, collection expressions, `field` keyword, records, pattern matching
- **File-scoped namespaces** — Always
- **`var` for obvious types** — Use explicit types when the type isn't clear from context
- **Naming** — PascalCase for public members, `_camelCase` for private fields, suffix async methods with `Async`
- **No regions** — Ever
- **No comments for obvious code** — Only comment "why", never "what"
- **XAML naming** — `x:Name` only when needed for code-behind access; prefer data binding
- **One page per file** — XAML pages get their own `.xaml` + `.xaml.cs` pair

### XAML Conventions

- Use `{Binding}` with compiled bindings (`x:DataType`) for type safety and performance
- Keep code-behind minimal — only UI-specific logic that cannot be expressed in XAML or the view model
- Define reusable styles in `Resources/Styles/Styles.xaml`, not inline
- Use `StaticResource` over `DynamicResource` unless theme switching at runtime is required
- Prefer `DataTemplate` in XAML over programmatic template creation

### Platform-Specific Code

- Use partial classes and `Platforms/` folders for platform-specific implementations
- Use conditional compilation (`#if ANDROID`, `#if IOS`) only as a last resort
- Prefer `Microsoft.Maui.Essentials` abstractions over direct platform APIs
- Register platform-specific services via DI with `#if` in `MauiProgram.cs` when needed

## Skills

Load these dotnet-claude-kit skills for context:

- `modern-csharp` — C# 14 language features and idioms
- `architecture-advisor` — Architecture guidance for structuring the app
- `error-handling` — Result pattern for service layer error propagation
- `testing` — xUnit v3, unit testing view models and services
- `configuration` — Options pattern, secrets management
- `dependency-injection` — Service registration, scoped vs transient lifetimes
- `logging` — Serilog, structured logging
- `httpclient-factory` — IHttpClientFactory, typed clients, resilience
- `resilience` — Polly v8 resilience pipelines for network calls
- `workflow-mastery` — Parallel worktrees, verification loops, subagent patterns
- `self-correction-loop` — Capture corrections as permanent rules in MEMORY.md
- `wrap-up-ritual` — Structured session handoff to `.claude/handoff.md`
- `context-discipline` — Token budget management, MCP-first navigation

## MCP Tools

> **Setup:** Install once globally with `dotnet tool install -g CWM.RoslynNavigator` and register with `claude mcp add --scope user cwm-roslyn-navigator -- cwm-roslyn-navigator --solution ${workspaceFolder}`. After that, these tools are available in every .NET project.

Use `cwm-roslyn-navigator` tools to minimize token consumption:

- **Before modifying a type** — Use `find_symbol` to locate it, `get_public_api` to understand its surface
- **Before adding a reference** — Use `find_references` to understand existing usage
- **To understand architecture** — Use `get_project_graph` to see project dependencies
- **To find implementations** — Use `find_implementations` instead of grep for interface/abstract class implementations
- **To check for errors** — Use `get_diagnostics` after changes

## Commands

```bash
# Build
dotnet build

# Run on Android emulator
dotnet build -t:Run -f net10.0-android

# Run on iOS simulator
dotnet build -t:Run -f net10.0-ios

# Run on Windows
dotnet build -t:Run -f net10.0-windows10.0.19041.0

# Run on macOS
dotnet build -t:Run -f net10.0-maccatalyst

# Run tests
dotnet test

# Format check
dotnet format --verify-no-changes

# Publish for Android (signed APK)
dotnet publish -f net10.0-android -c Release

# Publish for iOS
dotnet publish -f net10.0-ios -c Release
```

## Workflow

- **Plan first** — Enter plan mode for any non-trivial task (3+ steps or architecture decisions). Iterate until the plan is solid before writing code.
- **Verify before done** — Run `dotnet build` and `dotnet test` after changes. Use `get_diagnostics` via MCP to catch warnings. Ask: "Would a staff engineer approve this?"
- **Fix bugs autonomously** — When given a bug report, investigate and fix it without hand-holding. Check logs, errors, failing tests — then resolve them.
- **Stop and re-plan** — If implementation goes sideways, STOP and re-plan. Don't push through a broken approach.
- **Use subagents** — Offload research, exploration, and parallel analysis to subagents. One task per subagent for focused execution.
- **Learn from corrections** — After any correction, capture the pattern in memory so the same mistake never recurs.
- **Test on target platforms** — MAUI behavior varies across platforms. When fixing a bug, consider whether it's platform-specific or cross-platform.

## Anti-patterns

Do NOT generate code that:

- **Uses `DateTime.Now`** — Use `TimeProvider` injection instead
- **Creates `new HttpClient()`** — Use `IHttpClientFactory` registered in `MauiProgram.cs`
- **Uses `async void`** — Always return `Task`; the sole exception is event handlers where the framework requires `void` return (wrap in try/catch and log errors)
- **Blocks with `.Result` or `.Wait()`** — Await instead; this is especially dangerous on the UI thread (deadlocks)
- **Puts business logic in code-behind** — Code-behind should only contain UI-specific logic (animations, focus management); everything else belongs in the view model or services
- **Uses `MessagingCenter`** — Removed in .NET 9+; use `WeakReferenceMessenger` from CommunityToolkit.Mvvm
- **Navigates from view models via `Shell.Current`** — Abstract navigation behind `INavigationService` for testability
- **Stores sensitive data in `Preferences`** — Use `SecureStorage` for tokens, credentials, and secrets
- **Uses `Device.BeginInvokeOnMainThread`** — Deprecated; use `MainThread.BeginInvokeOnMainThread` or `MainThread.InvokeOnMainThreadAsync`
- **Skips `CancellationToken` propagation** — Pass tokens through from commands to services to HTTP calls
- **Creates heavyweight controls inside `DataTemplate`** — Use simple layouts in collection item templates; extract complex items to `ContentView`
- **Handles all platforms in one giant `#if` block** — Use partial classes and platform folders instead
- **Uses `OnAppearing` for heavy data loading without indication** — Show `ActivityIndicator` and load data via commands with loading state
- **Ignores `IDisposable` on pages/view models** — Unsubscribe from messages and cancel pending operations when navigating away
- **Uses string interpolation in log messages** — Use structured logging templates
- **Catches bare `Exception`** — Catch specific types; use global `MauiExceptions.UnhandledException` for crash reporting
- **Returns domain entities to the UI** — Map to view-model-friendly DTOs or use `ObservableObject` wrappers
