namespace SportowyHub.Models.Api;

public record ListingsResponse(
    List<ListingSummary> Listings,
    int Total);
