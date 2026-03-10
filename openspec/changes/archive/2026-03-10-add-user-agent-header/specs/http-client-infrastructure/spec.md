## ADDED Requirements

### Requirement: User-Agent header on all API requests
The `"Api"` named HttpClient pipeline SHALL include a `DelegatingHandler` that adds a `User-Agent` header to every outgoing HTTP request. The User-Agent string SHALL follow the format `SportowyHub/{appVersion} ({platform} {osVersion})` where `appVersion` is from `AppInfo.Current.VersionString`, `platform` is from `DeviceInfo.Current.Platform`, and `osVersion` is from `DeviceInfo.Current.VersionString`. The User-Agent string SHALL be computed once at handler construction and reused for all requests. The handler SHALL NOT overwrite a `User-Agent` header if one is already present on the request.

#### Scenario: API request includes User-Agent header
- **WHEN** any HTTP request is sent through the `"Api"` named HttpClient
- **THEN** the request SHALL include a `User-Agent` header matching the format `SportowyHub/{version} ({platform} {osVersion})`

#### Scenario: User-Agent contains correct app version
- **WHEN** `AppInfo.Current.VersionString` returns `"1.2.3"`
- **THEN** the User-Agent header SHALL contain `SportowyHub/1.2.3`

#### Scenario: User-Agent contains platform and OS version
- **WHEN** `DeviceInfo.Current.Platform` is `Android` and `DeviceInfo.Current.VersionString` is `"14"`
- **THEN** the User-Agent header SHALL contain `(Android 14)`

#### Scenario: Existing User-Agent header is not overwritten
- **WHEN** a request already has a `User-Agent` header set by the caller
- **THEN** the handler SHALL NOT replace or duplicate the existing header

## MODIFIED Requirements

### Requirement: DI registration for HTTP infrastructure
`MauiProgram.cs` SHALL register `HttpClient` as a named client `"Api"` via `IHttpClientFactory` and `IRequestProvider` as a singleton backed by `RequestProvider`. The `HttpClient` SHALL be configured with the base URL from `ApiConfig.BaseUrl`. The `"Api"` client SHALL have a `UserAgentHandler` registered in its message handler pipeline via `AddHttpMessageHandler<UserAgentHandler>()`. The `UserAgentHandler` SHALL be registered as a transient service.

#### Scenario: RequestProvider resolves from DI
- **WHEN** a service requests `IRequestProvider` from the DI container
- **THEN** it SHALL receive a singleton `RequestProvider` instance wrapping the shared `HttpClient`

#### Scenario: API client pipeline includes UserAgentHandler
- **WHEN** the `"Api"` named HttpClient is created by the factory
- **THEN** its message handler pipeline SHALL include the `UserAgentHandler`
