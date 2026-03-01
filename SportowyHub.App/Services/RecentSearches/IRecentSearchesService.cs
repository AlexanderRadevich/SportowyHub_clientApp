namespace SportowyHub.Services.RecentSearches;

public interface IRecentSearchesService
{
    IReadOnlyList<string> GetAll();
    void Add(string query);
    void Clear();
}
