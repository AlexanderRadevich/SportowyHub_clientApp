using System.Text.Json;
using SportowyHub.Services.Api;

namespace SportowyHub.Services.RecentSearches;

internal class RecentSearchesService : IRecentSearchesService
{
    private const string PreferencesKey = "recent_searches";
    private const int MaxItems = 10;

    private List<string> _cache = Load();

    public IReadOnlyList<string> GetAll() => _cache;

    public void Add(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return;
        }

        var trimmed = query.Trim();
        _cache.RemoveAll(x => x.Equals(trimmed, StringComparison.OrdinalIgnoreCase));
        _cache.Insert(0, trimmed);

        if (_cache.Count > MaxItems)
        {
            _cache.RemoveRange(MaxItems, _cache.Count - MaxItems);
        }

        Save(_cache);
    }

    public void Clear()
    {
        _cache.Clear();
        Preferences.Remove(PreferencesKey);
    }

    private static List<string> Load()
    {
        var json = Preferences.Get(PreferencesKey, string.Empty);
        if (string.IsNullOrEmpty(json))
        {
            return [];
        }

        return JsonSerializer.Deserialize(json, SportowyHubJsonContext.Default.ListString) ?? [];
    }

    private static void Save(List<string> items)
    {
        var json = JsonSerializer.Serialize(items, SportowyHubJsonContext.Default.ListString);
        Preferences.Set(PreferencesKey, json);
    }
}
