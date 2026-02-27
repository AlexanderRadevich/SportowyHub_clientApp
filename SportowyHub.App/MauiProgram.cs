using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SportowyHub.Views.Home;
using SportowyHub.Views.Search;
using SportowyHub.Views.Favorites;
using SportowyHub.Views.Profile;
using SportowyHub.Views.Auth;
using SportowyHub.ViewModels;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;

namespace SportowyHub;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureMauiHandlers(handlers =>
            {
#if ANDROID
                Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("CursorColor", (handler, view) =>
                {
                    if (!OperatingSystem.IsAndroidVersionAtLeast(29))
                        return;

                    var color = Application.Current?.RequestedTheme == AppTheme.Dark
                        ? Android.Graphics.Color.ParseColor("#FF3B4D")
                        : Android.Graphics.Color.ParseColor("#DE0F21");
                    handler.PlatformView.TextCursorDrawable?.SetTint(color);
                });
#endif
            });

        // HTTP infrastructure
        builder.Services.AddSingleton(_ => new HttpClient
        {
            BaseAddress = new Uri(ApiConfig.BaseUrl)
        });
        builder.Services.AddSingleton<IRequestProvider, RequestProvider>();
        builder.Services.AddSingleton<IAuthService, AuthService>();

        // Pages
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<SearchPage>();
        builder.Services.AddTransient<FavoritesPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<AccountProfilePage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<EmailVerificationPage>();

        // ViewModels
        builder.Services.AddTransient<SearchViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<EmailVerificationViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();
        builder.Services.AddTransient<AccountProfileViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}