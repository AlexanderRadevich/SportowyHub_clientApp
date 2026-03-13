# Tasks

## Tasks

- [x] Add `CancellationToken ct` parameter to `AccountProfileViewModel.SignOutAsync` and propagate to service calls
- [x] Add `CancellationToken ct` parameter to `HomeViewModel.LoadSectionsAsync` and propagate to service calls
- [x] Add `CancellationToken ct` parameter to `HomeViewModel.ToggleFavoriteAsync` and propagate to service calls
- [x] Add `CancellationToken ct` parameter to `ProfileViewModel.RefreshAuthStateAsync` and propagate to service calls
- [x] Add `CancellationToken ct` parameter to `SearchViewModel.AppearingAsync` and propagate to service calls
- [x] Add `CancellationToken ct` parameter to async commands in `MyListingsViewModel` and propagate
- [x] Add `CancellationToken ct` parameter to async commands in `FavoritesViewModel` and propagate
- [x] Replace all remaining `CancellationToken.None` with propagated tokens in affected ViewModels
- [x] Add `CancellationToken ct = default` to service interface methods that lack it
- [x] Verify `dotnet build` succeeds and no tests break
