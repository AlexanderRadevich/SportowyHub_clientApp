using SportowyHub.Models.Api;

namespace SportowyHub.Services.Media;

public interface IMediaService
{
    Task<MediaItem> UploadAsync(string listingId, Stream fileStream, string fileName, int? sortOrder = null, CancellationToken ct = default);
    Task DeleteAsync(int mediaId, CancellationToken ct = default);
}
