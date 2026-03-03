using SportowyHub.Models.Api;

namespace SportowyHub.Services.Dsar;

public interface IDsarService
{
    Task<DsarListResponse> GetRequestsAsync(int limit = 20, int offset = 0, CancellationToken ct = default);
    Task<DsarRequestResponse> RequestDataExportAsync(CancellationToken ct = default);
    Task<DsarRequestResponse> RequestAccountDeletionAsync(CancellationToken ct = default);
}
