using Microsoft.Extensions.Logging;
using SportowyHub.Views.Home;
using SportowyHub.Views.Search;
using SportowyHub.Views.Favorites;
using SportowyHub.Views.Profile;
using SportowyHub.Views.Auth;
using SportowyHub.ViewModels;

namespace SportowyHub;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Pages
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<SearchPage>();
        builder.Services.AddTransient<FavoritesPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<LoginPage>();

        // ViewModels
        builder.Services.AddTransient<SearchViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<LoginViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}