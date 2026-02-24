## ADDED Requirements

### Requirement: Generic HTTP request provider interface
The app SHALL contain an `IRequestProvider` interface exposing generic HTTP methods: `GetAsync<TResult>(uri, token)`, `PostAsync<TRequest, TResponse>(uri, data, token)`, `PutAsync<TResult>(uri, data, token)`, and `DeleteAsync(uri, token)`. The `token` parameter SHALL be optional and default to empty string. When a non-empty token is provided, the request SHALL include an `Authorization: Bearer {token}` header.

#### Scenario: POST request with JSON body and no auth
- **WHEN** `PostAsync<TRequest, TResponse>(uri, data)` is called without a token
- **THEN** the provider SHALL send an HTTP POST with the request body serialized as JSON and no Authorization header

#### Scenario: GET request with Bearer token
- **WHEN** `GetAsync<TResult>(uri, token)` is called with a non-empty token
- **THEN** the provider SHALL send an HTTP GET with an `Authorization: Bearer {token}` header

#### Scenario: Request with empty token omits auth header
- **WHEN** any method is called with an empty or null token
- **THEN** the request SHALL NOT include an Authorization header

### Requirement: RequestProvider implementation with centralized error handling
The `RequestProvider` class SHALL implement `IRequestProvider` using a single `HttpClient` instance. On receiving an HTTP response, it SHALL call a centralized `HandleResponse` method that throws `ServiceAuthenticationException` for 401/403 status codes and `HttpRequestException` for all other non-success status codes. The response body SHALL be included in the exception message.

#### Scenario: Successful response deserialized
- **WHEN** the server returns a 2xx response with a JSON body
- **THEN** the provider SHALL deserialize the body to `TResult` and return it

#### Scenario: 401 response throws ServiceAuthenticationException
- **WHEN** the server returns a 401 Unauthorized response
- **THEN** the provider SHALL throw a `ServiceAuthenticationException` containing the response body

#### Scenario: 409 response throws HttpRequestException
- **WHEN** the server returns a 409 Conflict response
- **THEN** the provider SHALL throw an `HttpRequestException` with the response body and status code accessible

#### Scenario: 422 response throws HttpRequestException
- **WHEN** the server returns a 422 Validation Error response
- **THEN** the provider SHALL throw an `HttpRequestException` with the response body containing validation details

### Requirement: Source-generated JSON serialization context
The app SHALL contain a `SportowyHubJsonContext` class annotated with `[JsonSerializable]` for all API model types. The `RequestProvider` SHALL use this context for all JSON serialization and deserialization. The context SHALL use `PropertyNameCaseInsensitive = true` and `PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower`.

#### Scenario: JSON serialization uses source-generated context
- **WHEN** the `RequestProvider` serializes or deserializes a request/response body
- **THEN** it SHALL use `SportowyHubJsonContext.Default` for AOT-compatible serialization

#### Scenario: Snake case property mapping
- **WHEN** the API returns JSON with snake_case property names (e.g., `trust_level`)
- **THEN** the deserializer SHALL map them to PascalCase C# properties (e.g., `TrustLevel`)

### Requirement: API base URL configuration
The app SHALL contain an `ApiConfig` static class with a `BaseUrl` property providing the API server base URL. The default value SHALL be a compile-time constant. The value SHALL be overridable at runtime via `Preferences.Get("api_base_url", defaultValue)`.

#### Scenario: Default base URL used when no override exists
- **WHEN** no `api_base_url` preference is set
- **THEN** `ApiConfig.BaseUrl` SHALL return the compile-time default URL

#### Scenario: Override base URL via preferences
- **WHEN** a developer sets `Preferences.Set("api_base_url", "https://custom.example.com")`
- **THEN** `ApiConfig.BaseUrl` SHALL return `"https://custom.example.com"`

### Requirement: ServiceAuthenticationException for auth failures
The app SHALL contain a `ServiceAuthenticationException` class extending `Exception`. It SHALL store the HTTP response content as a `Content` string property. This exception SHALL be thrown by `RequestProvider` exclusively for 401 and 403 HTTP responses.

#### Scenario: Exception carries response content
- **WHEN** a `ServiceAuthenticationException` is thrown for a 401 response with body `{"error":{"code":"invalid_credentials","message":"Wrong password"}}`
- **THEN** the exception's `Content` property SHALL contain that JSON string

### Requirement: DI registration for HTTP infrastructure
`MauiProgram.cs` SHALL register `HttpClient` as a singleton and `IRequestProvider` as a singleton backed by `RequestProvider`. The `HttpClient` SHALL be configured with the base URL from `ApiConfig.BaseUrl`.

#### Scenario: RequestProvider resolves from DI
- **WHEN** a service requests `IRequestProvider` from the DI container
- **THEN** it SHALL receive a singleton `RequestProvider` instance wrapping the shared `HttpClient`
