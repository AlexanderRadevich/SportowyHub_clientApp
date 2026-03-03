namespace SportowyHub.Models.Api;

public record DsarRequestItem(
    int Id,
    string Type,
    string Status,
    string RequestedAt,
    string Deadline,
    string? CompletedAt,
    bool IsOverdue);

public record DsarListResponse(List<DsarRequestItem> Items);

public record DsarRequestResponse(
    int RequestId,
    string Type,
    string Status,
    string Deadline,
    string? ResultReference,
    string? Message);
