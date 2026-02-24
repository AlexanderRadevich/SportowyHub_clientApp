using System.Globalization;

namespace SportowyHub
{
    public partial class App : Application
    {
        private static readonly string[] SupportedLanguages = ["pl", "en", "uk", "ru"];

        public App()
        {
            // Apply persisted theme preference before UI loads
            var themePref = Preferences.Get("app_theme", "system");
            UserAppTheme = themePref switch
            {
                "light" => AppTheme.Light,
                "dark" => AppTheme.Dark,
                _ => AppTheme.Unspecified
            };

            // Apply persisted language preference before UI loads
            var langPref = Preferences.Get("app_language", "system");
            if (langPref != "system" && SupportedLanguages.Contains(langPref))
            {
                var culture = new CultureInfo(langPref);
                CultureInfo.CurrentUICulture = culture;
                CultureInfo.CurrentCulture = culture;
            }
            else
            {
                // System mode: detect device language, fall back to Polish if unsupported
                var currentLang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                if (!SupportedLanguages.Contains(currentLang))
                {
                    var polish = new CultureInfo("pl");
                    CultureInfo.CurrentUICulture = polish;
                    CultureInfo.CurrentCulture = polish;
                }
            }

            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}