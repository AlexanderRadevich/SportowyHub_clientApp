using System.Text.Json;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Models.Api;
using SportowyHub.Resources.Strings;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.Toast;

namespace SportowyHub.ViewModels;

public partial class EditProfileViewModel(
    IAuthService authService,
    INavigationService nav,
    IToastService toastService) : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    public partial string FirstName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string LastName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Phone { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool NotificationsEnabled { get; set; }

    [ObservableProperty]
    public partial string QuietHoursStart { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string QuietHoursEnd { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial string GeneralError { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string PhoneError { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string QuietHoursStartError { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string QuietHoursEndError { get; set; } = string.Empty;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("profile", out var profileObj) && profileObj is string json)
        {
            var profile = JsonSerializer.Deserialize(json, SportowyHubJsonContext.Default.UserProfile);
            if (profile is null) return;

            FirstName = profile.Account?.FirstName ?? string.Empty;
            LastName = profile.Account?.LastName ?? string.Empty;
            Phone = profile.Phone ?? string.Empty;
            NotificationsEnabled = profile.Account?.NotificationsEnabled ?? false;
            QuietHoursStart = profile.Account?.QuietHoursStart ?? string.Empty;
            QuietHoursEnd = profile.Account?.QuietHoursEnd ?? string.Empty;
        }
    }

    partial void OnQuietHoursStartChanged(string value) => ValidateQuietHours(value, v => QuietHoursStartError = v);
    partial void OnQuietHoursEndChanged(string value) => ValidateQuietHours(value, v => QuietHoursEndError = v);

    private static void ValidateQuietHours(string value, Action<string> setError)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            setError(string.Empty);
            return;
        }

        setError(TimeRegex().IsMatch(value) ? string.Empty : AppResources.EditProfileInvalidTime);
    }

    private bool CanSave() => !IsLoading;

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task Save(CancellationToken ct)
    {
        GeneralError = string.Empty;
        PhoneError = string.Empty;

        if (!string.IsNullOrWhiteSpace(QuietHoursStartError) || !string.IsNullOrWhiteSpace(QuietHoursEndError))
        {
            return;
        }

        IsLoading = true;

        try
        {
            var request = new UpdateProfileRequest(
                Phone: string.IsNullOrWhiteSpace(Phone) ? null : Phone,
                Account: new UpdateProfileAccountRequest(
                    FirstName: string.IsNullOrWhiteSpace(FirstName) ? null : FirstName,
                    LastName: string.IsNullOrWhiteSpace(LastName) ? null : LastName,
                    NotificationsEnabled: NotificationsEnabled,
                    QuietHoursStart: string.IsNullOrWhiteSpace(QuietHoursStart) ? null : QuietHoursStart,
                    QuietHoursEnd: string.IsNullOrWhiteSpace(QuietHoursEnd) ? null : QuietHoursEnd));

            await authService.UpdateProfileAsync(request, ct);
            await toastService.ShowSuccess(AppResources.EditProfileSuccess);
            await nav.GoBackAsync();
        }
        catch (HttpRequestException ex)
        {
            var (message, fieldErrors, _) = ApiErrorParser.Parse(ex.Message, AppResources.EditProfileError);

            if (fieldErrors?.TryGetValue("phone", out var phoneErr) == true)
            {
                PhoneError = phoneErr;
            }

            GeneralError = message;
        }
        catch (Exception ex)
        {
            GeneralError = AppResources.EditProfileError;
            await toastService.ShowError(ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [GeneratedRegex(@"^([01]\d|2[0-3]):[0-5]\d$")]
    private static partial Regex TimeRegex();
}
