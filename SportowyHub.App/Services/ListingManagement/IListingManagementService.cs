using SportowyHub.Models.Api;

namespace SportowyHub.Services.ListingManagement;

public interface IListingManagementService
{
    Task<MyListingsResponse> GetMyListingsAsync(string? status = null, CancellationToken ct = default);
    Task<ListingDetail> CreateListingAsync(CreateListingRequest request, CancellationToken ct = default);
    Task<UpdateListingResponse> UpdateListingAsync(string id, UpdateListingRequest request, CancellationToken ct = default);
    Task DeleteListingAsync(string id, CancellationToken ct = default);
    Task<UpdateStatusResponse> UpdateStatusAsync(string id, string status, CancellationToken ct = default);
    Task<ResubmitResponse> ResubmitForReviewAsync(string id, CancellationToken ct = default);
}
