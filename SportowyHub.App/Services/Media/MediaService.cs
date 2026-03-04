using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;

namespace SportowyHub.Services.Media;

internal class MediaService(IRequestProvider requestProvider, IAuthService authService) : IMediaService
{
    public async Task<MediaItem> UploadAsync(string listingId, Stream fileStream, string fileName, int? sortOrder = null, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync() ?? "";
        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(listingId), "listing_id");
        content.Add(new StreamContent(fileStream), "file", fileName);

        if (sortOrder.HasValue)
        {
            content.Add(new StringContent(sortOrder.Value.ToString()), "sort_order");
        }

        return await requestProvider.PostMultipartAsync<MediaItem>("/api/private/media", content, token, ct);
    }

    public async Task DeleteAsync(int mediaId, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync() ?? "";
        await requestProvider.DeleteAsync($"/api/private/media/{mediaId}", token, ct);
    }
}
