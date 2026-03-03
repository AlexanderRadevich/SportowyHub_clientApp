using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;

namespace SportowyHub.Services.Dsar;

internal class DsarService(IRequestProvider requestProvider, IAuthService authService) : IDsarService
{
    public async Task<DsarListResponse> GetRequestsAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync() ?? "";
        return await requestProvider.GetAsync<DsarListResponse>(
            $"/api/private/dsar?limit={limit}&offset={offset}", token, ct);
    }

    public async Task<DsarRequestResponse> RequestDataExportAsync(CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync() ?? "";
        return await requestProvider.PostAsync<Dictionary<string, string>, DsarRequestResponse>(
            "/api/private/dsar/export", new(), token, ct: ct);
    }

    public async Task<DsarRequestResponse> RequestAccountDeletionAsync(CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync() ?? "";
        return await requestProvider.PostAsync<Dictionary<string, string>, DsarRequestResponse>(
            "/api/private/dsar/delete", new(), token, ct: ct);
    }
}
