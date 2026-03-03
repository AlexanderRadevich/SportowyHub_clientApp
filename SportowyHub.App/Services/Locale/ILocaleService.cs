using SportowyHub.Models.Api;

namespace SportowyHub.Services.Locale;

public interface ILocaleService
{
    Task<LocaleInfo> GetLocaleInfoAsync(CancellationToken ct = default);
}
