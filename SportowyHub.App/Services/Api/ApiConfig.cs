namespace SportowyHub.Services.Api;

public static class ApiConfig
{
    private const string DefaultBaseUrl = "https://unduly-hypsometric-agatha.ngrok-free.dev";

    public static string BaseUrl =>
        Preferences.Get("api_base_url", DefaultBaseUrl);
}
