namespace SportowyHub.Services.Api;

public static class ApiConfig
{
    private const string DefaultBaseUrl = "https://unduly-hypsometric-agatha.ngrok-free.dev";

    public static string BaseUrl =>
        Preferences.Get("api_base_url", DefaultBaseUrl);

    public const string GoogleClientId = "460179961292-f0ekuaisjrl3k4thuin6ic4cdh6ldetq.apps.googleusercontent.com";
    public const string GoogleAuthUrl = "https://accounts.google.com/o/oauth2/v2/auth";
    public const string GoogleTokenUrl = "https://oauth2.googleapis.com/token";

    public const string OAuthCallbackScheme = "com.sportowyHub.app";
    public static string OAuthRedirectUri => $"{OAuthCallbackScheme}:/";
}
