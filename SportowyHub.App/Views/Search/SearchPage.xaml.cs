using SportowyHub.ViewModels;

namespace SportowyHub.Views.Search;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SearchEntry.Focus();
        ((SearchViewModel)BindingContext).AppearingCommand.Execute(null);
    }
}