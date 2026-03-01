namespace SportowyHub.Services.Navigation;

public interface INavigationService
{
    Task GoToAsync(string route);
    Task GoBackAsync();
    Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel);
    Task DisplayAlertAsync(string title, string message, string cancel);
}
