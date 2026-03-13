using SportowyHub.Models;
using SportowyHub.Models.Api;

namespace SportowyHub.Services.Moderation;

public interface IModerationService
{
    Task<Result<ReportResponse>> ReportListingAsync(string listingId, string reasonCode, string? comment = null, CancellationToken ct = default);
}
