# Split Auth Service Interface -- Tasks

## Tasks

- [x] Create `ITokenProvider` interface with `GetTokenAsync` and `RefreshTokenAsync`
- [x] Create `IProfileService` interface with profile CRUD and avatar methods
- [x] Slim `IAuthService` to auth-only methods (login, register, logout, OAuth)
- [x] Update `AuthService` to implement all three interfaces
- [x] Register all three interfaces in `MauiProgram.cs` DI container
- [x] Update consumer ViewModels/services to inject the narrowest interface
- [x] Update unit test mocks to use the new interfaces
- [x] Verify build and run existing test suite
