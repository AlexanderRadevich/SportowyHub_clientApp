using Android.App;
using Android.Content.PM;
using SportowyHub.Services.Api;

namespace SportowyHub;

[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
[IntentFilter([Android.Content.Intent.ActionView],
    Categories = [Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable],
    DataScheme = ApiConfig.OAuthCallbackScheme)]
public class WebAuthenticationCallbackActivity : Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity;
