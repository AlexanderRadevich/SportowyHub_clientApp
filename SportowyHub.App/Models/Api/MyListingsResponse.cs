namespace SportowyHub.Models.Api;

public record MyListingsResponse(List<MyListingSummary> Listings, int Total);
