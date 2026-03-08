using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SportowyHub.Views.Home;
using SportowyHub.Views.Search;
using SportowyHub.Views.Favorites;
using SportowyHub.Views.Profile;
using SportowyHub.Views.Auth;
using SportowyHub.Views.Listings;
using SportowyHub.Views.MyListings;
using SportowyHub.ViewModels;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;
using SportowyHub.Services.Favorites;
using SportowyHub.Services.Listings;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.RecentSearches;
using SportowyHub.Services.Dsar;
using SportowyHub.Services.Locale;
using SportowyHub.Services.ListingManagement;
using SportowyHub.Services.Media;
using SportowyHub.Services.Messaging;
using SportowyHub.Services.Moderation;
using SportowyHub.Services.PhoneVerification;
using SportowyHub.Services.Geography;
using SportowyHub.Services.Sections;
using SportowyHub.Services.ThemeSync;
using SportowyHub.Services.Toast;

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

        builder.Services.AddHttpClient("Api", client =>
        {
            client.BaseAddress = new Uri(ApiConfig.BaseUrl);
        });
        builder.Services.AddSingleton<IRequestProvider, RequestProvider>();
        builder.Services.AddSingleton<IReturnUrlService, ReturnUrlService>();
        builder.Services.AddSingleton<INavigationService, ShellNavigationService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IListingsService, ListingsService>();
        builder.Services.AddSingleton<IToastService, ToastService>();
        builder.Services.AddSingleton<IRecentSearchesService, RecentSearchesService>();
        builder.Services.AddSingleton<IFavoritesService, FavoritesService>();
        builder.Services.AddSingleton<ISectionsService, SectionsService>();
        builder.Services.AddSingleton<IGeographyService, GeographyService>();
        builder.Services.AddSingleton<ILocaleService, LocaleService>();
        builder.Services.AddSingleton<IPhoneVerificationService, PhoneVerificationService>();
        builder.Services.AddSingleton<IThemeSyncService, ThemeSyncService>();
        builder.Services.AddSingleton<IListingManagementService, ListingManagementService>();
        builder.Services.AddSingleton<IMediaService, MediaService>();
        builder.Services.AddSingleton<IMessagingService, MessagingService>();
        builder.Services.AddSingleton<IModerationService, ModerationService>();
        builder.Services.AddSingleton<IDsarService, DsarService>();

        // Pages
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<SearchPage>();
        builder.Services.AddTransient<FavoritesPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<AccountProfilePage>();
        builder.Services.AddTransient<EditProfilePage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<EmailVerificationPage>();
        builder.Services.AddTransient<ListingDetailPage>();
        builder.Services.AddTransient<MyListingsPage>();
        builder.Services.AddTransient<CreateEditListingPage>();
        // ViewModels
        builder.Services.AddTransient<SearchViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<EmailVerificationViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();
        builder.Services.AddTransient<AccountProfileViewModel>();
        builder.Services.AddTransient<EditProfileViewModel>();
        builder.Services.AddTransient<FavoritesViewModel>();
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<ListingDetailViewModel>();
        builder.Services.AddTransient<MyListingsViewModel>();
        builder.Services.AddTransient<CreateEditListingViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
        builder.Logging.SetMinimumLevel(LogLevel.Debug);
#endif

        return builder.Build();
    }
}
