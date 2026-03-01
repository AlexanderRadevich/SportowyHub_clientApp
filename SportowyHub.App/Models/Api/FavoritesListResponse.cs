namespace SportowyHub.Models.Api;

public record FavoritesListResponse(
    List<FavoriteItem> Items,
    int Total,
    int Page,
    int PerPage,
    int Pages);
