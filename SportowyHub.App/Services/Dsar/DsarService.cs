using Microsoft.Extensions.Logging;
using SportowyHub.Models;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;

namespace SportowyHub.Services.Dsar;

internal class DsarService(IRequestProvider requestProvider, ITokenProvider authService, ILogger<DsarService> logger) : IDsarService
{
    public async Task<Result<DsarListResponse>> GetRequestsAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
    {
        try
        {
            var token = await authService.GetTokenAsync() ?? "";
            var response = await requestProvider.GetAsync<DsarListResponse>(
                $"/api/private/dsar?limit={limit}&offset={offset}", token, ct);
            return Result<DsarListResponse>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to get DSAR requests");
            return Result<DsarListResponse>.Failure(ex.Message);
        }
    }

    public async Task<Result<DsarRequestResponse>> RequestDataExportAsync(CancellationToken ct = default)
    {
        try
        {
            var token = await authService.GetTokenAsync() ?? "";
            var response = await requestProvider.PostAsync<Dictionary<string, string>, DsarRequestResponse>(
                "/api/private/dsar/export", new(), token, ct: ct);
            return Result<DsarRequestResponse>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to request data export");
            return Result<DsarRequestResponse>.Failure(ex.Message);
        }
    }

    public async Task<Result<DsarRequestResponse>> RequestAccountDeletionAsync(CancellationToken ct = default)
    {
        try
        {
            var token = await authService.GetTokenAsync() ?? "";
            var response = await requestProvider.PostAsync<Dictionary<string, string>, DsarRequestResponse>(
                "/api/private/dsar/delete", new(), token, ct: ct);
            return Result<DsarRequestResponse>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to request account deletion");
            return Result<DsarRequestResponse>.Failure(ex.Message);
        }
    }
}
