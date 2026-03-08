using CommunityToolkit.Mvvm.ComponentModel;
using SportowyHub.Models.Api;

namespace SportowyHub.Models;

public partial class SearchFilterState : ObservableObject
{
    [ObservableProperty]
    public partial Section? SelectedSection { get; set; }

    [ObservableProperty]
    public partial int? SelectedCategoryId { get; set; }

    [ObservableProperty]
    public partial int? SelectedVoivodeshipId { get; set; }

    [ObservableProperty]
    public partial int? SelectedCityId { get; set; }

    [ObservableProperty]
    public partial string? SelectedLocationLabel { get; set; }

    [ObservableProperty]
    public partial float? PriceMin { get; set; }

    [ObservableProperty]
    public partial float? PriceMax { get; set; }

    [ObservableProperty]
    public partial string? Condition { get; set; }

    [ObservableProperty]
    public partial string? SelectedCategoryName { get; set; }

    [ObservableProperty]
    public partial string? Sort { get; set; }

    [ObservableProperty]
    public partial string? SelectedSortLabel { get; set; }

    public int ActiveFilterCount
    {
        get
        {
            var count = 0;
            if (SelectedSection is not null) { count++; }
            if (SelectedCategoryId.HasValue) { count++; }
            if (SelectedVoivodeshipId.HasValue) { count++; }
            if (PriceMin.HasValue) { count++; }
            if (PriceMax.HasValue) { count++; }
            if (Condition is not null) { count++; }
            if (Sort is not null) { count++; }
            return count;
        }
    }

    public void CopyFrom(SearchFilterState other)
    {
        SelectedSection = other.SelectedSection;
        SelectedCategoryId = other.SelectedCategoryId;
        SelectedVoivodeshipId = other.SelectedVoivodeshipId;
        SelectedCityId = other.SelectedCityId;
        SelectedLocationLabel = other.SelectedLocationLabel;
        PriceMin = other.PriceMin;
        PriceMax = other.PriceMax;
        SelectedCategoryName = other.SelectedCategoryName;
        Condition = other.Condition;
        Sort = other.Sort;
        SelectedSortLabel = other.SelectedSortLabel;
    }

    public void Reset()
    {
        SelectedSection = null;
        SelectedCategoryId = null;
        SelectedVoivodeshipId = null;
        SelectedCityId = null;
        SelectedLocationLabel = null;
        PriceMin = null;
        PriceMax = null;
        SelectedCategoryName = null;
        Condition = null;
        Sort = null;
        SelectedSortLabel = null;
        OnPropertyChanged(nameof(ActiveFilterCount));
    }

    partial void OnSelectedSectionChanged(Section? value) => OnPropertyChanged(nameof(ActiveFilterCount));
    partial void OnSelectedCategoryIdChanged(int? value) => OnPropertyChanged(nameof(ActiveFilterCount));
    partial void OnSelectedVoivodeshipIdChanged(int? value) => OnPropertyChanged(nameof(ActiveFilterCount));
    partial void OnPriceMinChanged(float? value) => OnPropertyChanged(nameof(ActiveFilterCount));
    partial void OnPriceMaxChanged(float? value) => OnPropertyChanged(nameof(ActiveFilterCount));
    partial void OnConditionChanged(string? value) => OnPropertyChanged(nameof(ActiveFilterCount));
    partial void OnSortChanged(string? value) => OnPropertyChanged(nameof(ActiveFilterCount));
}
