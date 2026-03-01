using SportowyHub.ViewModels;

namespace SportowyHub.Views.Listings;

public partial class ListingDetailPage : ContentPage
{
    public ListingDetailPage(ListingDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
