using Microsoft.Extensions.Logging;
using SportowyHub.Models;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;

namespace SportowyHub.Services.Media;

internal class MediaService(IRequestProvider requestProvider, ITokenProvider authService, ILogger<MediaService> logger) : IMediaService
{
    public async Task<Result<MediaItem>> UploadAsync(string listingId, Stream fileStream, string fileName, int? sortOrder = null, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(listingId);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        try
        {
            var token = await authService.GetTokenAsync() ?? "";
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(listingId), "listing_id");
            content.Add(new StreamContent(fileStream), "file", fileName);

            if (sortOrder.HasValue)
            {
                content.Add(new StringContent(sortOrder.Value.ToString()), "sort_order");
            }

            var response = await requestProvider.PostMultipartAsync<MediaItem>("/api/private/media", content, token, ct);
            return Result<MediaItem>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to upload media for listing {ListingId}", listingId);
            return Result<MediaItem>.Failure(ex.Message);
        }
    }

    public async Task<Result<bool>> DeleteAsync(int mediaId, CancellationToken ct = default)
    {
        try
        {
            var token = await authService.GetTokenAsync() ?? "";
            await requestProvider.DeleteAsync($"/api/private/media/{mediaId}", token, ct);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to delete media {MediaId}", mediaId);
            return Result<bool>.Failure(ex.Message);
        }
    }
}
