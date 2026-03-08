using System.Windows.Input;
using SportowyHub.Models.Api;

namespace SportowyHub.Controls;

public partial class ListingCardView : ContentView
{
    public static readonly BindableProperty ListingProperty =
        BindableProperty.Create(nameof(Listing), typeof(ListingSummary), typeof(ListingCardView));

    public static readonly BindableProperty IsFavoritedProperty =
        BindableProperty.Create(nameof(IsFavorited), typeof(bool), typeof(ListingCardView));

    public static readonly BindableProperty TapCommandProperty =
        BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(ListingCardView));

    public static readonly BindableProperty ToggleFavoriteCommandProperty =
        BindableProperty.Create(nameof(ToggleFavoriteCommand), typeof(ICommand), typeof(ListingCardView));

    public static readonly BindableProperty ConditionTextProperty =
        BindableProperty.Create(nameof(ConditionText), typeof(string), typeof(ListingCardView));

    public static readonly BindableProperty HasConditionProperty =
        BindableProperty.Create(nameof(HasCondition), typeof(bool), typeof(ListingCardView));

    public static readonly BindableProperty ConditionBadgeColorProperty =
        BindableProperty.Create(nameof(ConditionBadgeColor), typeof(Color), typeof(ListingCardView), Colors.Black);

    public ListingSummary? Listing
    {
        get => (ListingSummary?)GetValue(ListingProperty);
        set => SetValue(ListingProperty, value);
    }

    public bool IsFavorited
    {
        get => (bool)GetValue(IsFavoritedProperty);
        set => SetValue(IsFavoritedProperty, value);
    }

    public ICommand? TapCommand
    {
        get => (ICommand?)GetValue(TapCommandProperty);
        set => SetValue(TapCommandProperty, value);
    }

    public ICommand? ToggleFavoriteCommand
    {
        get => (ICommand?)GetValue(ToggleFavoriteCommandProperty);
        set => SetValue(ToggleFavoriteCommandProperty, value);
    }

    public string? ConditionText
    {
        get => (string?)GetValue(ConditionTextProperty);
        set => SetValue(ConditionTextProperty, value);
    }

    public bool HasCondition
    {
        get => (bool)GetValue(HasConditionProperty);
        set => SetValue(HasConditionProperty, value);
    }

    public Color ConditionBadgeColor
    {
        get => (Color)GetValue(ConditionBadgeColorProperty);
        set => SetValue(ConditionBadgeColorProperty, value);
    }

    public ListingCardView()
    {
        InitializeComponent();
    }
}
