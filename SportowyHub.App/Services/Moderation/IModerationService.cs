using SportowyHub.Models.Api;

namespace SportowyHub.Services.Moderation;

public interface IModerationService
{
    Task<ReportResponse> ReportListingAsync(string listingId, string reasonCode, string? comment = null, CancellationToken ct = default);
}
