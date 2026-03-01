namespace SportowyHub.Services.Navigation;

public class ShellNavigationService : INavigationService
{
    public Task GoToAsync(string route)
    {
        return Shell.Current.GoToAsync(route);
    }

    public Task GoBackAsync()
    {
        return Shell.Current.GoToAsync("..");
    }

    public Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel)
    {
        return Shell.Current.DisplayAlertAsync(title, message, accept, cancel);
    }

    public Task DisplayAlertAsync(string title, string message, string cancel)
    {
        return Shell.Current.DisplayAlertAsync(title, message, cancel);
    }
}
