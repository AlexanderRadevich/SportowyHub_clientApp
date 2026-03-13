using SportowyHub.Models;
using SportowyHub.Models.Api;

namespace SportowyHub.Services.ListingManagement;

public interface IListingManagementService
{
    Task<Result<MyListingsResponse>> GetMyListingsAsync(string? status = null, CancellationToken ct = default);
    Task<Result<ListingDetail>> GetMyListingAsync(string id, CancellationToken ct = default);
    Task<Result<ListingDetail>> CreateListingAsync(CreateListingRequest request, CancellationToken ct = default);
    Task<Result<UpdateListingResponse>> UpdateListingAsync(string id, UpdateListingRequest request, CancellationToken ct = default);
    Task<Result<bool>> DeleteListingAsync(string id, CancellationToken ct = default);
    Task<Result<UpdateStatusResponse>> UpdateStatusAsync(string id, string status, CancellationToken ct = default);
    Task<Result<ResubmitResponse>> ResubmitForReviewAsync(string id, CancellationToken ct = default);
}
