using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;

namespace SportowyHub.Services.ListingManagement;

internal class ListingManagementService(IRequestProvider requestProvider, IAuthService authService) : IListingManagementService
{
    public async Task<MyListingsResponse> GetMyListingsAsync(string? status = null, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync() ?? "";
        var uri = string.IsNullOrWhiteSpace(status)
            ? "/api/private/listings/my"
            : $"/api/private/listings/my?status={Uri.EscapeDataString(status)}";
        return await requestProvider.GetAsync<MyListingsResponse>(uri, token, ct);
    }

    public async Task<ListingDetail> CreateListingAsync(CreateListingRequest request, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync() ?? "";
        return await requestProvider.PostAsync<CreateListingRequest, ListingDetail>(
            "/api/private/listings/", request, token, ct: ct);
    }

    public async Task<UpdateListingResponse> UpdateListingAsync(string id, UpdateListingRequest request, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync() ?? "";
        return await requestProvider.PutAsync<UpdateListingRequest, UpdateListingResponse>(
            $"/api/private/listings/{Uri.EscapeDataString(id)}", request, token, ct);
    }

    public async Task DeleteListingAsync(string id, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync() ?? "";
        await requestProvider.DeleteAsync($"/api/private/listings/{Uri.EscapeDataString(id)}", token, ct);
    }

    public async Task<UpdateStatusResponse> UpdateStatusAsync(string id, string status, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync() ?? "";
        return await requestProvider.PatchAsync<UpdateStatusRequest, UpdateStatusResponse>(
            $"/api/private/listings/{Uri.EscapeDataString(id)}/status",
            new UpdateStatusRequest(status), token, ct);
    }

    public async Task<ResubmitResponse> ResubmitForReviewAsync(string id, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync() ?? "";
        return await requestProvider.PostAsync<Dictionary<string, string>, ResubmitResponse>(
            $"/api/private/listings/{Uri.EscapeDataString(id)}/resubmit-for-review",
            new(), token, ct: ct);
    }
}
