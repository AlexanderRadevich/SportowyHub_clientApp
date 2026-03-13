using SportowyHub.ViewModels;

namespace SportowyHub.Views.Search;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        viewModel.RequestUnfocus += () =>
            MainThread.BeginInvokeOnMainThread(() => SearchEntry.Unfocus());
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SearchEntry.Focus();
        var vm = (SearchViewModel)BindingContext;
        vm.AppearingCommand.Execute(null);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }

    private void OnSearchEntryFocused(object? sender, FocusEventArgs e)
    {
        var vm = (SearchViewModel)BindingContext;
        if (string.IsNullOrWhiteSpace(vm.SearchText) && !vm.HasActiveFilters && vm.HasSearchResults)
        {
            vm.ClearSearchResults();
        }
    }
}
