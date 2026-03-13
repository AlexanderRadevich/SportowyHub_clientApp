# Tasks: Externalize API Config

## Tasks

- [x] Create environment-based config mechanism (embedded `appsettings.{env}.json` or MSBuild properties)
- [x] Move `DefaultBaseUrl` from hardcoded constant to config source
- [x] Move `GoogleClientId` from hardcoded constant to config source
- [x] Add MSBuild target to fail release builds if resolved URL contains "ngrok"
- [x] Update `MauiProgram.cs` to register config in DI
- [x] Update all references to `ApiConfig.DefaultBaseUrl` to use the new config
- [x] Add developer override mechanism (environment variable or local untracked file)
- [x] Test Debug build resolves to dev URL
- [x] Test Release build resolves to production URL and rejects ngrok
