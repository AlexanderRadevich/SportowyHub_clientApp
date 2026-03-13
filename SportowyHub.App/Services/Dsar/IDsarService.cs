using SportowyHub.Models;
using SportowyHub.Models.Api;

namespace SportowyHub.Services.Dsar;

public interface IDsarService
{
    Task<Result<DsarListResponse>> GetRequestsAsync(int limit = 20, int offset = 0, CancellationToken ct = default);
    Task<Result<DsarRequestResponse>> RequestDataExportAsync(CancellationToken ct = default);
    Task<Result<DsarRequestResponse>> RequestAccountDeletionAsync(CancellationToken ct = default);
}
