using SportowyHub.ViewModels;

namespace SportowyHub.Views.MyListings;

public partial class CreateEditListingPage : ContentPage
{
    public CreateEditListingPage(CreateEditListingViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((CreateEditListingViewModel)BindingContext).AppearingCommand.Execute(null);
    }
}
