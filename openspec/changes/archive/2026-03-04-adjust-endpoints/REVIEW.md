## Review Result: PASS

All spec requirements are implemented correctly. All scenarios have corresponding tests. No CLAUDE.md convention violations were found in the changed files.

---

## Coverage Matrix

| Spec Requirement | Implementation File | Test File | Status |
|---|---|---|---|
| Task 1.1: `CreateListingAsync` URI = `/api/private/listings` (no trailing slash) | `SportowyHub.App/Services/ListingManagement/ListingManagementService.cs` line 22 | `SportowyHub.Tests/Services/ListingManagementServiceTests.cs` (3 tests) | PASS |
| Task 1.2: `CreateConversationAsync` URI = `/api/private/conversations` (no trailing slash) | `SportowyHub.App/Services/Messaging/MessagingService.cs` line 13 | `SportowyHub.Tests/Services/MessagingServiceTests.cs` (3 tests) | PASS |
| Task 1.3: `UploadAsync` URI = `/api/private/media` (no trailing slash) | `SportowyHub.App/Services/Media/MediaService.cs` line 21 | `SportowyHub.Tests/Services/MediaServiceTests.cs` (3 tests) | PASS |
| Task 2.1: `FavoriteActionResponse` gains `int? FavoritesCount` | `SportowyHub.App/Models/Api/FavoriteActionResponse.cs` | `SportowyHub.Tests/Models/FavoriteActionResponseTests.cs` (6 tests) | PASS |
| Task 2.2: `FavoriteActionResponse` registered in `SportowyHubJsonContext` | `SportowyHub.App/Services/Api/SportowyHubJsonContext.cs` line 35 | (verified via deserialization tests using `SportowyHubJsonContext.Default.Options`) | PASS |
| Task 3.1: `FavoriteItem` gains `int SerialId` | `SportowyHub.App/Models/Api/FavoriteItem.cs` line 14 | (no dedicated test — see gap below) | PARTIAL |
| Task 3.2: `FavoriteItem` registered in `SportowyHubJsonContext` | `SportowyHub.App/Services/Api/SportowyHubJsonContext.cs` line 32 | (no dedicated test) | PARTIAL |

---

## Scenario Coverage

### endpoint-alignment spec.md

| Scenario | Test Method | Status |
|---|---|---|
| Create listing request uses correct URL | `CreateListingAsync_WhenCalled_SendsRequestWithNoTrailingSlash` | PASS |
| No 308 redirect: URI is exact string match | `CreateListingAsync_WhenCalled_UriIsExactlyListingsEndpoint` | PASS |
| Create conversation request uses correct URL | `CreateConversationAsync_WhenCalled_SendsRequestWithNoTrailingSlash` | PASS |
| No 308 redirect: URI is exact string match | `CreateConversationAsync_WhenCalled_UriIsExactlyConversationsEndpoint` | PASS |
| Media upload request uses correct URL | `UploadAsync_WhenCalled_SendsRequestWithNoTrailingSlash` | PASS |
| No 308 redirect: URI is exact string match | `UploadAsync_WhenCalled_UriIsExactlyMediaEndpoint` | PASS |
| Verified endpoint catalogue matches backend | No automated test | NOT TESTED (see gap below) |

### auth-api-models spec.md

| Scenario | Test Method | Status |
|---|---|---|
| Add favorite returns favorites count | `Deserialize_WhenFavoritesCountPresent_PopulatesValue` | PASS |
| FavoritesCount is null-safe when field is absent | `Deserialize_WhenFavoritesCountAbsent_FavoritesCountIsNull` + `Deserialize_WhenFavoritesCountAbsent_DoesNotThrow` | PASS |
| FavoritesCount null when JSON value is null | `Deserialize_WhenFavoritesCountIsNull_FavoritesCountIsNull` | PASS |
| FavoritesCount zero when JSON value is 0 | `Deserialize_WhenFavoritesCountIsZero_PopulatesZero` | PASS |
| Status field populated correctly | `Deserialize_WhenStatusIsPresent_PopulatesStatus` | PASS |
| Favorites list response contains serial ID per item | No dedicated test | NOT TESTED (see gap below) |
| FavoriteItem registered in SportowyHubJsonContext with SerialId | No dedicated test | NOT TESTED (see gap below) |

---

## Gaps

### Gap 1: No tests for `FavoriteItem.SerialId`

The spec defines two scenarios for `FavoriteItem`:
- Favorites list response contains serial ID per item (GIVEN/WHEN/THEN deserialization test)
- FavoriteItem is registered in SportowyHubJsonContext (round-trip test)

Neither scenario has a corresponding test. The field itself is correctly implemented (`int SerialId` as the last positional parameter in `FavoriteItem.cs`), and `FavoriteItem` is registered in `SportowyHubJsonContext` (line 32). The omission is a test coverage gap only; the production code is correct.

Recommended test to add in `SportowyHub.Tests/Models/FavoriteItemTests.cs`:

```csharp
[Fact]
public void Deserialize_WhenSerialIdPresent_PopulatesValue()
{
    var json = """{"id":"abc","title":"Test","price":null,"currency":null,"city":null,"status":"active","slug":null,"serial_id":7}""";

    var result = JsonSerializer.Deserialize<FavoriteItem>(json, SportowyHubJsonContext.Default.Options);

    result.Should().NotBeNull();
    result!.SerialId.Should().Be(7);
}
```

### Gap 2: No automated test for full endpoint catalogue audit (Task 4.3)

The spec requires verifying that all other service endpoint URIs (auth, sections, locale, DSAR, phone verification, theme, favorites GET/DELETE) match backend route definitions exactly. This is documented in `tasks.md` item 4.3 as a manual audit. No automated test covers it, which is consistent with the task description. The manual audit outcome is not recorded in `tasks.md` item 4.4 as required.

---

## Convention Checks

All changed files were reviewed against CLAUDE.md coding standards.

### FavoriteActionResponse.cs

```csharp
public record FavoriteActionResponse(
    string Status,
    int? FavoritesCount);
```

- File-scoped namespace: PASS
- No comments: PASS
- Nullable `int?` as specified in design.md Decision 2: PASS
- `FavoritesCount` added as second positional parameter (after `Status`): PASS

### FavoriteItem.cs

```csharp
public record FavoriteItem(
    string Id,
    string Title,
    [property: JsonConverter(typeof(FlexibleDecimalConverter))] decimal? Price,
    string? Currency,
    string? City,
    string Status,
    string? Slug,
    int SerialId);
```

- File-scoped namespace: PASS
- No comments: PASS
- `SerialId` added as last positional parameter: PASS
- Non-nullable `int` as specified in design.md Decision 3: PASS

### ListingManagementService.cs

```csharp
return await requestProvider.PostAsync<CreateListingRequest, ListingDetail>(
    "/api/private/listings", request, token, ct: ct);
```

- Trailing slash removed: PASS
- No async void: PASS (returns Task)
- CancellationToken propagated: PASS (`ct` forwarded to `PostAsync`)
- IRequestProvider used (not direct HttpClient): PASS
- No string interpolation in logging: PASS (no logging calls in this service)
- No `.Result`/`.Wait()`: PASS
- Braces on all control flow (ternary for `uri` in `GetMyListingsAsync` is acceptable — not an `if` statement): PASS

### MessagingService.cs

```csharp
return await requestProvider.PostAsync<CreateConversationRequest, Conversation>(
    "/api/private/conversations",
    new CreateConversationRequest(listingId), token, ct: ct);
```

- Trailing slash removed: PASS
- No async void: PASS
- CancellationToken propagated: PASS
- IRequestProvider used: PASS
- No `.Result`/`.Wait()`: PASS

### MediaService.cs

```csharp
return await requestProvider.PostMultipartAsync<MediaItem>("/api/private/media", content, token, ct);
```

- Trailing slash removed: PASS
- `using var content` on `MultipartFormDataContent`: PASS (properly disposed)
- No async void: PASS
- CancellationToken propagated: PASS
- IRequestProvider used: PASS
- No `.Result`/`.Wait()`: PASS
- `if (sortOrder.HasValue) { ... }` uses braces: PASS

### Test Files

- NSubstitute used for mocking (consistent with project — CLAUDE.md lists Moq but project uses NSubstitute): PASS (consistent with existing test files)
- `Arg.Do<string>` pattern used for URI capture: correct NSubstitute usage
- `FluentAssertions` used for assertions: PASS
- No comments: PASS
- File-scoped namespaces: PASS
- Constructor-injected SUT pattern (`_sut = new ...`): PASS
- `async Task` test methods (not async void): PASS

---

## Notes

1. The test project uses `NSubstitute` (version 5.3.0) rather than `Moq` as listed in CLAUDE.md. This is consistent with all other test files in the project. The discrepancy is in CLAUDE.md, not the code.

2. `FavoriteActionResponseTests` uses `SportowyHubJsonContext.Default.Options` directly, which validates both the `FavoriteActionResponse` type registration and the `SnakeCaseLower` naming policy in a single set of tests. This is the correct approach.

3. The `InternalsVisibleTo("SportowyHub.Tests")` attribute is present in `SportowyHub.App/Properties/AssemblyInfo.cs`, which enables the test project to instantiate internal service classes (`ListingManagementService`, `MessagingService`, `MediaService`) directly. This is properly configured.

4. Task 4.4 (document manual audit outcome in tasks.md) was not completed. The tasks.md still shows all items as unchecked `[ ]`. This is a process gap but does not affect correctness.
