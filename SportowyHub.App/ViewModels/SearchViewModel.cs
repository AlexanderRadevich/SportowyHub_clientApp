using System.Collections.ObjectModel;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using SportowyHub.Messages;
using SportowyHub.Models;
using SportowyHub.Models.Api;
using SportowyHub.Services.Geography;
using SportowyHub.Services.Listings;
using SportowyHub.Services.Locale;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.RecentSearches;
using SportowyHub.Services.Sections;
using SportowyHub.Services.Toast;
using SportowyHub.Views.Search;

namespace SportowyHub.ViewModels;

public partial class SearchViewModel : ObservableObject, IQueryAttributable, IDisposable
{
    private const int PageSize = 30;
    private readonly IListingsService _listingsService;
    private readonly INavigationService _nav;
    private readonly IToastService _toastService;
    private readonly IRecentSearchesService _recentSearchesService;
    private readonly ISectionsService _sectionsService;
    private readonly IGeographyService _geographyService;
    private readonly ILocaleService _localeService;
    private readonly ILogger<SearchViewModel> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private int _searchOffset;
    private int _searchTotal;
    private CancellationTokenSource? _debounceCts;
    private Section? _pendingSportSection;

    internal static Section? PendingNavigationSection { get; set; }

    public SearchViewModel(
        IListingsService listingsService,
        INavigationService nav,
        IToastService toastService,
        IRecentSearchesService recentSearchesService,
        ISectionsService sectionsService,
        IGeographyService geographyService,
        ILocaleService localeService,
        ILogger<SearchViewModel> logger,
        ILoggerFactory loggerFactory)
    {
        _listingsService = listingsService;
        _nav = nav;
        _toastService = toastService;
        _recentSearchesService = recentSearchesService;
        _sectionsService = sectionsService;
        _geographyService = geographyService;
        _localeService = localeService;
        _logger = logger;
        _loggerFactory = loggerFactory;

        WeakReferenceMessenger.Default.Register<NavigateToSearchMessage>(this, (r, m) =>
        {
            var self = (SearchViewModel)r;
            var section = m.Value;
            self.FilterState.SelectedSection = section;
            self.UpdateFilterCount();
            _ = self.ExecuteSearch(self.SearchText, 0, CancellationToken.None);
        });
    }

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
        Resources.Strings.AppResources.PopularSearch1,
        Resources.Strings.AppResources.PopularSearch2,
        Resources.Strings.AppResources.PopularSearch3,
        Resources.Strings.AppResources.PopularSearch4,
        Resources.Strings.AppResources.PopularSearch5
    ];

    public ObservableCollection<SearchCardItem> MappedSearchResults { get; } = [];

    public ObservableCollection<ActiveFilterChip> ActiveFilterChips { get; } = [];

    public event Action? RequestUnfocus;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("sport", out var sportObj) && sportObj is string sportSlug && !string.IsNullOrWhiteSpace(sportSlug))
        {
            _pendingSportSection = new Section(0, sportSlug, sportSlug);
        }
    }

    partial void OnSearchTextChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value) && !HasActiveFilters)
        {
            MappedSearchResults.Clear();
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
        catch (TaskCanceledException ex)
        {
            _logger.LogDebug(ex, "Search debounce cancelled for query {Query}", query);
            return;
        }

        await ExecuteSearch(query, 0, ct);
    }

    private async Task ExecuteSearch(string query, int offset, CancellationToken ct)
    {
        IsSearching = true;

        try
        {
            var result = await _listingsService.SearchAsync(
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

            if (!result.IsSuccess)
            {
                await _toastService.ShowError(result.ErrorMessage!);
                return;
            }

            var response = result.Data!;

            if (offset == 0)
            {
                MappedSearchResults.Clear();
            }

            var mapped = response.Items.Select(item =>
            {
                var condition = item.ExtractCondition();
                return new SearchCardItem(
                    item.ToListingSummary(),
                    condition.HasCondition,
                    condition.Text,
                    condition.BadgeColor);
            });

            foreach (var card in mapped)
            {
                MappedSearchResults.Add(card);
            }

            _searchOffset = offset + response.Items.Count;
            _searchTotal = response.Total;
            TotalResults = response.Total;
            HasSearchResults = MappedSearchResults.Count > 0;
            ShowNoResults = MappedSearchResults.Count == 0 && (!string.IsNullOrWhiteSpace(SearchText) || HasActiveFilters);

            if (offset == 0 && !string.IsNullOrWhiteSpace(query))
            {
                _recentSearchesService.Add(query);
                LoadRecentSearches();
            }
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogDebug(ex, "Search request cancelled");
        }
        catch (Exception ex)
        {
            await _toastService.ShowError(ex.Message);
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
    private async Task GoToListingDetail(ListingSummary listing)
    {
        await _nav.GoToListingDetailAsync(listing.Id, listing.Title, listing.Price, listing.Currency, listing.City);
    }

    [RelayCommand]
    private async Task Appearing(CancellationToken ct)
    {
        LoadRecentSearches();

        var pending = PendingNavigationSection ?? _pendingSportSection;
        PendingNavigationSection = null;
        _pendingSportSection = null;

        if (pending is not null)
        {

            if (pending.Id > 0)
            {
                FilterState.SelectedSection = pending;
                UpdateFilterCount();
                await ExecuteSearch(SearchText, 0, ct);
            }
            else
            {
                var locale = _localeService.TwoLetterLanguageCode;
                var sectionsResult = await _sectionsService.GetSectionsAsync(locale, ct);
                if (sectionsResult.IsSuccess)
                {
                    var section = sectionsResult.Data!.Sports.FirstOrDefault(s => s.Slug == pending.Slug);
                    if (section is not null)
                    {
                        FilterState.SelectedSection = section;
                        UpdateFilterCount();
                        await ExecuteSearch(SearchText, 0, ct);
                    }
                }
                else
                {
                    _logger.LogWarning("Failed to resolve sport section {Slug}: {Error}", pending.Slug, sectionsResult.ErrorMessage);
                }
            }
        }
    }

    [RelayCommand]
    private async Task OpenFilter()
    {
        var popupVm = new SearchFilterPopupViewModel(
            _sectionsService,
            _geographyService,
            _localeService,
            _loggerFactory.CreateLogger<SearchFilterPopupViewModel>(),
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
            RequestUnfocus?.Invoke();
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
        RequestUnfocus?.Invoke();
    }

    public void ClearSearchResults()
    {
        MappedSearchResults.Clear();
        HasSearchResults = false;
        ShowNoResults = false;
        TotalResults = 0;
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
        _recentSearchesService.Clear();
        RecentSearches.Clear();
    }

    private void LoadRecentSearches()
    {
        RecentSearches.Clear();
        foreach (var item in _recentSearchesService.GetAll())
        {
            RecentSearches.Add(item);
        }
    }

    private bool _disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _debounceCts?.Cancel();
            _debounceCts?.Dispose();
            _debounceCts = null;
            WeakReferenceMessenger.Default.Unregister<NavigateToSearchMessage>(this);
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
