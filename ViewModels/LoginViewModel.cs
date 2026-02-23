using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SportowyHub.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _isPasswordVisible;

    [RelayCommand]
    private async Task Login()
    {
        // UI-only MVP — no backend call
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private void TogglePasswordVisibility()
    {
        IsPasswordVisible = !IsPasswordVisible;
    }

    [RelayCommand]
    private async Task NavigateToRegister()
    {
        await Shell.Current.GoToAsync("//register");
    }
}