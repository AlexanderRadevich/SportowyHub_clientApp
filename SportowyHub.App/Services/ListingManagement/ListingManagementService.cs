using Microsoft.Extensions.Logging;
using SportowyHub.Models;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;

namespace SportowyHub.Services.ListingManagement;

internal class ListingManagementService(IRequestProvider requestProvider, ITokenProvider authService, ILogger<ListingManagementService> logger) : IListingManagementService
{
    public async Task<Result<MyListingsResponse>> GetMyListingsAsync(string? status = null, CancellationToken ct = default)
    {
        try
        {
            var token = await authService.GetTokenAsync() ?? "";
            var uri = string.IsNullOrWhiteSpace(status)
                ? "/api/private/listings/my"
                : $"/api/private/listings/my?status={Uri.EscapeDataString(status)}";
            var response = await requestProvider.GetAsync<MyListingsResponse>(uri, token, ct);
            return Result<MyListingsResponse>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to get my listings");
            return Result<MyListingsResponse>.Failure(ex.Message);
        }
    }

    public async Task<Result<ListingDetail>> GetMyListingAsync(string id, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        try
        {
            var token = await authService.GetTokenAsync() ?? "";
            var response = await requestProvider.GetAsync<ListingDetail>(
                $"/api/private/listings/{Uri.EscapeDataString(id)}", token, ct);
            return Result<ListingDetail>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to get my listing {ListingId}", id);
            return Result<ListingDetail>.Failure(ex.Message);
        }
    }

    public async Task<Result<ListingDetail>> CreateListingAsync(CreateListingRequest request, CancellationToken ct = default)
    {
        try
        {
            var token = await authService.GetTokenAsync() ?? "";
            var response = await requestProvider.PostAsync<CreateListingRequest, ListingDetail>(
                "/api/private/listings", request, token, ct: ct);
            return Result<ListingDetail>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to create listing");
            return Result<ListingDetail>.Failure(ex.Message);
        }
    }

    public async Task<Result<UpdateListingResponse>> UpdateListingAsync(string id, UpdateListingRequest request, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        try
        {
            var token = await authService.GetTokenAsync() ?? "";
            var response = await requestProvider.PutAsync<UpdateListingRequest, UpdateListingResponse>(
                $"/api/private/listings/{Uri.EscapeDataString(id)}", request, token, ct);
            return Result<UpdateListingResponse>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to update listing {ListingId}", id);
            return Result<UpdateListingResponse>.Failure(ex.Message);
        }
    }

    public async Task<Result<bool>> DeleteListingAsync(string id, CancellationToken ct = default)
    {
        try
        {
            var token = await authService.GetTokenAsync() ?? "";
            await requestProvider.DeleteAsync($"/api/private/listings/{Uri.EscapeDataString(id)}", token, ct);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to delete listing {ListingId}", id);
            return Result<bool>.Failure(ex.Message);
        }
    }

    public async Task<Result<UpdateStatusResponse>> UpdateStatusAsync(string id, string status, CancellationToken ct = default)
    {
        try
        {
            var token = await authService.GetTokenAsync() ?? "";
            var response = await requestProvider.PatchAsync<UpdateStatusRequest, UpdateStatusResponse>(
                $"/api/private/listings/{Uri.EscapeDataString(id)}/status",
                new UpdateStatusRequest(status), token, ct);
            return Result<UpdateStatusResponse>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to update status for listing {ListingId}", id);
            return Result<UpdateStatusResponse>.Failure(ex.Message);
        }
    }

    public async Task<Result<ResubmitResponse>> ResubmitForReviewAsync(string id, CancellationToken ct = default)
    {
        try
        {
            var token = await authService.GetTokenAsync() ?? "";
            var response = await requestProvider.PostAsync<Dictionary<string, string>, ResubmitResponse>(
                $"/api/private/listings/{Uri.EscapeDataString(id)}/resubmit-for-review",
                new(), token, ct: ct);
            return Result<ResubmitResponse>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to resubmit listing {ListingId} for review", id);
            return Result<ResubmitResponse>.Failure(ex.Message);
        }
    }
}
