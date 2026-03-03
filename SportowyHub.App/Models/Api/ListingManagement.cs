namespace SportowyHub.Models.Api;

public record CreateListingRequest(
    int CategoryId,
    string Title,
    string Description,
    decimal? Price,
    string? Currency,
    int CityId,
    int VoivodeshipId,
    double? LocationLatitude,
    double? LocationLongitude,
    Dictionary<string, string>? Attributes,
    string? ContentLocale);

public record UpdateListingRequest(
    int? CategoryId,
    string? Title,
    string? Description,
    decimal? Price,
    string? Currency,
    int? CityId,
    int? VoivodeshipId,
    double? LocationLatitude,
    double? LocationLongitude,
    Dictionary<string, string>? Attributes,
    string? ContentLocale);

public record UpdateListingResponse(string Message);

public record UpdateStatusRequest(string Status);

public record UpdateStatusResponse(string Message, string Status);

public record ResubmitResponse(string Message, string ListingId, string Status);
