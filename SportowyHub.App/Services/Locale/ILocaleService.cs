using SportowyHub.Models.Api;

namespace SportowyHub.Services.Locale;

public interface ILocaleService
{
    string TwoLetterLanguageCode { get; }
    Task<LocaleInfo> GetLocaleInfoAsync(CancellationToken ct = default);
}
