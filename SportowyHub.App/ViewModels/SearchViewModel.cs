using System.Collections.ObjectModel;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Models;
using SportowyHub.Models.Api;
using SportowyHub.Services.Geography;
using SportowyHub.Services.Listings;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.RecentSearches;
using SportowyHub.Services.Sections;
using SportowyHub.Services.Toast;
using SportowyHub.Views.Search;

namespace SportowyHub.ViewModels;

public partial class SearchViewModel(
    IListingsService listingsService,
    INavigationService nav,
    IToastService toastService,
    IRecentSearchesService recentSearchesService,
    ISectionsService sectionsService,
    IGeographyService geographyService) : ObservableObject, IQueryAttributable
{
    private const int PageSize = 30;
    private int _searchOffset;
    private int _searchTotal;
    private CancellationTokenSource? _debounceCts;

    internal static Section? PendingSportSection { get; set; }

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

    [ObservableProperty]
    public partial int ActiveFilterCount { get; set; }

    [ObservableProperty]
    public partial bool HasActiveFilters { get; set; }

    public SearchFilterState FilterState { get; } = new();

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

    public ObservableCollection<ActiveFilterChip> ActiveFilterChips { get; } = [];

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("sport", out var sportObj) && sportObj is string sportSlug && !string.IsNullOrWhiteSpace(sportSlug))
        {
            PendingSportSection = new Section(0, sportSlug, sportSlug);
        }
    }

    partial void OnSearchTextChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value) && !HasActiveFilters)
        {
            SearchResults.Clear();
            HasSearchResults = false;
            ShowNoResults = false;
            TotalResults = 0;
            _debounceCts?.Cancel();
            _debounceCts?.Dispose();
            _debounceCts = null;
            return;
        }

        _debounceCts?.Cancel();
        _debounceCts?.Dispose();
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
            var response = await listingsService.SearchAsync(
                query: string.IsNullOrWhiteSpace(query) ? null : query,
                categoryId: FilterState.SelectedCategoryId,
                sport: FilterState.SelectedSection?.Slug,
                cityId: FilterState.SelectedCityId,
                voivodeshipId: FilterState.SelectedVoivodeshipId,
                priceMin: FilterState.PriceMin,
                priceMax: FilterState.PriceMax,
                condition: FilterState.Condition,
                sort: FilterState.Sort,
                limit: PageSize,
                offset: offset,
                ct: ct);

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
            ShowNoResults = SearchResults.Count == 0 && (!string.IsNullOrWhiteSpace(SearchText) || HasActiveFilters);

            if (offset == 0 && !string.IsNullOrWhiteSpace(query))
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
        if (IsSearching || _searchOffset >= _searchTotal)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(SearchText) && !HasActiveFilters)
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
        var priceStr = item.Price?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty;
        var query = $"listing-detail?id={Uri.EscapeDataString(item.Id)}" +
                    $"&title={Uri.EscapeDataString(item.Title)}" +
                    $"&price={Uri.EscapeDataString(priceStr)}" +
                    $"&currency={Uri.EscapeDataString(item.Currency ?? string.Empty)}" +
                    $"&city={Uri.EscapeDataString(item.City ?? string.Empty)}";
        await nav.GoToAsync(query);
    }

    [RelayCommand]
    private async Task Appearing()
    {
        LoadRecentSearches();

        if (PendingSportSection is not null)
        {
            var pending = PendingSportSection;
            PendingSportSection = null;

            if (pending.Id > 0)
            {
                FilterState.SelectedSection = pending;
                UpdateFilterCount();
                await ExecuteSearch(SearchText, 0, CancellationToken.None);
            }
            else
            {
                try
                {
                    var locale = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
                    var response = await sectionsService.GetSectionsAsync(locale);
                    var section = response.Sports.FirstOrDefault(s => s.Slug == pending.Slug);
                    if (section is not null)
                    {
                        FilterState.SelectedSection = section;
                        UpdateFilterCount();
                        await ExecuteSearch(SearchText, 0, CancellationToken.None);
                    }
                }
                catch
                {
                }
            }
        }
    }

    [RelayCommand]
    private async Task OpenFilter()
    {
        var popupVm = new SearchFilterPopupViewModel(
            sectionsService,
            geographyService,
            FilterState);

        var popup = new SearchFilterPopup(popupVm);
        var page = Shell.Current.CurrentPage;
        if (page is null)
        {
            return;
        }

        _ = page.ShowPopupAsync(popup, PopupOptions.Empty);
        var newState = await popup.GetResultAsync();

        if (newState is not null)
        {
            FilterState.CopyFrom(newState);
            UpdateFilterCount();
            await ExecuteSearch(SearchText, 0, CancellationToken.None);
        }
    }

    [RelayCommand]
    private async Task RemoveFilter(string key)
    {
        switch (key)
        {
            case ActiveFilterChip.Keys.Sport:
                FilterState.SelectedSection = null;
                FilterState.SelectedCategoryId = null;
                FilterState.SelectedCategoryName = null;
                break;
            case ActiveFilterChip.Keys.Category:
                FilterState.SelectedCategoryId = null;
                FilterState.SelectedCategoryName = null;
                break;
            case ActiveFilterChip.Keys.Location:
                FilterState.SelectedVoivodeshipId = null;
                FilterState.SelectedCityId = null;
                FilterState.SelectedLocationLabel = null;
                break;
            case ActiveFilterChip.Keys.PriceMin:
                FilterState.PriceMin = null;
                break;
            case ActiveFilterChip.Keys.PriceMax:
                FilterState.PriceMax = null;
                break;
            case ActiveFilterChip.Keys.Condition:
                FilterState.Condition = null;
                break;
            case ActiveFilterChip.Keys.Sort:
                FilterState.Sort = null;
                FilterState.SelectedSortLabel = null;
                break;
        }

        UpdateFilterCount();
        await ExecuteSearch(SearchText, 0, CancellationToken.None);
    }

    private void UpdateFilterCount()
    {
        ActiveFilterCount = FilterState.ActiveFilterCount;
        HasActiveFilters = ActiveFilterCount > 0;
        RebuildFilterChips();
    }

    private void RebuildFilterChips()
    {
        ActiveFilterChips.Clear();

        if (FilterState.SelectedSection is not null)
        {
            ActiveFilterChips.Add(new ActiveFilterChip(ActiveFilterChip.Keys.Sport, FilterState.SelectedSection.Name));
        }

        if (FilterState.SelectedCategoryId.HasValue && FilterState.SelectedCategoryName is not null)
        {
            ActiveFilterChips.Add(new ActiveFilterChip(ActiveFilterChip.Keys.Category, FilterState.SelectedCategoryName));
        }

        if (FilterState.SelectedLocationLabel is not null)
        {
            ActiveFilterChips.Add(new ActiveFilterChip(ActiveFilterChip.Keys.Location, FilterState.SelectedLocationLabel));
        }

        if (FilterState.PriceMin.HasValue)
        {
            ActiveFilterChips.Add(new ActiveFilterChip(ActiveFilterChip.Keys.PriceMin, $"≥ {FilterState.PriceMin.Value:0.##}"));
        }

        if (FilterState.PriceMax.HasValue)
        {
            ActiveFilterChips.Add(new ActiveFilterChip(ActiveFilterChip.Keys.PriceMax, $"≤ {FilterState.PriceMax.Value:0.##}"));
        }

        if (FilterState.Condition is not null)
        {
            var label = FilterState.Condition == "new"
                ? Resources.Strings.AppResources.FilterConditionNew
                : Resources.Strings.AppResources.FilterConditionUsed;
            ActiveFilterChips.Add(new ActiveFilterChip(ActiveFilterChip.Keys.Condition, label));
        }

        if (FilterState.Sort is not null && FilterState.SelectedSortLabel is not null)
        {
            ActiveFilterChips.Add(new ActiveFilterChip(ActiveFilterChip.Keys.Sort, FilterState.SelectedSortLabel));
        }
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
