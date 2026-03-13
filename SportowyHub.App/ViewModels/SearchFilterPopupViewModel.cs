using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SportowyHub.Models;
using SportowyHub.Models.Api;
using SportowyHub.Services.Geography;
using SportowyHub.Services.Locale;
using SportowyHub.Services.Sections;

namespace SportowyHub.ViewModels;

public partial class SearchFilterPopupViewModel : ObservableObject, IDisposable
{
    private readonly ISectionsService _sectionsService;
    private readonly IGeographyService _geographyService;
    private readonly ILocaleService _localeService;
    private readonly ILogger<SearchFilterPopupViewModel> _logger;
    private CancellationTokenSource? _locationDebounceCts;

    public SearchFilterPopupViewModel(
        ISectionsService sectionsService,
        IGeographyService geographyService,
        ILocaleService localeService,
        ILogger<SearchFilterPopupViewModel> logger,
        SearchFilterState currentState)
    {
        _sectionsService = sectionsService;
        _geographyService = geographyService;
        _localeService = localeService;
        _logger = logger;

        SelectedSection = currentState.SelectedSection;
        _selectedCategoryId = currentState.SelectedCategoryId;
        SelectedVoivodeshipId = currentState.SelectedVoivodeshipId;
        SelectedCityId = currentState.SelectedCityId;
        SelectedLocationLabel = currentState.SelectedLocationLabel;
        PriceMinText = currentState.PriceMin?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
        PriceMaxText = currentState.PriceMax?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
        Condition = currentState.Condition;
        Sort = currentState.Sort;

        SortOptions =
        [
            new SortOption(Resources.Strings.AppResources.FilterSortNewest, "newest"),
            new SortOption(Resources.Strings.AppResources.FilterSortPriceAsc, "price_asc"),
            new SortOption(Resources.Strings.AppResources.FilterSortPriceDesc, "price_desc"),
        ];

        if (Sort is not null)
        {
            SelectedSortOption = SortOptions.FirstOrDefault(o => o.Value == Sort);
        }
    }

    public ObservableCollection<Section> Sections { get; } = [];
    public ObservableCollection<Category> Categories { get; } = [];
    public ObservableCollection<GeographyAutocompleteItem> LocationResults { get; } = [];
    public List<SortOption> SortOptions { get; }

    [ObservableProperty]
    public partial Section? SelectedSection { get; set; }

    private int? _selectedCategoryId;

    [ObservableProperty]
    public partial Category? SelectedCategory { get; set; }

    [ObservableProperty]
    public partial bool IsCategoryEnabled { get; set; }

    [ObservableProperty]
    public partial string CategoryPlaceholder { get; set; } = Resources.Strings.AppResources.FilterCategoryPlaceholder;

    [ObservableProperty]
    public partial int? SelectedVoivodeshipId { get; set; }

    [ObservableProperty]
    public partial int? SelectedCityId { get; set; }

    [ObservableProperty]
    public partial string? SelectedLocationLabel { get; set; }

    [ObservableProperty]
    public partial string LocationQuery { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool HasSelectedLocation { get; set; }

    [ObservableProperty]
    public partial string PriceMinText { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string PriceMaxText { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string? Condition { get; set; }

    [ObservableProperty]
    public partial string? Sort { get; set; }

    [ObservableProperty]
    public partial SortOption? SelectedSortOption { get; set; }

    public bool IsConditionNew => Condition == "new";
    public bool IsConditionUsed => Condition == "used";

    public event Action<SearchFilterState>? Applied;

    public async Task LoadDataAsync(CancellationToken ct = default)
    {
        var locale = _localeService.TwoLetterLanguageCode;

        var result = await _sectionsService.GetSectionsAsync(locale, ct);
        if (result.IsSuccess)
        {
            Sections.Clear();
            foreach (var section in result.Data!.Sports)
            {
                Sections.Add(section);
            }

            if (SelectedSection is not null)
            {
                SelectedSection = Sections.FirstOrDefault(s => s.Id == SelectedSection.Id);
                if (SelectedSection is not null)
                {
                    await LoadCategoriesAsync(SelectedSection.Id, locale, ct);
                }
            }
        }
        else
        {
            _logger.LogWarning("Failed to load filter sections: {Error}", result.ErrorMessage);
        }

        HasSelectedLocation = SelectedLocationLabel is not null;
    }

    private async Task LoadCategoriesAsync(int sectionId, string locale, CancellationToken ct)
    {
        var result = await _sectionsService.GetCategoriesAsync(sectionId, locale, ct);
        if (result.IsSuccess)
        {
            Categories.Clear();
            foreach (var category in result.Data!.Categories)
            {
                Categories.Add(category);
            }

            IsCategoryEnabled = true;
            CategoryPlaceholder = Resources.Strings.AppResources.FilterCategory;

            if (_selectedCategoryId.HasValue)
            {
                SelectedCategory = Categories.FirstOrDefault(c => c.Id == _selectedCategoryId.Value);
                _selectedCategoryId = null;
            }
        }
        else
        {
            _logger.LogWarning("Failed to load categories for section {SectionId}: {Error}", sectionId, result.ErrorMessage);
            IsCategoryEnabled = false;
        }
    }

    partial void OnSelectedSectionChanged(Section? value)
    {
        SelectedCategory = null;
        Categories.Clear();

        if (value is null)
        {
            IsCategoryEnabled = false;
            CategoryPlaceholder = Resources.Strings.AppResources.FilterCategoryPlaceholder;
            return;
        }

        var locale = _localeService.TwoLetterLanguageCode;
        _ = LoadCategoriesAsync(value.Id, locale, CancellationToken.None);
    }

    partial void OnLocationQueryChanged(string value)
    {
        _locationDebounceCts?.Cancel();
        _locationDebounceCts?.Dispose();

        if (string.IsNullOrWhiteSpace(value) || value.Length < 2)
        {
            _locationDebounceCts = null;
            LocationResults.Clear();
            return;
        }

        _locationDebounceCts = new CancellationTokenSource();
        _ = PerformLocationSearchAsync(value, _locationDebounceCts.Token);
    }

    private async Task PerformLocationSearchAsync(string query, CancellationToken ct)
    {
        try
        {
            await Task.Delay(300, ct);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogDebug(ex, "Location search debounce cancelled for query {Query}", query);
            return;
        }

        var locale = _localeService.TwoLetterLanguageCode;
        var result = await _geographyService.AutocompleteAsync(query, locale, 10, ct);
        if (result.IsSuccess)
        {
            LocationResults.Clear();
            foreach (var item in result.Data!)
            {
                LocationResults.Add(item);
            }
        }
    }

    [RelayCommand]
    private void SelectLocation(GeographyAutocompleteItem item)
    {
        if (item.Type == "separator")
        {
            return;
        }

        SelectedVoivodeshipId = item.VoivodeshipId;
        SelectedCityId = item.Type == "city" ? item.CityId : null;
        SelectedLocationLabel = item.Type == "city" ? item.Label : item.Name;
        HasSelectedLocation = true;
        LocationQuery = string.Empty;
        LocationResults.Clear();
    }

    [RelayCommand]
    private void ClearLocation()
    {
        SelectedVoivodeshipId = null;
        SelectedCityId = null;
        SelectedLocationLabel = null;
        HasSelectedLocation = false;
        LocationQuery = string.Empty;
        LocationResults.Clear();
    }

    partial void OnConditionChanged(string? value)
    {
        OnPropertyChanged(nameof(IsConditionNew));
        OnPropertyChanged(nameof(IsConditionUsed));
    }

    [RelayCommand]
    private void ToggleCondition(string value)
    {
        Condition = Condition == value ? null : value;
    }

    partial void OnSelectedSortOptionChanged(SortOption? value)
    {
        Sort = value?.Value;
    }

    [RelayCommand]
    private void Reset()
    {
        SelectedSection = null;
        SelectedCategory = null;
        SelectedVoivodeshipId = null;
        SelectedCityId = null;
        SelectedLocationLabel = null;
        HasSelectedLocation = false;
        LocationQuery = string.Empty;
        LocationResults.Clear();
        PriceMinText = string.Empty;
        PriceMaxText = string.Empty;
        Condition = null;
        Sort = null;
        SelectedSortOption = null;

        Applied?.Invoke(new SearchFilterState());
    }

    [RelayCommand]
    private void Apply()
    {
        var state = new SearchFilterState
        {
            SelectedSection = SelectedSection,
            SelectedCategoryId = SelectedCategory?.Id,
            SelectedCategoryName = SelectedCategory?.Name,
            SelectedVoivodeshipId = SelectedVoivodeshipId,
            SelectedCityId = SelectedCityId,
            SelectedLocationLabel = SelectedLocationLabel,
            PriceMin = float.TryParse(PriceMinText, CultureInfo.InvariantCulture, out var pMin) ? pMin : null,
            PriceMax = float.TryParse(PriceMaxText, CultureInfo.InvariantCulture, out var pMax) ? pMax : null,
            Condition = Condition,
            Sort = Sort,
            SelectedSortLabel = SelectedSortOption?.Label,
        };

        Applied?.Invoke(state);
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
            _locationDebounceCts?.Cancel();
            _locationDebounceCts?.Dispose();
            _locationDebounceCts = null;
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

public record SortOption(string Label, string Value);
