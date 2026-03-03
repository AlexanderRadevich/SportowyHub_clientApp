namespace SportowyHub.Models.Api;

public record ReportListingRequest(string ListingId, string ReasonCode, string? Comment);

public record ReportResponse(int Id, string ListingId, string Type, string Status, string ReasonCode, string CreatedAt);
