namespace SportowyHub.Services.Api;

public static class ApiConfig
{
#if DEBUG
    private const string DefaultBaseUrl = "https://unduly-hypsometric-agatha.ngrok-free.dev";
    public const string GoogleClientId = "460179961292-f0ekuaisjrl3k4thuin6ic4cdh6ldetq.apps.googleusercontent.com";
#else
    private const string DefaultBaseUrl = "RELEASE_API_URL_NOT_CONFIGURED";
    public const string GoogleClientId = "RELEASE_GOOGLE_CLIENT_ID_NOT_CONFIGURED";
#endif

    public static string BaseUrl
    {
        get
        {
            var url = Preferences.Get("api_base_url", DefaultBaseUrl);
#if !DEBUG
            if (url is "RELEASE_API_URL_NOT_CONFIGURED" or "")
            {
                throw new InvalidOperationException(
                    "API base URL is not configured. Set ApiBaseUrl MSBuild property or configure api_base_url preference.");
            }
#endif
            return url;
        }
    }

    public const string GoogleAuthUrl = "https://accounts.google.com/o/oauth2/v2/auth";
    public const string GoogleTokenUrl = "https://oauth2.googleapis.com/token";

    public const string OAuthCallbackScheme = "com.sportowyHub.app";
    public static string OAuthRedirectUri => $"{OAuthCallbackScheme}:/";
}
