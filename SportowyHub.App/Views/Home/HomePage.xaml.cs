namespace SportowyHub.Views.Home;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    private async void OnSearchBarTapped(object? sender, TappedEventArgs e)
    {
        // Navigate to the Search tab (index 1)
        if (Shell.Current is Shell shell)
        {
            shell.CurrentItem = shell.Items[0].Items[1];
        }
    }
}