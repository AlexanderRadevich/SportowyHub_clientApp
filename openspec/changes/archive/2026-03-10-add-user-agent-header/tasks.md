## 1. UserAgentHandler Implementation

- [x] 1.1 Create `Services/Api/UserAgentHandler.cs` — a `DelegatingHandler` that builds the User-Agent string (`SportowyHub/{version} ({platform} {osVersion})`) in its constructor using `AppInfo.Current.VersionString`, `DeviceInfo.Current.Platform`, and `DeviceInfo.Current.VersionString`, and adds it to every request in `SendAsync` (skip if header already present)

## 2. DI Registration

- [x] 2.1 Register `UserAgentHandler` as transient in `MauiProgram.cs` and add `.AddHttpMessageHandler<UserAgentHandler>()` to the `"Api"` named client configuration

## 3. Verification

- [x] 3.1 Build the app (`dotnet build`) and verify no compilation errors
- [x] 3.2 Run existing tests (`dotnet test`) and verify no regressions
