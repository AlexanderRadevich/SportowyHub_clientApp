using SportowyHub.ViewModels;

namespace SportowyHub.Views.Home;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _viewModel;

    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.CheckAuthCommand.Execute(null);

        if (_viewModel.Listings.Count == 0)
        {
            _viewModel.LoadListingsCommand.Execute(null);
        }
    }
}
