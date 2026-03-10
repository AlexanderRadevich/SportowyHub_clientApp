using System.Collections.Specialized;
using SportowyHub.ViewModels;

namespace SportowyHub.Views.Search;

public partial class SearchPage : ContentPage
{
    private double _cardWidth;

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
        vm.MappedSearchResults.CollectionChanged -= OnMappedResultsChanged;
        vm.MappedSearchResults.CollectionChanged += OnMappedResultsChanged;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ((SearchViewModel)BindingContext).MappedSearchResults.CollectionChanged -= OnMappedResultsChanged;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (width > 0)
        {
            var padding = 32;
            var spacing = 10;
            _cardWidth = (width - padding - spacing) / 2;
            UpdateCardWidths();
        }
    }

    private void OnMappedResultsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action is NotifyCollectionChangedAction.Add && e.NewItems is not null)
        {
            MainThread.BeginInvokeOnMainThread(() => SetWidthForNewChildren(e.NewStartingIndex, e.NewItems.Count));
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(UpdateCardWidths);
        }
    }

    private void SetWidthForNewChildren(int startIndex, int count)
    {
        if (_cardWidth <= 0)
        {
            return;
        }

        var children = ResultsGrid.Children;
        for (var i = startIndex; i < startIndex + count && i < children.Count; i++)
        {
            if (children[i] is View view)
            {
                view.WidthRequest = _cardWidth;
            }
        }
    }

    private void UpdateCardWidths()
    {
        if (_cardWidth <= 0)
        {
            return;
        }

        foreach (var child in ResultsGrid.Children)
        {
            if (child is View view)
            {
                view.WidthRequest = _cardWidth;
            }
        }
    }

    private void OnSearchEntryFocused(object? sender, FocusEventArgs e)
    {
        var vm = (SearchViewModel)BindingContext;
        if (string.IsNullOrWhiteSpace(vm.SearchText) && !vm.HasActiveFilters && vm.HasSearchResults)
        {
            vm.ClearSearchResults();
        }
    }

    private void OnResultsScrolled(object? sender, ScrolledEventArgs e)
    {
        if (sender is not ScrollView scrollView)
        {
            return;
        }

        var vm = (SearchViewModel)BindingContext;
        if (vm.LoadMoreSearchResultsCommand.IsRunning)
        {
            return;
        }

        var contentHeight = scrollView.ContentSize.Height;
        var viewportHeight = scrollView.Height;
        if (contentHeight <= 0 || viewportHeight <= 0)
        {
            return;
        }

        if (e.ScrollY + viewportHeight + 200 >= contentHeight)
        {
            vm.LoadMoreSearchResultsCommand.Execute(null);
        }
    }
}
