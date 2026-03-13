using SportowyHub.Models.Api;
using SportowyHub.Services.Api;

namespace SportowyHub.Services.Locale;

internal class LocaleService(IRequestProvider requestProvider) : ILocaleService
{
    public string TwoLetterLanguageCode =>
        Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

    public async Task<LocaleInfo> GetLocaleInfoAsync(CancellationToken ct = default)
    {
        return await requestProvider.GetAsync<LocaleInfo>("/api/v1/locale", ct: ct);
    }
}
