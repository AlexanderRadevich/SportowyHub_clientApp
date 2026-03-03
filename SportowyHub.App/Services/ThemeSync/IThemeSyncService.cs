using SportowyHub.Models.Api;

namespace SportowyHub.Services.ThemeSync;

public interface IThemeSyncService
{
    Task<ThemePreferences> GetPreferencesAsync(CancellationToken ct = default);
    Task<ThemePreferences> UpdatePreferencesAsync(ThemePreferences preferences, CancellationToken ct = default);
}
