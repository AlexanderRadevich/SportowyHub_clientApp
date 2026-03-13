using System.Collections.Concurrent;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace SportowyHub.Services.Toast;

public class ToastService : IToastService
{
    private static readonly TimeSpan SuccessDuration = TimeSpan.FromSeconds(6);
    private static readonly ConcurrentDictionary<View, View?> _originalContentMap = new();

    public Task ShowError(string message)
    {
        return MainThread.InvokeOnMainThreadAsync(() =>
        {
            var page = GetCurrentPage();
            if (page is null)
            {
                return Task.CompletedTask;
            }

            return ShowTopNotification(page, message, Color.FromArgb("#DC2626"));
        });
    }

    public Task ShowSuccess(string message)
    {
        return MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var snackbar = Snackbar.Make(
                message,
                duration: SuccessDuration,
                visualOptions: new SnackbarOptions
                {
                    BackgroundColor = Color.FromArgb("#16A34A"),
                    TextColor = Colors.White,
                    CornerRadius = 8,
                    Font = Microsoft.Maui.Font.SystemFontOfSize(14)
                });

            await snackbar.Show();
        });
    }

    private static Page? GetCurrentPage()
    {
        if (Application.Current?.Windows.Count is null or 0)
        {
            return null;
        }

        var window = Application.Current.Windows[0];
        var page = window.Page;

        if (page is Shell shell)
        {
            page = shell.CurrentPage;
        }

        if (page is NavigationPage navPage)
        {
            page = navPage.CurrentPage;
        }

        return page;
    }

    private static async Task ShowTopNotification(Page page, string message, Color backgroundColor)
    {
        var label = new Label
        {
            Text = message,
            TextColor = Colors.White,
            FontSize = 14,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            LineBreakMode = LineBreakMode.WordWrap,
            MaxLines = 3
        };

        var border = new Border
        {
            Content = label,
            BackgroundColor = backgroundColor,
            Stroke = Colors.Transparent,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 8 },
            Padding = new Thickness(16, 12),
            Margin = new Thickness(16, 8, 16, 0),
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Start,
            Opacity = 0,
            TranslationY = -20,
            ZIndex = 999,
            Shadow = new Shadow
            {
                Brush = new SolidColorBrush(Colors.Black),
                Offset = new Point(0, 2),
                Radius = 8,
                Opacity = 0.3f
            }
        };

        var overlay = new Grid
        {
            InputTransparent = true,
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Fill,
            Padding = new Thickness(0, 48, 0, 0),
            ZIndex = 999,
            Children = { border }
        };

        AddOverlayToPage(page, overlay);

        await Task.WhenAll(
            border.FadeToAsync(1, 250, Easing.CubicOut),
            border.TranslateToAsync(0, 0, 250, Easing.CubicOut));

        await Task.Delay(4000);

        await Task.WhenAll(
            border.FadeToAsync(0, 250, Easing.CubicIn),
            border.TranslateToAsync(0, -20, 250, Easing.CubicIn));

        RemoveOverlayFromPage(page, overlay);
    }

    private static void AddOverlayToPage(Page page, View overlay)
    {
        if (page is not ContentPage contentPage)
        {
            return;
        }

        if (contentPage.Content is Grid grid)
        {
            grid.Children.Add(overlay);
        }
        else if (contentPage.Content is Layout layout)
        {
            layout.Children.Add(overlay);
        }
        else
        {
            var existingContent = contentPage.Content;
            _originalContentMap[overlay] = existingContent;
            var wrapper = new Grid { Children = { existingContent, overlay } };
            contentPage.Content = wrapper;
        }
    }

    private static void RemoveOverlayFromPage(Page page, View overlay)
    {
        if (page is not ContentPage contentPage)
        {
            return;
        }

        if (_originalContentMap.TryRemove(overlay, out var originalContent))
        {
            if (contentPage.Content is Grid wrapper && wrapper.Children.Contains(overlay))
            {
                wrapper.Children.Remove(overlay);
                contentPage.Content = originalContent;
            }

            return;
        }

        if (contentPage.Content is Grid grid)
        {
            grid.Children.Remove(overlay);
        }
        else if (contentPage.Content is Layout layout)
        {
            layout.Children.Remove(overlay);
        }
    }
}
