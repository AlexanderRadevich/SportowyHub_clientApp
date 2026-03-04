## 1. Fix Trailing Slash on Mutation Endpoints

- [ ] 1.1 In `SportowyHub.App/Services/ListingManagement/ListingManagementService.cs`, change the `CreateListingAsync` URI from `"/api/private/listings/"` to `"/api/private/listings"`
- [ ] 1.2 In `SportowyHub.App/Services/Messaging/MessagingService.cs`, change the `CreateConversationAsync` URI from `"/api/private/conversations/"` to `"/api/private/conversations"`
- [ ] 1.3 In `SportowyHub.App/Services/Media/MediaService.cs`, change the `UploadAsync` URI from `"/api/private/media/"` to `"/api/private/media"`

## 2. Fix FavoriteActionResponse DTO

- [ ] 2.1 In `SportowyHub.App/Models/Api/FavoriteActionResponse.cs`, add `int? FavoritesCount` as a positional parameter to the record (after existing parameters)
- [ ] 2.2 Verify `SportowyHubJsonContext` already registers `FavoriteActionResponse` — no change needed if so

## 3. Fix FavoriteItem DTO

- [ ] 3.1 In `SportowyHub.App/Models/Api/FavoriteItem.cs`, add `int SerialId` as a positional parameter to the record
- [ ] 3.2 Verify `SportowyHubJsonContext` already registers `FavoriteItem` — no change needed if so

## 4. Verification

- [ ] 4.1 Run `dotnet build` and confirm zero errors and zero warnings related to the changed files
- [ ] 4.2 Run `dotnet test` and confirm all existing unit tests pass
- [ ] 4.3 Manually audit all remaining service URI strings (auth, sections, locale, DSAR, phone verification, theme, favorites GET/DELETE) against the backend route definitions and confirm no further trailing slash or path discrepancies exist
- [ ] 4.4 Document the audit outcome as a comment in this tasks.md or as a checklist item marked complete
