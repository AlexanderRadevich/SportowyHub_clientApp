using SportowyHub.Models.Api;
using SportowyHub.Services.Api;

namespace SportowyHub.Services.Moderation;

internal class ModerationService(IRequestProvider requestProvider) : IModerationService
{
    public async Task<ReportResponse> ReportListingAsync(string listingId, string reasonCode, string? comment = null, CancellationToken ct = default)
    {
        return await requestProvider.PostAsync<ReportListingRequest, ReportResponse>(
            "/api/v1/moderation/report",
            new ReportListingRequest(listingId, reasonCode, comment),
            ct: ct);
    }
}
