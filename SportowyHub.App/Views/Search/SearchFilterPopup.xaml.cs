using CommunityToolkit.Maui.Views;
using SportowyHub.Models;
using SportowyHub.ViewModels;

namespace SportowyHub.Views.Search;

public partial class SearchFilterPopup : Popup
{
    private readonly TaskCompletionSource<SearchFilterState?> _tcs = new();

    public SearchFilterPopup(SearchFilterPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        viewModel.Applied += state =>
        {
            _tcs.TrySetResult(state);
            _ = CloseAsync();
        };

        Closed += (_, _) => _tcs.TrySetResult(null);
    }

    public Task<SearchFilterState?> GetResultAsync() => _tcs.Task;
}
