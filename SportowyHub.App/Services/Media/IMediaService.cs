using SportowyHub.Models;
using SportowyHub.Models.Api;

namespace SportowyHub.Services.Media;

public interface IMediaService
{
    Task<Result<MediaItem>> UploadAsync(string listingId, Stream fileStream, string fileName, int? sortOrder = null, CancellationToken ct = default);
    Task<Result<bool>> DeleteAsync(int mediaId, CancellationToken ct = default);
}
