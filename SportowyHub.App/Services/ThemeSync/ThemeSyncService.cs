using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;

namespace SportowyHub.Services.ThemeSync;

internal class ThemeSyncService(IRequestProvider requestProvider, IAuthService authService) : IThemeSyncService
{
    public async Task<ThemePreferences> GetPreferencesAsync(CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync() ?? "";
        return await requestProvider.GetAsync<ThemePreferences>("/api/private/theme/preferences", token, ct);
    }

    public async Task<ThemePreferences> UpdatePreferencesAsync(ThemePreferences preferences, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync() ?? "";
        return await requestProvider.PatchAsync<ThemePreferences, ThemePreferences>(
            "/api/private/theme/preferences", preferences, token, ct);
    }
}
