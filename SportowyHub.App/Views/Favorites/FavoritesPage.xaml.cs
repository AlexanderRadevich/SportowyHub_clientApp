using SportowyHub.ViewModels;

namespace SportowyHub.Views.Favorites;

public partial class FavoritesPage : ContentPage
{
    public FavoritesPage(FavoritesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((FavoritesViewModel)BindingContext).AppearingCommand.Execute(null);
    }
}
