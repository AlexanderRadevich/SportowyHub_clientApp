using SportowyHub.ViewModels;

namespace SportowyHub.Views.MyListings;

public partial class MyListingsPage : ContentPage
{
    public MyListingsPage(MyListingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((MyListingsViewModel)BindingContext).AppearingCommand.Execute(null);
    }
}
