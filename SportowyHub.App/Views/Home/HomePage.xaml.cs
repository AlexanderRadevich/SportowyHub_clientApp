using System.Collections.Specialized;
using Microsoft.Maui.Controls.Shapes;
using SportowyHub.Models.Api;
using SportowyHub.ViewModels;

namespace SportowyHub.Views.Home;

public partial class HomePage : ContentPage
{
    private const int ConditionChipCount = 3;
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

        _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        _viewModel.Sections.CollectionChanged += OnSectionsChanged;

        if (Application.Current is not null)
        {
            Application.Current.RequestedThemeChanged += OnThemeChanged;
        }

        if (_viewModel.Listings.Count == 0)
        {
            _viewModel.LoadListingsCommand.Execute(null);
        }

        if (_viewModel.Sections.Count == 0)
        {
            _viewModel.LoadSectionsCommand.Execute(null);
        }

        _viewModel.LoadFavoritesCommand.Execute(null);
        UpdateChipStyles();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
        _viewModel.Sections.CollectionChanged -= OnSectionsChanged;

        if (Application.Current is not null)
        {
            Application.Current.RequestedThemeChanged -= OnThemeChanged;
        }
    }

    private void OnThemeChanged(object? sender, AppThemeChangedEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            UpdateChipStyles();
            RebuildSectionChips();
        });
    }

    private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(HomeViewModel.SelectedCondition))
        {
            UpdateChipStyles();
        }
    }

    private void OnSectionsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(RebuildSectionChips);
    }

    private void RebuildSectionChips()
    {
        while (ChipsRow.Children.Count > ConditionChipCount)
        {
            ChipsRow.Children.RemoveAt(ChipsRow.Children.Count - 1);
        }

        foreach (var section in _viewModel.Sections)
        {
            var chip = CreateSectionChip(section);
            ChipsRow.Children.Add(chip);
        }
    }

    private Border CreateSectionChip(Section section)
    {
        var label = new Label
        {
            Text = section.Name,
            FontFamily = "OpenSansSemibold",
            FontSize = 13,
            VerticalOptions = LayoutOptions.Center
        };
        label.SetAppThemeColor(Label.TextColorProperty, GetColor("TextPrimary"), GetColor("TextPrimaryDark"));

        var border = new Border
        {
            Padding = new Thickness(14, 6),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(16) },
            Content = label
        };
        border.SetAppThemeColor(Border.BackgroundColorProperty, GetColor("SearchBarBackground"), GetColor("SearchBarBackgroundDark"));
        border.SetAppTheme(Border.StrokeProperty,
            new SolidColorBrush(GetColor("Border")),
            new SolidColorBrush(GetColor("BorderDark")));

        var tap = new TapGestureRecognizer();
        tap.SetBinding(TapGestureRecognizer.CommandProperty, new Binding(
            nameof(HomeViewModel.GoToFilteredSearchCommand),
            source: _viewModel));
        tap.CommandParameter = section;
        border.GestureRecognizers.Add(tap);

        return border;
    }

    private void UpdateChipStyles()
    {
        var selected = _viewModel.SelectedCondition;
        StyleChip(ChipAll, selected is null);
        StyleChip(ChipNew, selected == "new");
        StyleChip(ChipUsed, selected == "used");
    }

    private void StyleChip(Border chip, bool isSelected)
    {
        if (isSelected)
        {
            chip.SetAppThemeColor(Border.BackgroundColorProperty, GetColor("ChipSelectedBg"), GetColor("ChipSelectedBgDark"));
            if (chip.Content is Label label)
            {
                label.SetAppThemeColor(Label.TextColorProperty, GetColor("ChipSelectedText"), GetColor("ChipSelectedTextDark"));
            }
        }
        else
        {
            chip.SetAppThemeColor(Border.BackgroundColorProperty, GetColor("SearchBarBackground"), GetColor("SearchBarBackgroundDark"));
            if (chip.Content is Label label)
            {
                label.SetAppThemeColor(Label.TextColorProperty, GetColor("TextPrimary"), GetColor("TextPrimaryDark"));
            }
        }
    }

    private static Color GetColor(string key)
    {
        if (Application.Current?.Resources.TryGetValue(key, out var value) == true && value is Color color)
        {
            return color;
        }
        return Colors.Gray;
    }
}
