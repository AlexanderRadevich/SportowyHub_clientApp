namespace SportowyHub.Services.Toast;

public interface IToastService
{
    Task ShowError(string message);
    Task ShowSuccess(string message);
}
