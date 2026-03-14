using CommunityToolkit.Mvvm.ComponentModel;
using SportowyHub.Models.Api;

namespace SportowyHub.Models;

public partial class ListingCardItem(ListingSummary listing, bool isFavorited) : ObservableObject
{
    public ListingSummary Listing { get; } = listing;

    [ObservableProperty]
    public partial bool IsFavorited { get; set; } = isFavorited;
}
