using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace SportowyHub.Services.Toast;

public class ToastService : IToastService
{
    private static readonly TimeSpan Duration = TimeSpan.FromSeconds(6);

    public async Task ShowError(string message)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var snackbar = Snackbar.Make(
                message,
                duration: Duration,
                visualOptions: new SnackbarOptions
                {
                    BackgroundColor = Color.FromArgb("#DC2626"),
                    TextColor = Colors.White,
                    CornerRadius = 8,
                    Font = Microsoft.Maui.Font.SystemFontOfSize(14)
                });

            await snackbar.Show();
        });
    }

    public async Task ShowSuccess(string message)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var snackbar = Snackbar.Make(
                message,
                duration: Duration,
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
}
