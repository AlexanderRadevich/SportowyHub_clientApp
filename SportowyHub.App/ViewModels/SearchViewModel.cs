using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Models.Api;
using SportowyHub.Services.Listings;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.RecentSearches;
using SportowyHub.Services.Toast;

namespace SportowyHub.ViewModels;

public partial class SearchViewModel(
    IListingsService listingsService,
    INavigationService nav,
    IToastService toastService,
    IRecentSearchesService recentSearchesService) : ObservableObject
{
    private const int PageSize = 30;
    private int _searchOffset;
    private int _searchTotal;
    private CancellationTokenSource? _debounceCts;

    [ObservableProperty]
    public partial string SearchText { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsSearching { get; set; }

    [ObservableProperty]
    public partial int TotalResults { get; set; }

    [ObservableProperty]
    public partial bool HasSearchResults { get; set; }

    [ObservableProperty]
    public partial bool ShowNoResults { get; set; }

    public ObservableCollection<string> RecentSearches { get; } = [];

    public ObservableCollection<string> PopularSearches { get; } =
    [
        "Football",
        "Gym equipment",
        "Swimming goggles",
        "Cycling helmet",
        "Protein powder"
    ];

    public ObservableCollection<SearchResultItem> SearchResults { get; } = [];

    partial void OnSearchTextChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            SearchResults.Clear();
            HasSearchResults = false;
            ShowNoResults = false;
            TotalResults = 0;
            _debounceCts?.Cancel();
            return;
        }

        _debounceCts?.Cancel();
        _debounceCts = new CancellationTokenSource();
        _ = PerformDebouncedSearch(value, _debounceCts.Token);
    }

    private async Task PerformDebouncedSearch(string query, CancellationToken ct)
    {
        try
        {
            await Task.Delay(400, ct);
        }
        catch (TaskCanceledException)
        {
            return;
        }

        await ExecuteSearch(query, 0, ct);
    }

    private async Task ExecuteSearch(string query, int offset, CancellationToken ct)
    {
        IsSearching = true;

        try
        {
            var response = await listingsService.SearchAsync(query: query, limit: PageSize, offset: offset, ct: ct);

            if (offset == 0)
            {
                SearchResults.Clear();
            }

            foreach (var item in response.Items)
            {
                SearchResults.Add(item);
            }

            _searchOffset = offset + response.Items.Count;
            _searchTotal = response.Total;
            TotalResults = response.Total;
            HasSearchResults = SearchResults.Count > 0;
            ShowNoResults = SearchResults.Count == 0 && !string.IsNullOrWhiteSpace(SearchText);

            if (offset == 0)
            {
                recentSearchesService.Add(query);
                LoadRecentSearches();
            }
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            await toastService.ShowError(ex.Message);
        }
        finally
        {
            IsSearching = false;
        }
    }

    [RelayCommand]
    private async Task LoadMoreSearchResults(CancellationToken ct)
    {
        if (IsSearching || string.IsNullOrWhiteSpace(SearchText) || _searchOffset >= _searchTotal)
        {
            return;
        }

        await ExecuteSearch(SearchText, _searchOffset, ct);
    }

    [RelayCommand]
    private void ClearSearch()
    {
        SearchText = string.Empty;
    }

    [RelayCommand]
    private void GoBack()
    {
        if (Shell.Current is Shell shell)
        {
            shell.CurrentItem = shell.Items[0].Items[0];
        }
    }

    [RelayCommand]
    private void SelectSuggestion(string suggestion)
    {
        SearchText = suggestion;
    }

    [RelayCommand]
    private async Task GoToListingDetail(SearchResultItem item)
    {
        await nav.GoToAsync($"listing-detail?id={Uri.EscapeDataString(item.Id)}");
    }

    [RelayCommand]
    private void Appearing()
    {
        LoadRecentSearches();
    }

    [RelayCommand]
    private void ClearRecentSearches()
    {
        recentSearchesService.Clear();
        RecentSearches.Clear();
    }

    private void LoadRecentSearches()
    {
        RecentSearches.Clear();
        foreach (var item in recentSearchesService.GetAll())
        {
            RecentSearches.Add(item);
        }
    }
}
