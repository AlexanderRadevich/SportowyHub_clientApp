using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Models;
using SportowyHub.Models.Api;
using SportowyHub.Services.Geography;
using SportowyHub.Services.Sections;

namespace SportowyHub.ViewModels;

public partial class SearchFilterPopupViewModel : ObservableObject
{
    private readonly ISectionsService _sectionsService;
    private readonly IGeographyService _geographyService;
    private CancellationTokenSource? _locationDebounceCts;

    public SearchFilterPopupViewModel(
        ISectionsService sectionsService,
        IGeographyService geographyService,
        SearchFilterState currentState)
    {
        _sectionsService = sectionsService;
        _geographyService = geographyService;

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
        var locale = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

        try
        {
            var response = await _sectionsService.GetSectionsAsync(locale, ct);
            Sections.Clear();
            foreach (var section in response.Sports)
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
        catch
        {
        }

        HasSelectedLocation = SelectedLocationLabel is not null;
    }

    private async Task LoadCategoriesAsync(int sectionId, string locale, CancellationToken ct)
    {
        try
        {
            var response = await _sectionsService.GetCategoriesAsync(sectionId, locale, ct);
            Categories.Clear();
            foreach (var category in response.Categories)
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
        catch
        {
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

        var locale = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
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
        catch (TaskCanceledException)
        {
            return;
        }

        try
        {
            var locale = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            var results = await _geographyService.AutocompleteAsync(query, locale, 10, ct);
            LocationResults.Clear();
            foreach (var item in results)
            {
                LocationResults.Add(item);
            }
        }
        catch (TaskCanceledException)
        {
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
}

public record SortOption(string Label, string Value);
