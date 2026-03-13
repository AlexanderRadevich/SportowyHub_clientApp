using Microsoft.Extensions.Logging;
using SportowyHub.Models;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;

namespace SportowyHub.Services.Moderation;

internal class ModerationService(IRequestProvider requestProvider, ILogger<ModerationService> logger) : IModerationService
{
    public async Task<Result<ReportResponse>> ReportListingAsync(string listingId, string reasonCode, string? comment = null, CancellationToken ct = default)
    {
        try
        {
            var response = await requestProvider.PostAsync<ReportListingRequest, ReportResponse>(
                "/api/v1/moderation/report",
                new ReportListingRequest(listingId, reasonCode, comment),
                ct: ct);
            return Result<ReportResponse>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to report listing {ListingId}", listingId);
            return Result<ReportResponse>.Failure(ex.Message);
        }
    }
}
