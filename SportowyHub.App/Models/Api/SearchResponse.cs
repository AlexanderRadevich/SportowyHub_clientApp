namespace SportowyHub.Models.Api;

public record SearchResponse(
    List<SearchResultItem> Items,
    int Total,
    int Limit,
    int Offset);
