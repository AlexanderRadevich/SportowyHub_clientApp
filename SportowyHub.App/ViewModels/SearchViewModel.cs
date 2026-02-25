using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SportowyHub.ViewModels;

public partial class SearchViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string SearchText { get; set; }

    public ObservableCollection<string> RecentSearches { get; } =
    [
        "Running shoes",
        "Basketball",
        "Yoga mat",
        "Dumbbells",
        "Tennis racket"
    ];

    public ObservableCollection<string> PopularSearches { get; } =
    [
        "Football",
        "Gym equipment",
        "Swimming goggles",
        "Cycling helmet",
        "Protein powder"
    ];

    [RelayCommand]
    private void ClearSearch()
    {
        SearchText = string.Empty;
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private void SelectSuggestion(string suggestion)
    {
        SearchText = suggestion;
    }
}
